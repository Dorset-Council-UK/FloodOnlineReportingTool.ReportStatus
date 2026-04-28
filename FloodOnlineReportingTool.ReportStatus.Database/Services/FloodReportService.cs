using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodReportService(
    ILogger<FloodReportService> logger, 
    IFloodReportRepository floodReportRepository,
    IFloodReportSourceDuplicateRepository floodReportSourceRepository,
    ApplicationDbContext context
    ) : IFloodReportService
{
    int MaxBuffer = 25; // TODO - Check this

    public async Task CreateFromFloodReportCreated(FloodReportSourceCreated message, CancellationToken ct)
    {
        var floodReportId = Guid.CreateVersion7();
        var floodSourceId = Guid.CreateVersion7();  
        var (floodCauses, organisations) = message.ToSourceCollections(message.Id);

        FloodReportSource messageSource = new FloodReportSource()
        {
            Id = floodSourceId,
            FloodReportId = floodReportId, 
            Reference = message.Reference,
            ViewUri = message.ViewUri,
            CreatedUtc = message.CreatedUtc,
            UpdatedUtc = null,
            EligibilityCheckId = message.EligibilityCheckRecord.Id,
            Uprn = message.EligibilityCheckRecord.Uprn,
            Usrn = message.EligibilityCheckRecord.Usrn,
            Geom = new Point(message.EligibilityCheckRecord.Easting, message.EligibilityCheckRecord.Northing) { SRID = 27700 },
            ImpactStartUtc = message.EligibilityCheckRecord.ImpactStartUTC,
            ImpactDurationHours = message.EligibilityCheckRecord.ImpactDurationHours,
            IsOnGoing = message.EligibilityCheckRecord.IsOnGoing,
            IsUninhabitable = message.EligibilityCheckRecord.IsUninhabitable,
            VulnerableCount = message.EligibilityCheckRecord.VulnerableCount,
            LocationDescription = message.EligibilityCheckRecord.LocationDescription,
            FloodCauses = floodCauses,
            Organisations = organisations,
        };

        await GenerateFromSource(messageSource, message.EligibilityCheckRecord, message.Buffer, ct);
    }

    public async Task<FloodReport?> CreateFloodReport(FloodReportSource dto, double buffer, CancellationToken ct)
    {
        // Reverse-map wrapper types back to contract types for EligibilityCheckRecord
        IReadOnlyCollection<EligibilityCheckOrganisation> organisations = [.. dto.Organisations
            .Select(org => new EligibilityCheckOrganisation(org.OrganisationId, org.OrganisationName, org.FloodAuthorityId, org.FloodAuthorityName))];

        IReadOnlyCollection<EligibilityCheckFloodSource> floodSources = [.. dto.FloodCauses
            .Select(fc => new EligibilityCheckFloodSource(fc.CauseId, fc.Name))];

        EligibilityCheckRecord eligibilityCheckRecord = new EligibilityCheckRecord(
            dto.Id,
            dto.Uprn,
            dto.Usrn,
            dto.Easting,
            dto.Northing,
            dto.ImpactStartUtc,
            dto.ImpactDurationHours ?? 1,
            dto.IsOnGoing,
            dto.IsUninhabitable,
            dto.VulnerableCount,
            dto.LocationDescription,
            organisations,
            floodSources
        );

        return await GenerateFromSource(dto, eligibilityCheckRecord, buffer, ct);
    }

    private async Task<FloodReport?> GenerateFromSource(FloodReportSource messageSource, EligibilityCheckRecord checkRecord, double buffer, CancellationToken ct)
    {
        // Check if this is a duplicate record
        IReadOnlyCollection<Guid> ExistingIds = [];
        FloodReport? generatedReport = null;
        FloodReportType ReportType;
        if (messageSource.Uprn.HasValue)
        {
            ReportType = FloodReportType.Property;
            ExistingIds = await FindPotentialDuplicates(ReportType, 0, checkRecord, ct);
            
        }
        else if (messageSource.Usrn.HasValue)
        {
            ReportType = FloodReportType.Road;
            ExistingIds = await FindPotentialDuplicates(ReportType, 0, checkRecord, ct);
        }
        else
        {
            ReportType = FloodReportType.Location;
            ExistingIds = await FindPotentialDuplicates(ReportType, buffer, checkRecord, ct);
        }

        // Build the flood report and the initial source
        if (ExistingIds.Count == 0)
        {
            // Add the new source
            var ReportStatusFloodReportId = Guid.CreateVersion7();
            generatedReport = new FloodReport()
            {
                Id = ReportStatusFloodReportId,
                RecordStatus = RecordStatusIds.New,
                RecordType = ReportType,
                CreatedUtc = DateTimeOffset.UtcNow,
                UpdatedUtc = null,
                SourceRecords = [messageSource]
            };

            context.FloodReports.Add(generatedReport);
            await context.SaveChangesAsync(ct);
        }
        else
        {
            // Get and update the existing records - We add new duplicates to all matches for a user to review.
            var existingReports = await context.FloodReports
                .Where(fr => ExistingIds.Contains(fr.Id))
                .ToListAsync(ct);

            if (existingReports.Count == 0)
            {
                return null;
            }

            foreach (var existingReport in existingReports)
            {
                // Add the source record to the existing report as needed
                var updatedReport = existingReport with
                {
                    UpdatedUtc = DateTimeOffset.UtcNow,
                    RecordStatus = RecordStatusIds.DuplicateDetected,
                };
                context.FloodReports.Update(updatedReport);
            }

            // Add the duplicate source record
            FloodReportSourceDuplicate duplicateSource = new FloodReportSourceDuplicate
            {
                Id = Guid.NewGuid(),
                Reference = messageSource.Reference,
                ViewUri = messageSource.ViewUri,
                CreatedUtc = DateTimeOffset.UtcNow,
                UpdatedUtc = null,
                EligibilityCheckId = messageSource.EligibilityCheckId,
                Uprn = messageSource.Uprn,
                Usrn = messageSource.Usrn,
                Geom = messageSource.Geom,
                Easting = messageSource.Easting,
                Northing = messageSource.Northing,
                Latitude = messageSource.Latitude,
                Longitude = messageSource.Longitude,
                ImpactStartUtc = messageSource.ImpactStartUtc,
                ImpactDurationHours = messageSource.ImpactDurationHours,
                IsOnGoing = messageSource.IsOnGoing,
                IsUninhabitable = messageSource.IsUninhabitable,
                VulnerableCount = messageSource.VulnerableCount,
                LocationDescription = messageSource.LocationDescription,
                FloodReports = existingReports
            };
            context.FloodReportSourceDuplicates.Add(duplicateSource);

            await context.SaveChangesAsync(ct);
        }

        return generatedReport;
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood reports from the database.");

        var query = floodReportRepository.GetFloodReports();
        return await query.ToListAsync(ct);
    }

    public async Task<FloodReport?> GetFloodReportByReference(string reference, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report from the database by reference.");

        var query = floodReportRepository.GetFloodReportByReference(reference);
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<FloodReport?> GetFloodReportById(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report from the database by ID.");

        var query = floodReportRepository.GetFloodReportById(id);
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with UPRN {Uprn} from the database.", uprn);

        var query = floodReportRepository.GetFloodReportsByUprn(uprn);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUsrn(long usrn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with USRN {Usrn} from the database.", usrn);

        var query = floodReportRepository.GetFloodReportsByUsrn(usrn);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports within {Metres} metres of Easting {Easting} and Northing {Northing}.", metres, easting, northing);

        metres = Math.Clamp(metres, 0, IFloodReportService.MaxMetres);

        return await floodReportRepository
            .GetFloodReportsWithinMetres(easting, northing, metres)
            .ToListAsync(ct);
    }

    public async Task<FloodReport?> UpdateFloodReport(Guid id, FloodReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood report with ID {FloodReportId} in the database.", id);

        var existing = await floodReportRepository
            .GetFloodReports()
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,
            RecordStatus = dto.RecordStatus,

            SourceRecords = dto.SourceRecords
        };

        context.FloodReports.Update(updated);
        await context.SaveChangesAsync(ct);

        return updated;
    }

    public async Task<bool> DeleteFloodReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report with ID {FloodReportId} from the database.", id);

        var existing = await context.FloodReports.FindAsync([id], ct);

        if (existing == null)
        {
            return false;
        }

        context.FloodReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task<bool> FloodReportExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report with ID {FloodReportId} exists in the database.", id);

        return await floodReportRepository
            .GetFloodReports()
            .AnyAsync(o => o.Id.Equals(id), ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByPoint(double easting, double northing, double buffer, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood locations within buffer {buffer} of {easting}, {northing} from the database.", buffer, easting, northing);

        buffer = Math.Clamp(buffer, 0, MaxBuffer);

        return await floodReportRepository
            .GetFloodReportsWithinMetres(easting, northing, buffer)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(FloodReportType reportType, double buffer, EligibilityCheckRecord ecr, CancellationToken ct)
    {
        ImmutableHashSet<Guid> newSourceIds = [.. ecr.FloodSources.Select(s => s.Id)];
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingStart = ecr.ImpactStartUTC?.AddDays(-bufferDays);
        var incomingEnd = ecr.IsOnGoing ? DateTimeOffset.UtcNow : ecr.ImpactStartUTC?.AddHours(ecr.ImpactDurationHours);

        // All three cases now query FloodReport directly via its SourceRecords collection,
        // since FloodProperty / FloodRoad / FloodLocation join tables have been removed from the model.
        var query = context.FloodReports
            .AsNoTracking()
            .Where(fr => fr.SourceRecords.Any(sr =>
                // Match on flood causes — at least one cause in common
                sr.FloodCauses.Any(fc => newSourceIds.Contains(fc.CauseId))
                // Date overlap
                && sr.ImpactStartUtc != null
                && incomingStart != null
                && incomingEnd != null
                && sr.ImpactStartUtc <= incomingEnd
                && sr.ImpactStartUtc.Value.AddHours(sr.ImpactDurationHours ?? 1) >= incomingStart
                // Match on location
                && (reportType == FloodReportType.Property
                        ? sr.Uprn == ecr.Uprn
                    : reportType == FloodReportType.Road
                        ? sr.Usrn == ecr.Usrn
                        // Location: geometry within buffer
                        : sr.Geom.Intersects(new Point(ecr.Easting, ecr.Northing) { SRID = 27700 }.Buffer(buffer)))
            ));

        return await query
            .Select(fr => fr.Id)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReportSourceDuplicate>?> GetPotentialDuplicatesById(Guid id, CancellationToken ct)
    {
        return await floodReportSourceRepository
            .GetFloodReportSourceDuplicateById(id)
            .ToListAsync(ct);
    }
}

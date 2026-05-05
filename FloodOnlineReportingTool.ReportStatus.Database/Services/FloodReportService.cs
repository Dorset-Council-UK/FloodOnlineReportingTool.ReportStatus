using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Collections.Immutable;
using System.Security.Cryptography;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodReportService(
    ILogger<FloodReportService> logger, 
    IFloodReportRepository floodReportRepository,
    IConfiguration configuration,
    IFloodReportSourceDuplicateRepository floodReportSourceRepository,
    ApplicationDbContext context
    ) : IFloodReportService
{
    int MaxBuffer = configuration.GetValue<int>("ReportStatus:LocationBufferMetres");
    int GroundwaterCapDays = configuration.GetValue<int>("ReportStatus:GroundwaterCapDays");
    int FloodingCapDays = configuration.GetValue<int>("ReportStatus:FloodingCapDays");

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

    public async Task<List<FloodReport>?> CreateFloodReport(FloodReportSource dto, double buffer, CancellationToken ct)
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

    private async Task<List<FloodReport>?> GenerateFromSource(FloodReportSource messageSource, EligibilityCheckRecord checkRecord, double buffer, CancellationToken ct)
    {
        // Check if this is a duplicate record
        IReadOnlyCollection<Guid> ExistingIds = [];
        var generatedOrUpdatedReports = new List<FloodReport>();
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
            var generatedReport = new FloodReport()
            {
                Id = ReportStatusFloodReportId,
                Reference = await CreateReference(ct),
                RecordStatus = RecordStatusIds.New,
                RecordType = ReportType,
                CreatedUtc = DateTimeOffset.UtcNow,
                UpdatedUtc = null,
                SourceRecords = [messageSource]
            };

            context.FloodReports.Add(generatedReport);
            generatedOrUpdatedReports.Add(generatedReport);
            await context.SaveChangesAsync(ct);
        }
        else
        {
            // Get and update the existing records - We add new duplicates to all matches for a user to review.
            var existingReports = await context.FloodReports
                .AsNoTracking()
                .Where(fr => ExistingIds.Contains(fr.Id))
                .ToListAsync(ct);

            if (existingReports.Count == 0)
            {
                return null;
            }

            var duplicateDetectedUtc = DateTimeOffset.UtcNow;
            foreach (var existingReport in existingReports)
            {
                logger.LogInformation(
                    "Flood report {FloodReportId} status changed from {PreviousRecordStatus} to {NewRecordStatus} due to duplicate source {DuplicateSourceReference} at {ChangedUtc}.",
                    existingReport.Id,
                    existingReport.RecordStatus,
                    RecordStatusIds.DuplicateDetected,
                    messageSource.Reference,
                    duplicateDetectedUtc);
            }

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync(ct);
                try
                {
                    // Add the source record to the existing report as needed
                    await context.FloodReports
                        .Where(fr => ExistingIds.Contains(fr.Id))
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(fr => fr.RecordStatus, RecordStatusIds.DuplicateDetected)
                            .SetProperty(fr => fr.UpdatedUtc, duplicateDetectedUtc), ct);

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
                        FloodCauses = messageSource.FloodCauses?
                            .Select(fc => new FloodReportSourceFloodCause
                            {
                                Id = Guid.NewGuid(),
                                CauseId = fc.CauseId,
                                Name = fc.Name
                                // FloodReportSourceDuplicateId set by EF via navigation
                            }).ToList() ?? [],
                        Organisations = messageSource.Organisations?
                            .Select(org => new FloodReportSourceOrganisation
                            {
                                Id = Guid.NewGuid(),
                                OrganisationId = org.OrganisationId,
                                OrganisationName = org.OrganisationName,
                                FloodAuthorityId = org.FloodAuthorityId,
                                FloodAuthorityName = org.FloodAuthorityName
                            }).ToList() ?? [],
                        FloodReportReferences = existingReports
                            .Select(r => new FloodReportDuplicateReference
                            {
                                Id = Guid.NewGuid(),
                                FloodReportId = r.Id,
                                PreviousRecordStatus = r.RecordStatus
                            }).ToList()
                    };
                    context.FloodReportSourceDuplicates.Add(duplicateSource);

                    await context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            });

            generatedOrUpdatedReports = existingReports
                .Select(r => r with
                {
                    RecordStatus = RecordStatusIds.DuplicateDetected,
                    UpdatedUtc = duplicateDetectedUtc,
                })
                .ToList();
        }

        return generatedOrUpdatedReports;
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

    public async Task<bool> ReportWithReferenceExists(string reference, CancellationToken ct)
    {
        logger.LogInformation("Checking existence of flood report with reference number {Reference}.", reference);
        return await context.FloodReports
            .AsNoTracking()
            .AnyAsync(o => o.Reference == reference, ct);
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
            .AsNoTracking()
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct);

        if (existing is null)
        {
            return null;
        }

        await context.FloodReports
            .Where(fr => fr.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(fr => fr.RecordStatus, dto.RecordStatus)
                .SetProperty(fr => fr.SourceRecords, dto.SourceRecords)
                .SetProperty(fr => fr.UpdatedUtc, DateTimeOffset.UtcNow), ct);

        return existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,
            RecordStatus = dto.RecordStatus,
            SourceRecords = dto.SourceRecords,
        };
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

    public async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(FloodReportType reportType, double buffer, EligibilityCheckRecord incomingRecord, CancellationToken ct)
    {
        ImmutableHashSet<Guid> incomingAuthorityIds = [.. incomingRecord.Organisations.Select(o => o.FloodAuthorityId)];
        ImmutableHashSet<Guid> newSourceIds = [.. incomingRecord.FloodSources.Select(s => s.Id)];

        // Groundwater cap applies if groundwater is any of the causes, even alongside others
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingCapDays = isGroundWaterSource ? GroundwaterCapDays : FloodingCapDays;

        var incomingStart = incomingRecord.ImpactStartUTC?.AddDays(-bufferDays);

        // Cap ongoing events so that a flood marked as ongoing years ago cannot
        // incorrectly match against a new report today.
        var incomingEnd = ResolveEffectiveEnd(
            incomingRecord.ImpactStartUTC,
            incomingRecord.IsOnGoing,
            incomingRecord.ImpactDurationHours,
            incomingCapDays);

        // Capture as locals so EF Core can capture them in the query expression
        var groundwaterCapDays = GroundwaterCapDays;
        var floodingCapDays = FloodingCapDays;

        var query = context.FloodReports
            .AsNoTracking()
            .Where(fr => fr.SourceRecords.Any(sr =>
                // Match on flood authority as this is more forgiving than matching on cause. If the record is for an existing flood authority (regardless of cause) then we flag it as a potential duplicate for review.
                // Falls back to just matching if the incoming has no organisations (e.g. bad data)
                (!incomingAuthorityIds.Any() || sr.Organisations.Any(o => incomingAuthorityIds.Contains(o.FloodAuthorityId)))
                // Date overlap: db.start <= incoming.end
                && sr.ImpactStartUtc != null
                && incomingStart != null
                && incomingEnd != null
                && sr.ImpactStartUtc <= incomingEnd
                // Date overlap: db.end >= incoming.start
                // Ongoing DB records are capped by cause — groundwater cap applies if it is any of the causes.
                && (sr.IsOnGoing
                    ? (sr.FloodCauses.Any(fc => fc.CauseId == PrimaryCauseIds.WaterRisingOutOfTheGround)
                        ? sr.ImpactStartUtc.Value.AddDays(groundwaterCapDays)
                        : sr.ImpactStartUtc.Value.AddDays(floodingCapDays))
                    : sr.ImpactStartUtc.Value.AddHours(sr.ImpactDurationHours ?? 1)) >= incomingStart
                // Match on location
                && (reportType == FloodReportType.Property
                        ? sr.Uprn == incomingRecord.Uprn
                    : reportType == FloodReportType.Road
                        ? sr.Usrn == incomingRecord.Usrn
                        // Location: geometry within buffer
                        : sr.Geom.Intersects(new Point(incomingRecord.Easting, incomingRecord.Northing) { SRID = 27700 }.Buffer(buffer)))
            ));

        return await query
            .Select(fr => fr.Id)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReportSourceDuplicate>?> GetPotentialDuplicatesById(Guid id, CancellationToken ct)
    {
        var duplicates = await floodReportSourceRepository
            .GetFloodReportSourceDuplicateById(id)
            .ToListAsync(ct);

        return duplicates.Count == 0 ? null : duplicates;
    }

    /// <summary>
    /// Resolves the effective end datetime for a flood event.
    /// Ongoing events are capped at <paramref name="ongoingCapDays"/> past their start rather than
    /// using <see cref="DateTimeOffset.UtcNow"/>, which would cause stale records to match indefinitely.
    /// </summary>
    private static DateTimeOffset? ResolveEffectiveEnd(DateTimeOffset? start, bool isOngoing, double durationHours, int ongoingCapDays)
    {
        if (start is null)
        {
            return null;
        }

        if (!isOngoing)
        {
            return start.Value.AddHours(durationHours);
        }

        var cap = start.Value.AddDays(ongoingCapDays);
        return DateTimeOffset.UtcNow < cap ? DateTimeOffset.UtcNow : cap;
    }

    private async Task<string> CreateReference(CancellationToken ct)
    {
        var reference = GenerateReferenceId();
        //check existence of reference
        var reportWithReferenceExists = await ReportWithReferenceExists(reference, ct);
        var iteration = 0;
        var maxIterations = 100;
        while (reportWithReferenceExists && iteration < maxIterations)
        {
            reference = GenerateReferenceId();
            reportWithReferenceExists = await ReportWithReferenceExists(reference, ct);
            iteration++;
        }
        if (!reportWithReferenceExists)
        {
            logger.LogInformation("Creating a new flood report reference number {Reference}.", reference);
            return reference;
        }

        logger.LogError("Could not generate a unique reference after {maxIterations} tries", maxIterations);
        throw new Exception("Could not generate a unique reference");
    }

    private const string CrockfordAlphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    private static string GenerateReferenceId()
    {
        var bytes = RandomNumberGenerator.GetBytes(5); // 40 bits
        ulong value = ((ulong)bytes[0] << 32) | ((ulong)bytes[1] << 24) |
                      ((ulong)bytes[2] << 16) | ((ulong)bytes[3] << 8) | bytes[4];

        Span<char> chars = stackalloc char[8];
        for (int i = 7; i >= 0; i--)
        {
            chars[i] = CrockfordAlphabet[(int)(value & 0x1F)];
            value >>= 5;
        }
        return new string(chars);
    }
}
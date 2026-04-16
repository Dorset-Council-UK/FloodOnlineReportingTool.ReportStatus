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

public class FloodPropertyService(
    ILogger<FloodPropertyService> logger, 
    ApplicationDbContext context, 
    IFloodPropertyRepository floodPropertyRepository
    ) : IFloodPropertyService
{
    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodProperties(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties from the database.");

        return await floodPropertyRepository
            .GetFloodProperties()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties for UPRN {Uprn} from the database.", uprn);

        return await floodPropertyRepository
            .GetFloodPropertiesWithReports()
            .Where(fp => fp.Uprn.Equals(uprn))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties and their flood reports from the database.");

        return await floodPropertyRepository
            .GetFloodPropertiesWithReports()
            .ToListAsync(ct);
    }

    public async Task<FloodProperty?> GetFloodProperty(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property with ID {FloodPropertyId} from the database.", id);

        return await floodPropertyRepository
            .GetFloodProperties()
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct);
    }

    public async Task<FloodProperty?> GetFloodPropertyWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property and its flood reports with ID {FloodPropertyId} from the database.", id);

        return await floodPropertyRepository
            .GetFloodPropertiesWithReports()
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct);
    }

    public async Task<FloodProperty> CreateFloodProperty(FloodPropertyDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood property in the database");

        var creating = new FloodProperty()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Uprn = dto.Uprn,
            Notes = dto.Notes,
            Geom = new Point(dto.Geom.X, dto.Geom.Y) { SRID = 27700 },
        };

        context.FloodProperties.Add(creating);
        await context.SaveChangesAsync(ct);

        return creating;
    }

//#pragma warning disable MA0051 // Method is too long
//    public async Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, string locationDescription, CancellationToken ct)
//#pragma warning restore MA0051 // Method is too long
//    {
//        var floodReport = await floodReportService.GetFloodReportByReference(ecc.FloodReportReference, ct);
//        if (floodReport is null)
//        {
//            throw new InvalidOperationException("Cannot create Flood Road without a Flood Report.");
//        }

//        if (ecc.Uprn is null)
//        {
//            throw new InvalidOperationException("Cannot create Flood Property without a UPRN.");
//        }
//        var uprn = ecc.Uprn.Value;

//        // Build the flood report and sources
//        var updatedFloodReport = new FloodReportSource()
//        {
//            Id = floodReport.Id,
//            Reference = floodReport.Reference,
//            ViewUri = floodReport.ViewUri,
//            CreatedUtc = floodReport.CreatedUtc,

//            EligibilityCheckId = ecc.Id,
//            Uprn = uprn,
//            Usrn = ecc.Usrn,
//            Geom = new Point(ecc.Easting, ecc.Northing) { SRID = 27700 },
//            ImpactStartUtc = ecc.ImpactStartUTC,
//            ImpactDurationHours = ecc.ImpactDurationHours,

//            Sources = [.. ecc.FloodSources.Select(ecfs =>
//                new FloodReportSource()
//                {
//                    FloodReportId = floodReport.Id,
//                    SourceId = ecfs.Id,
//                    CreatedUtc = floodReport.CreatedUtc,
//                    Name = ecfs.Name,
//                }),
//            ],
//        };

//        // TODO - handle this logic
//        var potentialDuplicates = await FindPotentialDuplicates(uprn, ecc, ct);

//        FloodProperty? floodProperty = null;
//        ICollection<FloodPropertyFloodReport> floodPropertyFloodReports = [];
//        if (potentialDuplicates.Count > 0)
//        {
//            foreach (var duplicateFloodPropertyId in potentialDuplicates)
//            {
//                floodPropertyFloodReports.Add(new FloodPropertyFloodReport()
//                {
//                    FloodPropertyId = duplicateFloodPropertyId,
//                    FloodReportId = floodReport.Id,
//                    CreatedUtc = floodReport.CreatedUtc,
//                });
//            }
//        }
        

//        context.FloodReports.Update(updatedFloodReport);
//        if (floodProperty is not null) context.FloodProperties.Add(floodProperty);
//        if (floodPropertyFloodReports.Count > 0) context.FloodPropertyReports.AddRange(floodPropertyFloodReports);

//        await context.SaveChangesAsync(ct);
//    }

    public async Task<FloodProperty?> UpdateFloodProperty(Guid id, FloodPropertyDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood property with ID {FloodPropertyId} in the database.", id);

        var existing = await floodPropertyRepository
            .GetFloodProperties()
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Uprn = dto.Uprn,
            Notes = dto.Notes,
            Geom = dto.Geom,
        };

        context.FloodProperties.Update(updated);
        await context.SaveChangesAsync(ct);

        return updated;
    }

    public async Task<bool> DeleteFloodProperty(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood property with ID {FloodPropertyId} from the database.", id);

        var existing = await context.FloodProperties.FindAsync([id], ct);

        if (existing == null)
        {
            return false;
        }

        context.FloodProperties.Remove(existing);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task<bool> FloodPropertyExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood property with ID {FloodPropertyId} exists in the database.", id);

        return await floodPropertyRepository
            .GetFloodProperties()
            .AnyAsync(o => o.Id.Equals(id), ct);
    }

    /// <summary>
    /// Searches for existing flood reports associated with a property (by UPRN) that are considered potential duplicates of a new flood report.
    /// A flood report is considered a potential duplicate if it shares at least one flood source (by <c>SourceId</c>) with the new flood report
    /// and its impact period overlaps with the new flood report's impact period (with a buffer).
    /// </summary>
    /// <param name="uprn">The UPRN of the property to search for existing flood reports.</param>
    /// <param name="ecr">The <see cref="EligibilityCheckCreated"/> message containing details of the new flood report.</param>
    /// <returns>
    /// A read-only collection of Flood property <see cref="Guid"/>'s that are potential duplicates based on source and date overlap criteria.
    /// </returns>
    public async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(long uprn, EligibilityCheckRecord ecr, CancellationToken ct)
    {
        ImmutableHashSet<Guid> newSourceIds = [.. ecr.FloodSources.Select(s => s.Id)];
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingStart = ecr.ImpactStartUTC?.AddDays(-bufferDays);
        var incomingEnd = ecr.IsOnGoing ? DateTimeOffset.UtcNow : ecr.ImpactStartUTC?.AddHours(ecr.ImpactDurationHours);

        return await context.FloodProperties
            .AsNoTracking()
            .Where(fp => fp.Uprn.Equals(uprn))
            .SelectMany(fp => fp.FloodReports
                .Where(fpfr =>
                    // At least one matching source
                    fpfr.FloodReport.FloodCauses.Any(src => newSourceIds.Contains(src.CauseId))
                    // Date overlap
                    && fpfr.FloodReport.ImpactStartUtc != null
                    && incomingStart != null
                    && incomingEnd != null
                    && fpfr.FloodReport.ImpactStartUtc <= incomingEnd
                    && fpfr.FloodReport.ImpactStartUtc.Value.AddHours(fpfr.FloodReport.ImpactDurationHours ?? 1) >= incomingStart
                )
                .OrderBy(fpfr => fpfr.FloodPropertyId)
                    .ThenBy(fpfr => fpfr.FloodReportId)
            )
            .Select(fpfr => fpfr.FloodPropertyId)
            .ToListAsync(ct);
    }
}

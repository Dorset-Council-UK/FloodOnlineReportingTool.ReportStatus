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

public class FloodRoadService(
    ILogger<FloodRoadService> logger, 
    ApplicationDbContext context, 
    IFloodRoadRepository floodRoadRepository
    ) : IFloodRoadService
{
    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoads(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood roads from the database.");

        return await floodRoadRepository
            .GetFloodRoads()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsByUsrn(long usrn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood roads for USRN {Usrn} from the database.", usrn);

        return await floodRoadRepository
            .GetFloodRoadsWithReports()
            .Where(fr => fr.Usrn.Equals(usrn))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood roads and their flood reports from the database.");

        return await floodRoadRepository
            .GetFloodRoadsWithReports()
            .ToListAsync(ct);
    }

    public async Task<FloodRoad?> GetFloodRoad(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood road with ID {FloodRoadId} from the database.", id);

        return await floodRoadRepository
            .GetFloodRoads()
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct);
    }

    public async Task<FloodRoad?> GetFloodRoadWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood road and its flood reports with ID {FloodRoadId} from the database.", id);

        return await floodRoadRepository
            .GetFloodRoadsWithReports()
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct);
    }

    public async Task<FloodRoad> CreateFloodRoad(FloodRoadDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood road in the database");

        var creating = new FloodRoad()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Usrn = dto.Usrn,
            Notes = dto.Notes,
            Geom = new Point(dto.Geom.X, dto.Geom.Y) { SRID = 27700 },
        };

        context.FloodRoads.Add(creating);
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

//        if (ecc.Usrn is null)
//        {
//            throw new InvalidOperationException("Cannot create Flood Road without a USRN.");
//        }

//        var usrn = ecc.Usrn.Value;

//        // Build the flood report and sources
//        var updatedFloodReport = new FloodReportSource()
//        {
//            Id = floodReport.Id,
//            Reference = floodReport.Reference,
//            ViewUri = floodReport.ViewUri,
//            CreatedUtc = floodReport.CreatedUtc,

//            EligibilityCheckId = ecc.Id,
//            Uprn = ecc.Uprn,
//            Usrn = usrn,
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
//        var potentialDuplicates = await FindPotentialDuplicates(usrn, ecc, ct);

//        FloodRoad? floodRoad = null;
//        ICollection<FloodRoadFloodReport> floodRoadFloodReports = [];
//        if (potentialDuplicates.Count > 0)
//        {
//            foreach (var duplicateFloodRoadId in potentialDuplicates)
//            {
//                floodRoadFloodReports.Add(new FloodRoadFloodReport()
//                {
//                    FloodRoadId = duplicateFloodRoadId,
//                    FloodReportId = floodReport.Id,
//                    CreatedUtc = floodReport.CreatedUtc,
//                });
//            }
//        }

//        context.FloodReports.Update(updatedFloodReport);
//        if (floodRoad is not null) context.FloodRoads.Add(floodRoad);
//        if (floodRoadFloodReports.Count > 0) context.FloodRoadFloodReports.AddRange(floodRoadFloodReports);

//        await context.SaveChangesAsync(ct);
//    }

    public async Task<FloodRoad?> UpdateFloodRoad(Guid id, FloodRoadDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood road with ID {FloodRoadId} in the database.", id);

        var existing = await floodRoadRepository
            .GetFloodRoads()
            .FirstOrDefaultAsync(o => o.Id.Equals(id), ct);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Usrn = dto.Usrn,
            Notes = dto.Notes,
            Geom = dto.Geom,
        };

        context.FloodRoads.Update(updated);
        await context.SaveChangesAsync(ct);

        return updated;
    }

    public async Task<bool> DeleteFloodRoad(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood road with ID {FloodRoadId} from the database.", id);

        var existing = await context.FloodRoads.FindAsync([id], ct);

        if (existing == null)
        {
            return false;
        }

        context.FloodRoads.Remove(existing);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task<bool> FloodRoadExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood road with ID {FloodRoadId} exists in the database.", id);

        return await floodRoadRepository
            .GetFloodRoads()
            .AnyAsync(o => o.Id.Equals(id), ct);
    }

    /// <summary>
    /// Searches for existing flood reports associated with a road (by USRN) that are considered potential duplicates of a new flood report.
    /// A flood report is considered a potential duplicate if it shares at least one flood source (by <c>SourceId</c>) with the new flood report
    /// and its impact period overlaps with the new flood report's impact period (with a buffer).
    /// </summary>
    /// <param name="usrn">The USRN of the road to search for existing flood reports.</param>
    /// <param name="ecr">The <see cref="EligibilityCheckCreated"/> message containing details of the new flood report.</param>
    /// <returns>
    /// A read-only collection of Flood road <see cref="Guid"/>'s that are potential duplicates based on source and date overlap criteria.
    /// </returns>
    public async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(long usrn, EligibilityCheckRecord ecr, CancellationToken ct)
    {
        ImmutableHashSet<Guid> newSourceIds = [.. ecr.FloodSources.Select(s => s.Id)];
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingStart = ecr.ImpactStartUTC?.AddDays(-bufferDays);
        var incomingEnd = ecr.IsOnGoing ? DateTimeOffset.UtcNow : ecr.ImpactStartUTC?.AddHours(ecr.ImpactDurationHours);

        return await context.FloodRoads
            .AsNoTracking()
            .Where(fr => fr.Usrn.Equals(usrn))
            .SelectMany(fr => fr.FloodReports
                .Where(frfr =>
                    // At least one matching source
                    frfr.FloodReport.FloodCauses.Any(src => newSourceIds.Contains(src.CauseId))
                    // Date overlap
                    && frfr.FloodReport.ImpactStartUtc != null
                    && incomingStart != null
                    && incomingEnd != null
                    && frfr.FloodReport.ImpactStartUtc <= incomingEnd
                    && frfr.FloodReport.ImpactStartUtc.Value.AddHours(frfr.FloodReport.ImpactDurationHours ?? 1) >= incomingStart
                )
                .OrderBy(frfr => frfr.FloodRoadId)
                    .ThenBy(frfr => frfr.FloodReportId)
            )
            .Select(frfr => frfr.FloodRoadId)
            .ToListAsync(ct);
    }
}

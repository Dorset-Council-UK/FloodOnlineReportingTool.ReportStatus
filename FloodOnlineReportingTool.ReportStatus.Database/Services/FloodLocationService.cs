using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodLocationService(ILogger<FloodLocationService> logger, ApplicationDbContext context) : IFloodLocationService
{
    public async Task<IReadOnlyCollection<FloodLocation>> GetFloodLocations(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood locations from the database.");

        return await context.FloodLocations
            .AsNoTracking()
            .OrderBy(fl => fl.Id)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodLocation>> GetFloodLocationsByPoint(double easting, double northing, double buffer, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood locations within buffer {buffer} of {easting}, {northing} from the database.", buffer, easting, northing);

        return await context.FloodLocations
            .AsNoTracking()
            .Where(fl => fl.Location.Intersects(new Point(easting, northing).Buffer(buffer)))
            .OrderBy(fl => fl.Id)
            .Include(fl => fl.FloodReports.OrderBy(fr => fr.FloodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodLocation>> GetFloodLocationsWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood locations and their flood reports from the database.");

        return await context.FloodLocations
            .AsNoTracking()
            .OrderBy(fl => fl.Id)
            .Include(fl => fl.FloodReports.OrderBy(fr => fr.FloodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodLocation?> GetFloodLocation(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood location with ID {FloodLocationId} from the database.", id);

        return await context.FloodLocations
            .FindAsync([id], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodLocation?> GetFloodLocationWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood location and its flood reports with ID {FloodLocationId} from the database.", id);

        var location = await context.FloodLocations
            .AsNoTracking()
            .OrderBy(fl => fl.Id)
            .Include(fl => fl.FloodReports.OrderBy(fr => fr.FloodReportId))
            .FirstOrDefaultAsync(fl => fl.Id.Equals(id), ct)
            .ConfigureAwait(false);

        return location;
    }

    public async Task<FloodLocation> CreateFloodLocation(FloodLocationDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood location in the database");

        var creating = new FloodLocation()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Notes = dto.Notes,
            Location = dto.Location,
        };

        context.FloodLocations.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, string locationDescription, double buffer, CancellationToken ct)
    {
        var createdUtc = DateTimeOffset.UtcNow;
        var floodReportId = Guid.CreateVersion7();

        // Build the flood report and sources
        var floodReport = new FloodReport()
        {
            Id = floodReportId,
            CreatedUtc = createdUtc,

            EligibilityCheckId = ecc.Id,
            Uprn = ecc.Uprn,
            Usrn = ecc.Usrn,
            Easting = ecc.Easting,
            Northing = ecc.Northing,
            ImpactStartUtc = ecc.ImpactStartUTC,
            ImpactDurationHours = ecc.ImpactDurationHours,

            Sources = [.. ecc.FloodSources.Select(ecfs =>
                new FloodReportSource()
                {
                    FloodReportId = floodReportId,
                    SourceId = ecfs.Id,
                    CreatedUtc = createdUtc,
                    Name = ecfs.Name,
                }),
            ],
        };

        var potentialDuplicates = await FindPotentialDuplicates(buffer, ecc, ct).ConfigureAwait(false);

        FloodLocation? floodLocation = null;
        ICollection<FloodLocationFloodReport> floodLocationFloodReports = [];
        if (potentialDuplicates.Count > 0)
        {
            foreach (var duplicateFloodLocationId in potentialDuplicates)
            {
                floodLocationFloodReports.Add(new FloodLocationFloodReport()
                {
                    FloodLocationId = duplicateFloodLocationId,
                    FloodReportId = floodReportId,
                    CreatedUtc = createdUtc,
                });
            }
        }
        else
        {
            var floodLocationId = Guid.CreateVersion7();
            floodLocation = new FloodLocation()
            {
                Id = floodLocationId,
                CreatedUtc = createdUtc,

                Title = locationDescription,
                Notes = null,
                Location = new Point(ecc.Easting, ecc.Northing),

                FloodReports = [
                    new()
                    {
                        FloodLocationId = floodLocationId,
                        FloodReportId = floodReportId,
                        CreatedUtc = createdUtc,
                    },
                ],
            };
        }

        context.FloodReports.Add(floodReport);
        if (floodLocation is not null) context.FloodLocations.Add(floodLocation);
        if (floodLocationFloodReports.Count > 0) context.FloodLocationFloodReports.AddRange(floodLocationFloodReports);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task<FloodLocation?> UpdateFloodLocation(Guid id, FloodLocationDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood location with ID {FloodLocationId} in the database.", id);

        var existing = await context.FloodLocations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Notes = dto.Notes,
            Location = dto.Location,
        };

        context.FloodLocations.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodLocation(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood location with ID {FloodLocationId} from the database.", id);

        var existing = await context.FloodLocations
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodLocations.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodLocationExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood location with ID {FloodLocationId} exists in the database.", id);

        return await context.FloodLocations
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for existing flood reports associated with a location (by Easting, Northing, with a distance buffer) that are considered potential duplicates of a new flood report.
    /// A flood report is considered a potential duplicate if it shares at least one flood source (by <c>SourceId</c>) with the new flood report
    /// and its impact period overlaps with the new flood report's impact period (with a buffer).
    /// </summary>
    /// <param name="buffer">The distance buffer (in metres) around the Easting and Northing coordinates to include in the search.</param>
    /// <param name="ecc">The <see cref="EligibilityCheckCreated"/> message containing details of the new flood report.</param>
    /// <returns>
    /// A read-only collection of Flood location <see cref="Guid"/>'s that are potential duplicates based on source and date overlap criteria.
    /// </returns>
    private async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(double buffer, EligibilityCheckCreated ecc, CancellationToken ct)
    {
        ImmutableHashSet<Guid> newSourceIds = [.. ecc.FloodSources.Select(s => s.Id)];
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingStart = ecc.ImpactStartUTC?.AddDays(-bufferDays);
        var incomingEnd = ecc.IsOnGoing ? DateTimeOffset.UtcNow : ecc.ImpactStartUTC?.AddHours(ecc.ImpactDurationHours);

        return await context.FloodLocations
            .AsNoTracking()
            .Where(fl => fl.Location.Intersects(new Point(ecc.Easting, ecc.Northing).Buffer(buffer)))
            .SelectMany(fl => fl.FloodReports
                .Where(flfr =>
                    // At least one matching source
                    flfr.FloodReport.Sources.Any(src => newSourceIds.Contains(src.SourceId))
                    // Date overlap
                    && flfr.FloodReport.ImpactStartUtc != null
                    && incomingStart != null
                    && incomingEnd != null
                    && flfr.FloodReport.ImpactStartUtc <= incomingEnd
                    && flfr.FloodReport.ImpactStartUtc.Value.AddHours(flfr.FloodReport.ImpactDurationHours) >= incomingStart
                )
                .OrderBy(flfr => flfr.FloodLocationId)
                    .ThenBy(flfr => flfr.FloodReportId)
            )
            .Select(flfr => flfr.FloodLocationId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}

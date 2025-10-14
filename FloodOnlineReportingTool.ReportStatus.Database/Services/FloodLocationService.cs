using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

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
            .OrderBy(fl => fl.Id)
            .Where(fl => fl.Location.Intersects(new Point(easting, northing).Buffer(buffer)))
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
}

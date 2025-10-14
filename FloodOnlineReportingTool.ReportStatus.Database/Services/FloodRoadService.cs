using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodRoadService(ILogger<FloodRoadService> logger, ApplicationDbContext context) : IFloodRoadService
{
    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoads(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood roads from the database.");

        return await context.FloodRoads
            .AsNoTracking()
            .OrderBy(fr => fr.Id)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsByUsrn(long Usrn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties for USRN {Usrn} from the database.", Usrn);

        return await context.FloodRoads
            .AsNoTracking()
            .OrderBy(fr => fr.Id)
            .Where(fr => fr.Usrn.Equals(Usrn))
            .Include(fr => fr.FloodReports.OrderBy(frfr => frfr.FloodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood roads and their flood reports from the database.");

        return await context.FloodRoads
            .AsNoTracking()
            .OrderBy(fr => fr.Id)
            .Include(fr => fr.FloodReports.OrderBy(frfr => frfr.FloodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodRoad?> GetFloodRoad(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood road with ID {FloodRoadId} from the database.", id);

        return await context.FloodRoads
            .FindAsync([id], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodRoad?> GetFloodRoadWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood road and its flood reports with ID {FloodRoadId} from the database.", id);

        return await context.FloodRoads
            .AsNoTracking()
            .OrderBy(fr => fr.Id)
            .Include(fr => fr.FloodReports.OrderBy(frfr => frfr.FloodReportId))
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct)
            .ConfigureAwait(false);
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
            Location = dto.Location,
        };

        context.FloodRoads.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodRoad?> UpdateFloodRoad(Guid id, FloodRoadDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood road with ID {FloodRoadId} in the database.", id);

        var existing = await context.FloodRoads
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

        context.FloodRoads.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodRoad(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood road with ID {FloodRoadId} from the database.", id);

        var existing = await context.FloodRoads
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodRoads.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodRoadExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood road with ID {FloodRoadId} exists in the database.", id);

        return await context.FloodRoads
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }
}

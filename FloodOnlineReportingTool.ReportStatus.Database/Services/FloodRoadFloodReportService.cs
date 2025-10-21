using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodRoadFloodReportService(ILogger<FloodRoadFloodReportService> logger, ApplicationDbContext context) : IFloodRoadFloodReportService
{
    public async Task<IReadOnlyCollection<FloodRoadFloodReport>> GetFloodRoadFloodReports(Guid floodRoadId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood report relationships for flood road {FloodRoadId} from the database.", floodRoadId);

        return await context.FloodRoadFloodReports
            .AsNoTracking()
            .Where(frfr => frfr.FloodRoadId.Equals(floodRoadId))
            .OrderBy(frfr => frfr.FloodRoadId)
            .ThenBy(frfr => frfr.FloodReportId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodRoadFloodReport?> GetFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving the flood report relationship for the flood road with IDs {FloodRoadId}/{FloodReportId} from the database.", floodRoadId, floodReportId);

        return await context.FloodRoadFloodReports
            .FindAsync([floodRoadId, floodReportId], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodRoadFloodReport> CreateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Creating a new unconfirmed flood report relationship for the flood road with IDs {FloodRoadId}/{FloodReportId} from the database.", floodRoadId, floodReportId);

        var creating = new FloodRoadFloodReport()
        {
            FloodRoadId = floodRoadId,
            FloodReportId = floodReportId,
            CreatedUtc = DateTimeOffset.UtcNow,
        };

        context.FloodRoadFloodReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodRoadFloodReport> CreateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Creating a new confirmed flood report relationship for the flood road with IDs {FloodRoadId}/{FloodReportId} from the database.", floodRoadId, floodReportId);

        var now = DateTimeOffset.UtcNow;
        var creating = new FloodRoadFloodReport()
        {
            FloodRoadId = floodRoadId,
            FloodReportId = floodReportId,
            CreatedUtc = now,

            ConfirmedUtc = now,
            ConfirmedByUserId = userId,
            ConfirmedUserName = userName,
        };

        context.FloodRoadFloodReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodRoadFloodReport?> ConfirmFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Flood road, flood report {FloodRoadId}/{FloodReportId} confirmed by user {UserId}/{UserName}.", floodRoadId, floodReportId, userId, userName);

        var existing = await context.FloodRoadFloodReports
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.FloodRoadId.Equals(floodRoadId) && o.FloodReportId.Equals(floodReportId), ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        var updated = existing with
        {
            UpdatedUtc = now,

            ConfirmedUtc = now,
            ConfirmedByUserId = userId,
            ConfirmedUserName = userName,
        };

        context.FloodRoadFloodReports.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report for the flood road with IDs {FloodRoadId}/{FloodReportId} from the database.", floodRoadId, floodReportId);

        var existing = await context.FloodRoadFloodReports
            .FindAsync([floodRoadId, floodReportId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodRoadFloodReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodRoadFloodReportExists(Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report for the flood road with IDs {FloodRoadId}/{FloodReportId} exists in the database.", floodRoadId, floodReportId);

        return await context.FloodRoadFloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.FloodRoadId.Equals(floodRoadId) && o.FloodReportId.Equals(floodReportId), ct)
            .ConfigureAwait(false);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodLocationFloodReportService(ILogger<FloodLocationFloodReportService> logger, ApplicationDbContext context) : IFloodLocationFloodReportService
{
    public async Task<IReadOnlyCollection<FloodLocationFloodReport>> GetFloodLocationFloodReports(Guid floodLocationId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood report relationships for flood location {FloodLocationId} from the database.", floodLocationId);

        return await context.FloodLocationFloodReports
            .AsNoTracking()
            .OrderBy(flr => flr.FloodLocationId)
            .ThenBy(flr => flr.FloodReportId)
            .Where(flr => flr.FloodLocationId.Equals(floodLocationId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodLocationFloodReport?> GetFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving the flood report relationship for the flood location with IDs {FloodLocationId}/{FloodReportId} from the database.", floodLocationId, floodReportId);

        return await context.FloodLocationFloodReports
            .FindAsync([floodLocationId, floodReportId], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodLocationFloodReport> CreateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Creating a new unconfirmed flood report relationship for the flood location with IDs {FloodLocationId}/{FloodReportId} from the database.", floodLocationId, floodReportId);

        var creating = new FloodLocationFloodReport()
        {
            FloodLocationId = floodLocationId,
            FloodReportId = floodReportId,
            CreatedUtc = DateTimeOffset.UtcNow,
        };

        context.FloodLocationFloodReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodLocationFloodReport> CreateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Creating a new confirmed flood report relationship for the flood location with IDs {FloodLocationId}/{FloodReportId} from the database.", floodLocationId, floodReportId);

        var now = DateTimeOffset.UtcNow;
        var creating = new FloodLocationFloodReport()
        {
            FloodLocationId = floodLocationId,
            FloodReportId = floodReportId,
            CreatedUtc = now,

            ConfirmedUtc = now,
            ConfirmedByUserId = userId,
            ConfirmedUserName = userName,
        };

        context.FloodLocationFloodReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodLocationFloodReport?> ConfirmFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Flood location, flood report {FloodLocationId}/{FloodReportId} confirmed by user {UserId}/{UserName}.", floodLocationId, floodReportId, userId, userName);

        var existing = await context.FloodLocationFloodReports
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.FloodLocationId.Equals(floodLocationId) && o.FloodReportId.Equals(floodReportId), ct)
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

        context.FloodLocationFloodReports.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report for the flood location with IDs {FloodLocationId}/{FloodReportId} from the database.", floodLocationId, floodReportId);

        var existing = await context.FloodLocationFloodReports
            .FindAsync([floodLocationId, floodReportId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodLocationFloodReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodLocationFloodReportExists(Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report for the flood location with IDs {FloodLocationId}/{FloodReportId} exists in the database.", floodLocationId, floodReportId);

        return await context.FloodLocationFloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.FloodLocationId.Equals(floodLocationId) && o.FloodReportId.Equals(floodReportId), ct)
            .ConfigureAwait(false);
    }
}

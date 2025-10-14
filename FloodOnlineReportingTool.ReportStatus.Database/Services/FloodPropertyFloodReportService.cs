using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodPropertyFloodReportService(ILogger<FloodPropertyFloodReportService> logger, ApplicationDbContext context) : IFloodPropertyFloodReportService
{
    public async Task<IReadOnlyCollection<FloodPropertyFloodReport>> GetFloodPropertyFloodReports(Guid floodPropertyId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood report relationships for flood property {FloodPropertyId} from the database.", floodPropertyId);

        return await context.FloodPropertyReports
            .AsNoTracking()
            .OrderBy(fpr => fpr.FloodPropertyId)
            .ThenBy(fpr => fpr.FloodReportId)
            .Where(o => o.FloodPropertyId.Equals(floodPropertyId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodPropertyFloodReport?> GetFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving the flood report relationship for the flood property with IDs {FloodPropertyId}/{FloodReportId} from the database.", floodPropertyId, floodReportId);

        return await context.FloodPropertyReports
            .FindAsync([floodPropertyId, floodReportId], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodPropertyFloodReport> CreateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Creating a new unconfirmed flood report relationship for the flood property with IDs {FloodPropertyId}/{FloodReportId} from the database.", floodPropertyId, floodReportId);

        var creating = new FloodPropertyFloodReport()
        {
            FloodPropertyId = floodPropertyId,
            FloodReportId = floodReportId,
            CreatedUtc = DateTimeOffset.UtcNow,
        };

        context.FloodPropertyReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodPropertyFloodReport> CreateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Creating a new confirmed flood report relationship for the flood property with IDs {FloodPropertyId}/{FloodReportId} from the database.", floodPropertyId, floodReportId);

        var now = DateTimeOffset.UtcNow;
        var creating = new FloodPropertyFloodReport()
        {
            FloodPropertyId = floodPropertyId,
            FloodReportId = floodReportId,
            CreatedUtc = now,

            ConfirmedUtc = now,
            ConfirmedByUserId = userId,
            ConfirmedUserName = userName,
        };

        context.FloodPropertyReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodPropertyFloodReport?> ConfirmFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, string userId, string userName, CancellationToken ct)
    {
        logger.LogDebug("Flood property, flood report {FloodPropertyId}/{FloodReportId} confirmed by user {UserId}/{UserName}.", floodPropertyId, floodReportId, userId, userName);

        var existing = await context.FloodPropertyReports
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.FloodPropertyId.Equals(floodPropertyId) && o.FloodReportId.Equals(floodReportId), ct)
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

        context.FloodPropertyReports.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report for the flood property with IDs {FloodPropertyId}/{FloodReportId} from the database.", floodPropertyId, floodReportId);

        var existing = await context.FloodPropertyReports
            .FindAsync([floodPropertyId, floodReportId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodPropertyReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodPropertyFloodReportExists(Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report for the flood property with IDs {FloodPropertyId}/{FloodReportId} exists in the database.", floodPropertyId, floodReportId);

        return await context.FloodPropertyReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.FloodPropertyId.Equals(floodPropertyId) && o.FloodReportId.Equals(floodReportId), ct)
            .ConfigureAwait(false);
    }
}

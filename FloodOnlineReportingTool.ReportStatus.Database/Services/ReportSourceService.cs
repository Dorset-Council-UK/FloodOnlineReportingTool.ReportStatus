using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class ReportSourceService(ILogger<ReportSourceService> logger, ApplicationDbContext context) : IReportSourceService
{
    public async Task<IReadOnlyList<ReportSource>> GetReportSources(Guid reportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all report sources from the database.");

        return await context.ReportSources
            .AsNoTracking()
            .Where(o => o.ReportId == reportId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<ReportSource?> GetReportSource(Guid reportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving report source with ID {ReportId}/{SourceId} from the database.", reportId, sourceId);

        var reportSource = await context.ReportSources
            .FindAsync([reportId, sourceId], ct)
            .ConfigureAwait(false);

        return reportSource;
    }

    public async Task<ReportSource> CreateReportSource(Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new report source.");

        var newReportSource = new ReportSource()
        {
            ReportId = reportId,
            SourceId = sourceId,
            CreatedUtc = DateTimeOffset.UtcNow,
            Name = dto.Name,
        };

        context.ReportSources.Add(newReportSource);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newReportSource;
    }

    public async Task<ReportSource?> UpdateReportSource(Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating report source with ID {ReportId}/{SourceId} in the database.", reportId, sourceId);

        var existing = await context.ReportSources
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.ReportId == reportId && o.SourceId == sourceId, ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,
            Name = dto.Name,
        };

        context.ReportSources.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteReportSource(Guid reportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Delete report source with ID {ReportId}/{SourceId} from the database.", reportId, sourceId);

        var existing = await context.ReportSources
            .FindAsync([reportId, sourceId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.ReportSources.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> ReportSourceExists(Guid reportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Checking if report source with ID {ReportId}/{SourceId} exists in the database.", reportId, sourceId);

        return await context.ReportSources
            .AsNoTracking()
            .AnyAsync(o => o.ReportId == reportId && o.SourceId == sourceId, ct)
            .ConfigureAwait(false);
    }
}

using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodReportSourceService(ILogger<FloodReportSourceService> logger, ApplicationDbContext context) : IFloodReportSourceService
{
    public async Task<IReadOnlyCollection<FloodReportSource>> GetFloodReportSources(Guid floodReportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood report sources for {FloodReportId} from the database.", floodReportId);

        return await context.FloodReportSources
            .AsNoTracking()
            .OrderBy(frs => frs.FloodReportId)
            .ThenBy(frs => frs.SourceId)
            .Where(frs => frs.FloodReportId.Equals(floodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodReportSource?> GetFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report source with ID {FloodReportId}/{SourceId} from the database.", floodReportId, sourceId);

        return await context.FloodReportSources
            .FindAsync([floodReportId, sourceId], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodReportSource> CreateFloodReportSource(Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood report source.");

        var newFloodReportSource = new FloodReportSource()
        {
            FloodReportId = floodReportId,
            SourceId = sourceId,
            CreatedUtc = DateTimeOffset.UtcNow,
            Name = dto.Name,
        };

        context.FloodReportSources.Add(newFloodReportSource);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newFloodReportSource;
    }

    public async Task CreateFloodReportSources(Guid floodReportId, IReadOnlyCollection<EligibilityCheckFloodSource> floodSources, CancellationToken ct)
    {
        logger.LogDebug("Creating multiple flood report sources for flood report ID {FloodReportId}.", floodReportId);

        var now = DateTimeOffset.UtcNow;
        var newFloodReportSources = floodSources.Select(fs => new FloodReportSource()
        {
            FloodReportId = floodReportId,
            SourceId = fs.Id,
            CreatedUtc = now,
            Name = fs.Name,
        });

        context.FloodReportSources.AddRange(newFloodReportSources);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task<FloodReportSource?> UpdateFloodReportSource(Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood report source with ID {FloodReportId}/{SourceId} in the database.", floodReportId, sourceId);

        var existing = await context.FloodReportSources
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.FloodReportId.Equals(floodReportId) && o.SourceId.Equals(sourceId), ct)
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

        context.FloodReportSources.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report source with ID {FloodReportId}/{SourceId} from the database.", floodReportId, sourceId);

        var existing = await context.FloodReportSources
            .FindAsync([floodReportId, sourceId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodReportSources.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodReportSourceExists(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report source with ID {FloodReportId}/{SourceId} exists in the database.", floodReportId, sourceId);

        return await context.FloodReportSources
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.FloodReportId.Equals(floodReportId) && o.SourceId.Equals(sourceId), ct)
            .ConfigureAwait(false);
    }
}

using FloodOnlineReportingTool.Contracts.Shared.Models;
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
            .Where(frs => frs.FloodReportId.Equals(floodReportId))
            .OrderBy(frs => frs.FloodReportId)
            .ThenBy(frs => frs.Id)
            .ToListAsync(ct);
    }

    public async Task<FloodReportSource?> GetFloodReportSourceByReference(Guid floodReportId, string sourceReference, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report source with reference from the database.");

        return await context.FloodReportSources
        .AsNoTracking()
        .Where(frs => frs.FloodReportId == floodReportId && frs.Reference == sourceReference)
        .FirstOrDefaultAsync(ct);
    }

    public async Task<FloodReportSource?> GetFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report source with ID {FloodReportId}/{SourceId} from the database.", floodReportId, sourceId);

        return await context.FloodReportSources
        .AsNoTracking()
        .Where(frs => frs.FloodReportId == floodReportId && frs.EligibilityCheckId == sourceId)
        .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> AddFloodReportFloodSource(Guid floodReportId, Guid sourceId, string name, CancellationToken ct)
    {
        logger.LogDebug("Adding a new flood source to report.");
        var currentSource = await GetFloodReportSource(floodReportId, sourceId, ct);

        if (currentSource == null)
        {
            logger.LogDebug("Flood report source not found.");
            return false;
        }

        var newFloodReportSource = new EligibilityCheckFloodSource(sourceId, name);

        // TODO - look at this function
        //currentSource.FloodCauses.Add(newFloodReportSource);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task AddManyFloodReportFloodSources(Guid floodReportId, IReadOnlyCollection<EligibilityCheckFloodSource> floodSources, CancellationToken ct)
    {
        logger.LogDebug("Creating multiple flood report flood sources for flood report ID {FloodReportId}.", floodReportId);
        var floodSourceRecords = await GetFloodReportSources(floodReportId, ct);

        var now = DateTimeOffset.UtcNow;
        var newFloodReportSources = floodSources.Select(fs => new EligibilityCheckFloodSource(
            fs.Id,
            fs.Name
        ));

        // TODO - look at this function
        //foreach ( var floodSource in floodSourceRecords)
        //{
        //    foreach( var cause in newFloodReportSources)
        //    {
        //        floodSource.FloodCauses.Add(cause);
        //    }
        //}
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteFloodReportFloodSource(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report flood source with ID {FloodReportId}/{SourceId} from the database.", floodReportId, sourceId);

        var currentSource = await GetFloodReportSource(floodReportId, sourceId, ct);

        if (currentSource == null)
        {
            logger.LogDebug("Flood report source not found.");
            return false;
        }

        // TODO - look at this function
        //currentSource.FloodCauses.Remove(currentSource.FloodCauses.FirstOrDefault(fc => fc.Id == sourceId));
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task<bool> FloodReportFloodSourceExists(Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood cause with ID {FloodReportId}/{SourceId} exists within a flood report source in the database.", floodReportId, sourceId);

        return await context.FloodReportSources
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.FloodReportId.Equals(floodReportId) && o.FloodCauses.Any(fc => fc.CauseId == sourceId), ct);
    }
}

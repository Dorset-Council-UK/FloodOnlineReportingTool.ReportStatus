using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class SearchService(ILogger<SearchService> logger, ApplicationDbContext context) : ISearchService
{
    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUPRN(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by UPRN");

        return await context.FloodReports
            .AsNoTracking()
            .Where(r => r.Uprn.Equals(uprn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByCoordinates(double easting, double northing, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by coordinates");

        return await context.FloodReports
            .AsNoTracking()
            .Where(r => r.Easting.Equals(easting) && r.Northing.Equals(northing))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByStartDate(DateTimeOffset startDateUtc, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by flood start date");

        return await context.FloodReports
            .AsNoTracking()
            .Where(r => r.ImpactStartUtc.Equals(startDateUtc))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}

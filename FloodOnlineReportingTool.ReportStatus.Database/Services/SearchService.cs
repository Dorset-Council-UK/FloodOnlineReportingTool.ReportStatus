using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class SearchService(ILogger<ReportService> logger, ApplicationDbContext context) : ISearchService
{
    public async Task<IReadOnlyList<Report>> GetReportsByUPRN(double uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by UPRN");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.Uprn.Equals(uprn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Report>> GetReportsByCoordinates(double easting, double northing, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by coordinates");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.Easting == easting && r.Northing == northing)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Report>> GetReportsByStartDate(DateTimeOffset startDateUtc, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by flood start date");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.ImpactStartUtc == startDateUtc)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}

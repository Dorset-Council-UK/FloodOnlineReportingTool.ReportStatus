using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class SearchService(ILogger<ReportService> logger, ApplicationDbContext context) : ISearchService
{
    public async Task<IReadOnlyList<Report>> GetReportsByUPRN(double Uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by UPRN");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.Uprn == Uprn)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Report>> GetReportsByCoordinates(double Easting, double Northing, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by coordinates");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.Easting == Easting)
            .Where(r => r.Northing == Northing)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Report>> GetReportsByStartDate(DateTimeOffset ImpactStartUtc, CancellationToken ct)
    {
        logger.LogDebug("Retrieving search results by flood start date");

        return await context.Reports
            .AsNoTracking()
            .Where(r => r.ImpactStartUtc == ImpactStartUtc)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}

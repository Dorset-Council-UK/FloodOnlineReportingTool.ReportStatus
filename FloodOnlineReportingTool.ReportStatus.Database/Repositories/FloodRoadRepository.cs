using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodRoadRepository(ApplicationDbContext context) : IFloodRoadRepository
{
    public IQueryable<FloodRoad> GetFloodRoads()
    {
        return context.FloodRoads
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodRoad> GetFloodRoadsWithReports()
    {
        return context.FloodRoads
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fr => fr.Id)
            .Include(fr => fr.FloodReports.OrderBy(frfr => frfr.FloodReportId));
    }
}

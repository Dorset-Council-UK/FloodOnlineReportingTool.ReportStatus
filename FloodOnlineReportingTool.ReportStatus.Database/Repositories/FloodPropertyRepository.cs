using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodPropertyRepository(ApplicationDbContext context) : IFloodPropertyRepository
{
    public IQueryable<FloodProperty> GetFloodProperties()
    {
        return context.FloodProperties
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fp => fp.Id);
    }

    public IQueryable<FloodProperty> GetFloodPropertiesWithReports()
    {
        return context.FloodProperties
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fp => fp.Id)
            .Include(fp => fp.FloodReports.OrderBy(fr => fr.FloodReportId));
    }
}

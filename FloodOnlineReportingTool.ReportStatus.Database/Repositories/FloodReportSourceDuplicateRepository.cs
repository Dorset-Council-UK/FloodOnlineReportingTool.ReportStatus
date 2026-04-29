using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodReportSourceDuplicateRepository(ApplicationDbContext context) : IFloodReportSourceDuplicateRepository
{

    public IQueryable<FloodReportSourceDuplicate> GetFloodReportSourceDuplicateById(Guid floodReportId)
    {
        return context.FloodReportSourceDuplicates
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(frsd => frsd.FloodReports.Any(fr => fr.Id == floodReportId));
    }
}

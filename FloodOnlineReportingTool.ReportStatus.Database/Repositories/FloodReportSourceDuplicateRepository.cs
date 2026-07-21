using FloodOnlineReportingTool.Database.DbContexts;
using FloodOnlineReportingTool.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.Database.Repositories;

public class FloodReportSourceDuplicateRepository(ApplicationDbContext context) : IFloodReportSourceDuplicateRepository
{

    public IQueryable<FloodReportSourceDuplicate> GetFloodReportSourceDuplicateById(Guid floodReportId)
    {
        return context.FloodReportSourceDuplicates
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(frsd => frsd.FloodReportReferences.Any(frr => frr.FloodReportId == floodReportId));
    }
}

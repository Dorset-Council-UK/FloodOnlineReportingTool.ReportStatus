using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodEventRepository(ApplicationDbContext context) : IFloodEventRepository
{
    public IQueryable<FloodEvent> GetFloodEvents()
    {
        return context.FloodEvents
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodEvent?> GetFloodEventByReference(string reference)
    {
        return context.FloodEvents
            .AsNoTracking()
            .Where(fr => fr.Reference.Equals(reference))
            .OrderBy(frs => frs.Id);
    }

    public IQueryable<FloodEvent?> GetFloodEventById(Guid id)
    {
        return context.FloodEvents
            .AsNoTracking()
            .Where(fr => fr.Id.Equals(id))
            .OrderBy(frs => frs.Id);
    }
}

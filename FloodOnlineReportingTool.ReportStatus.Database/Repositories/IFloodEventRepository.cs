using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodEventRepository
{
    IQueryable<FloodEvent> GetFloodEvents();
    IQueryable<FloodEvent?> GetFloodEventByReference(string reference);
    IQueryable<FloodEvent?> GetFloodEventById(Guid id);
}

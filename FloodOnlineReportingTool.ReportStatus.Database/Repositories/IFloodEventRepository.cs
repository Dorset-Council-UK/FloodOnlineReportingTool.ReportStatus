using FloodOnlineReportingTool.Database.Models;

namespace FloodOnlineReportingTool.Database.Repositories;

public interface IFloodEventRepository
{
    IQueryable<FloodEvent> GetFloodEvents();
    IQueryable<FloodEvent?> GetFloodEventByReference(string reference);
    IQueryable<FloodEvent?> GetFloodEventById(Guid id);
}

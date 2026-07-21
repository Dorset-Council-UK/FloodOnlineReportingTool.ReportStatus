using FloodOnlineReportingTool.Database.Models;

namespace FloodOnlineReportingTool.Database.Repositories;

public interface IFloodReportSourceDuplicateRepository
{
    IQueryable<FloodReportSourceDuplicate> GetFloodReportSourceDuplicateById(Guid floodReportId);

}

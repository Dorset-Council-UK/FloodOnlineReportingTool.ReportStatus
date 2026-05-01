using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodReportSourceDuplicateRepository
{
    IQueryable<FloodReportSourceDuplicate> GetFloodReportSourceDuplicateById(Guid floodReportId);

}

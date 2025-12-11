using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodPropertyRepository
{
    IQueryable<FloodProperty> GetFloodProperties();
    IQueryable<FloodProperty> GetFloodPropertiesWithReports();
}

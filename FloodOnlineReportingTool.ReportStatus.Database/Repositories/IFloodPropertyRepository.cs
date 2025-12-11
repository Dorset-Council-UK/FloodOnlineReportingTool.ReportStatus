using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodPropertyRepository
{
    /// <summary>
    /// Gets flood properties from the database.
    /// </summary>
    IQueryable<FloodProperty> GetFloodProperties();
    /// <summary>
    /// Gets flood properties including their associated flood reports from the database.
    /// </summary>
    IQueryable<FloodProperty> GetFloodPropertiesWithReports();
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodRoadRepository
{
    /// <summary>
    /// Gets flood roads from the database.
    /// </summary>
    IQueryable<FloodRoad> GetFloodRoads();

    /// <summary>
    /// Gets flood roads including their associated flood reports from the database.
    /// </summary>
    IQueryable<FloodRoad> GetFloodRoadsWithReports();
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodLocationRepository
{
    /// <summary>
    /// Get all flood locations.
    /// </summary>
    IQueryable<FloodLocation> GetFloodLocations();

    /// <summary>
    /// Get flood locations including their associated flood reports.
    /// </summary>
    IQueryable<FloodLocation> GetFloodLocationsWithReports();

    /// <summary>
    /// Get flood locations within a certain buffer distance of a point defined by easting and northing.
    /// </summary>
    IQueryable<FloodLocation> GetFloodLocationsByPoint(double easting, double northing, double buffer);
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodLocationRepository
{
    IQueryable<FloodLocation> GetFloodLocations();
    IQueryable<FloodLocation> GetFloodLocationsWithReports();
    IQueryable<FloodLocation> GetFloodLocationsByPoint(double easting, double northing, double buffer);
}

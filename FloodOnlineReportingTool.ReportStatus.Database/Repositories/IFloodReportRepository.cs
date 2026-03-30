using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodReportRepository
{
    IQueryable<FloodReport> GetFloodReports();
    IQueryable<FloodReport?> GetFloodReportById(Guid id);
    IQueryable<FloodReport> GetFloodReportsWithinMetres(double easting, double northing, double metres);
    IQueryable<FloodReport> GetFloodReportsByUprn(long uprn);
    IQueryable<FloodReport> GetFloodReportsByUsrn(long usrn);
    IQueryable<FloodReport> SearchFloodReports(DateTimeOffset? resultsFrom, DateTimeOffset? resultsTo, ICollection<Guid> floodSources, long? uprn, long? usrn, MultiPolygon? geom);
}

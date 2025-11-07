using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories.Search;

public interface ISearchFloodReportRepository
{
    IQueryable<FloodReport> GetFloodReports();
    IQueryable<FloodReport> GetFloodReportsWithinMetres(double easting, double northing, double metres);
    IQueryable<FloodReport> GetFloodReportsByUprn(long uprn);
    IQueryable<FloodReport> GetFloodReportsByUsrn(long usrn);
}

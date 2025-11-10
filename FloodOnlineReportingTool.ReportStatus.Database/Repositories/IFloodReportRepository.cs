using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface IFloodReportRepository
{
    IQueryable<FloodReport> GetFloodReports();
    IQueryable<FloodReport?> GetFloodReportById(Guid id);
    IQueryable<FloodReport> GetFloodReportsWithinMetres(double easting, double northing, double metres);
    IQueryable<FloodReport> GetFloodReportsByUprn(long uprn);
    IQueryable<FloodReport> GetFloodReportsByUsrn(long usrn);
}

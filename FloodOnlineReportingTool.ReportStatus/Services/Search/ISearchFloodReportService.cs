using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Models.Search;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodReportService
{
    const int DefaultPageSize = 10;

    Task<PagedResult<FloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<FloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<FloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
}

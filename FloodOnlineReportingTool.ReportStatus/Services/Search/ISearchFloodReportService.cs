using FloodOnlineReportingTool.Contracts.Shared.Search;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodReportService
{
    const int DefaultPageSize = 10;
    const double MaxMetres = 100;

    Task<PagedResult<SearchResultFloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodReport>> SearchFloodReports(DateTimeOffset? resultsFrom, DateTimeOffset? resultsTo, ICollection<Guid> floodSources, long? uprn, long? usrn, MultiPolygon? geom, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);

    // TODO: Search by created date
    // TODO: Search by impact start date
    // TODO: Search by ID
    // TODO: Search by reference
}

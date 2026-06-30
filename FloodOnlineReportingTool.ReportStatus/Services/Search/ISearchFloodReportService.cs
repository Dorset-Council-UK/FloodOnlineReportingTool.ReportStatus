using FloodOnlineReportingTool.Contracts.Shared.Search;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodReportService
{
    const int DefaultPageSize = 10;
    const double MaxMetres = 100;

    Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<SearchFloodReportPagedResult<SearchResultFloodReport>> SearchFloodReports(DateTimeOffset? resultsFrom, DateTimeOffset? resultsTo, ICollection<Guid> floodSources, long? uprn, long? usrn, string? postcode, MultiPolygon? geom, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);

    // TODO: Search by created date
    // TODO: Search by impact start date
    // TODO: Search by ID
    // TODO: Search by reference
}

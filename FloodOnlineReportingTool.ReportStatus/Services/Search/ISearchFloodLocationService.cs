using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodLocationService
{
    const int DefaultPageSize = 10;

    Task<PagedResult<SearchResultFloodLocation>> GetFloodLocations(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodLocation>> GetFloodLocationsWithReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodLocation>> GetFloodLocationsByPoint(double easting, double northing, double buffer, int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
}

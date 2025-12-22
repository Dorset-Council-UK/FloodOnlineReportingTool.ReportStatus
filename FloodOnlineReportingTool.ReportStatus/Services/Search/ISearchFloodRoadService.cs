using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodRoadService
{
    const int DefaultPageSize = 10;

    Task<PagedResult<SearchResultFloodRoad>> GetFloodRoads(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodRoad>> GetFloodRoadsWithReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
}

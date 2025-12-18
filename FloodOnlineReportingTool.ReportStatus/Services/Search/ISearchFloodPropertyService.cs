using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal interface ISearchFloodPropertyService
{
    const int DefaultPageSize = 10;

    Task<PagedResult<SearchResultFloodProperty>> GetFloodProperties(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
    Task<PagedResult<SearchResultFloodProperty>> GetFloodPropertiesWithReports(int pageNumber = 1, int pageSize = DefaultPageSize, CancellationToken ct = default);
}

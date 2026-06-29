using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Models;

public record SearchFloodReportPagedResult(
    IReadOnlyCollection<SearchResultFloodReport> Results,
    int TotalCount,
    int TotalFloodSourceCount,
    int PageSize,
    int CurrentPage,
    int TotalPages);
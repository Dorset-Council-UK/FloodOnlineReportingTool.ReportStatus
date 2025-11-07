namespace FloodOnlineReportingTool.ReportStatus.Models.Search;

internal record PagedResult<T>(
    IReadOnlyCollection<T> Results,
    int TotalCount,
    int PageSize,
    int CurrentPage,
    int TotalPages
);
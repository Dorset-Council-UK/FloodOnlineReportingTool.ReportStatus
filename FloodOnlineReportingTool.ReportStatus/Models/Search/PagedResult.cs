namespace FloodOnlineReportingTool.ReportStatus.Models.Search;

public record PagedResult<T>(
    IReadOnlyCollection<T> Results,
    int TotalCount,
    int PageSize,
    int CurrentPage,
    int TotalPages
);
using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace Tests.Data;

public static class SearchResultFloodReportCauseData
{
    public static readonly ICollection<SearchResultFloodReportCause> Empty = [];
    public static readonly ICollection<SearchResultFloodReportCause> ThreeItems = [
        new(Guid.NewGuid(), "Cause 1"),
        new(Guid.NewGuid(), "Cause 2"),
        new(Guid.NewGuid(), "Cause 3"),
    ];
}

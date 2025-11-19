using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace Tests.Data;

public static class SearchResultFloodReportSourceData
{
    public static readonly ICollection<SearchResultFloodReportSource> Empty = [];
    public static readonly ICollection<SearchResultFloodReportSource> ThreeItems = [
        new(Guid.NewGuid(), "Source 1"),
        new(Guid.NewGuid(), "Source 2"),
        new(Guid.NewGuid(), "Source 3"),
    ];
}

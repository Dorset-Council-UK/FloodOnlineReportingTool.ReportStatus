using FloodOnlineReportingTool.ReportStatus.Models.Search;

namespace Tests.Data;

public static class SearchResultFloodReportData
{
    public static readonly DateTimeOffset FixedDate = new(2025, 11, 13, 12, 19, 0, TimeSpan.Zero);

    public static readonly SearchResultFloodReport Item1 = new (Guid.NewGuid(), FixedDate.AddDays(-1), null, 1, 1, null, [..SearchResultFloodReportSourceData.ThreeItems]);
    public static readonly SearchResultFloodReport Item2 = new(Guid.NewGuid(), FixedDate.AddDays(-2), null, 2, 2, null, [.. SearchResultFloodReportSourceData.ThreeItems]);
    public static readonly SearchResultFloodReport Item3 = new(Guid.NewGuid(), FixedDate.AddDays(-3), null, 3, 3, null, [.. SearchResultFloodReportSourceData.ThreeItems]);

    public static readonly ICollection<SearchResultFloodReport> Empty = [];
    public static readonly ICollection<SearchResultFloodReport> ThreeItems = [Item1, Item2, Item3];
}

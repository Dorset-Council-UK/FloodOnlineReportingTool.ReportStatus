using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class SearchResultFloodReportData
{
    public static readonly DateTimeOffset FixedDate = new(2025, 11, 13, 12, 19, 0, TimeSpan.Zero);

    public static readonly SearchResultFloodReport Item1 = new(Guid.NewGuid(), FixedDate.AddDays(-1), null, new Point(368991.12, 90881.94) { SRID = 27700 }, null, [..SearchResultFloodReportSourceData.ThreeItems]);
    public static readonly SearchResultFloodReport Item2 = new(Guid.NewGuid(), FixedDate.AddDays(-2), null, new Point(369500.00, 91000.00) { SRID = 27700 }, null, [.. SearchResultFloodReportSourceData.ThreeItems]);
    public static readonly SearchResultFloodReport Item3 = new(Guid.NewGuid(), FixedDate.AddDays(-3), null, new Point(370000.00, 91500.00) { SRID = 27700 }, null, [.. SearchResultFloodReportSourceData.ThreeItems]);

    public static readonly ICollection<SearchResultFloodReport> Empty = [];
    public static readonly ICollection<SearchResultFloodReport> ThreeItems = [Item1, Item2, Item3];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReport>, PagedResult<SearchResultFloodReport>>> Four
    {
        get
        {
            var floodReport1 = FloodReportData.Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000");
            var floodReport2 = FloodReportData.Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000");
            var floodReport3 = FloodReportData.Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000");

            ICollection<FloodReport> collection0 = [];
            ICollection<FloodReport> collection1 = [floodReport1];
            ICollection<FloodReport> collection2 = [floodReport1, floodReport2];
            ICollection<FloodReport> collection3 = [floodReport1, floodReport2, floodReport3];


            return [
                new(collection0, new(
                    Results: collection0.ToSearchResults(),
                    TotalCount: 0,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 0
                )),
                new(collection1, new(
                    Results: collection1.ToSearchResults(),
                    TotalCount: 1,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 1
                )),
                new(collection2, new(
                    Results: collection2.ToSearchResults(),
                    TotalCount: 2,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 1
                )),
                new(collection3, new(
                    Results: collection3.ToSearchResults(),
                    TotalCount: 3,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 1
                )),
            ];

        }
    }
}

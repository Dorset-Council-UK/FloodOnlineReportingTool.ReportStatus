using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class SearchResultFloodRoadData
{
    public static PagedResult<SearchResultFloodRoad> NotFound => new(
        Results: [],
        TotalCount: 0,
        PageSize: 10,
        CurrentPage: 1,
        TotalPages: 0
    );

    /// <summary>
    /// Unit test data for flood road searches.
    /// </summary>
    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>, PagedResult<SearchResultFloodRoad>>> Four
    {
        get
        {
            var road1 = FloodRoadData.Create(floodRoadId: new Guid("33333333-1111-0000-0000-000000000000"));
            var road2 = FloodRoadData.Create(floodRoadId: new Guid("33333333-2222-0000-0000-000000000000"));
            var road3 = FloodRoadData.Create(floodRoadId: new Guid("33333333-3333-0000-0000-000000000000"));

            ICollection<FloodRoad> collection1 = [road1];
            ICollection<FloodRoad> collection2 = [road1, road2];
            ICollection<FloodRoad> collection3 = [road1, road2, road3];

            return [
                new([], NotFound),
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

    /// <summary>
    /// Unit test data for flood road searches with flood reports.
    /// </summary>
    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>, PagedResult<SearchResultFloodRoad>>> Four_WithReports
    {
        get
        {
            var road1 = FloodRoadData.Create(floodRoadId: new Guid("33333333-1111-0000-0000-000000000000"), floodReportCount: 1);
            var road2 = FloodRoadData.Create(floodRoadId: new Guid("33333333-2222-0000-0000-000000000000"), floodReportCount: 2);
            var road3 = FloodRoadData.Create(floodRoadId: new Guid("33333333-3333-0000-0000-000000000000"), floodReportCount: 3);

            ICollection<FloodRoad> collection1 = [road1];
            ICollection<FloodRoad> collection2 = [road1, road2];
            ICollection<FloodRoad> collection3 = [road1, road2, road3];

            return [
                new([], NotFound),
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

using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class SearchResultFloodLocationData
{
    public static readonly DateTimeOffset FixedDate = new(2025, 12, 11, 16, 22, 0, TimeSpan.Zero);

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>, PagedResult<SearchResultFloodLocation>>> Four
    {
        get
        {
            var floodLocation1 = FloodLocationData.Create(floodLocationId: new Guid("33333333-1111-0000-0000-000000000000"));
            var floodLocation2 = FloodLocationData.Create(floodLocationId: new Guid("33333333-2222-0000-0000-000000000000"));
            var floodLocation3 = FloodLocationData.Create(floodLocationId: new Guid("33333333-3333-0000-0000-000000000000"));

            ICollection<FloodLocation> collection0 = [];
            ICollection<FloodLocation> collection1 = [floodLocation1];
            ICollection<FloodLocation> collection2 = [floodLocation1, floodLocation2];
            ICollection<FloodLocation> collection3 = [floodLocation1, floodLocation2, floodLocation3];


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

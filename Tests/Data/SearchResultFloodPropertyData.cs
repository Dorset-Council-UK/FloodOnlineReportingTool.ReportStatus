using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class SearchResultFloodPropertyData
{
    /// <summary>
    /// Unit test data for flood property searches.
    /// </summary>
    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>, PagedResult<SearchResultFloodProperty>>> Four
    {
        get
        {
            var property1 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-1111-0000-0000-000000000000"));
            var property2 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-2222-0000-0000-000000000000"));
            var property3 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-3333-0000-0000-000000000000"));

            ICollection<FloodProperty> collection0 = [];
            ICollection<FloodProperty> collection1 = [property1];
            ICollection<FloodProperty> collection2 = [property1, property2];
            ICollection<FloodProperty> collection3 = [property1, property2, property3];

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

    /// <summary>
    /// Unit test data for flood property searches with flood reports.
    /// </summary>
    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>, PagedResult<SearchResultFloodProperty>>> Four_WithReports
    {
        get
        {
            var property1 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-1111-0000-0000-000000000000"), floodReportCount: 1);
            var property2 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-2222-0000-0000-000000000000"), floodReportCount: 2);
            var property3 = FloodPropertyData.Create(floodPropertyId: new Guid("33333333-3333-0000-0000-000000000000"), floodReportCount: 3);

            ICollection<FloodProperty> collection0 = [];
            ICollection<FloodProperty> collection1 = [property1];
            ICollection<FloodProperty> collection2 = [property1, property2];
            ICollection<FloodProperty> collection3 = [property1, property2, property3];

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

using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

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

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>, PagedResult<SearchResultFloodLocation>>> Four_WithReports
    {
        get
        {
            var floodLocation1 = FloodLocationData.Create(floodLocationId: new Guid("33333333-1111-0000-0000-000000000000"), floodReportCount: 1);
            var floodLocation2 = FloodLocationData.Create(floodLocationId: new Guid("33333333-2222-0000-0000-000000000000"), floodReportCount: 2);
            var floodLocation3 = FloodLocationData.Create(floodLocationId: new Guid("33333333-3333-0000-0000-000000000000"), floodReportCount: 3);

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

    /// <summary>
    /// <see cref="double">Easting</see>, <see cref="double">Northing</see>, <see cref="double">Buffer</see>,
    /// <see cref="ICollection&lt;&gt;">Expected Flood Locations</see>,
    /// <see cref="PagedResult&lt;&gt;">Expected Paged Result</see>
    /// </summary>
    public static IEnumerable<TheoryDataRow<double, double, double, ICollection<FloodLocation>, PagedResult<SearchResultFloodLocation>>> Four_WithPoint
    {
        get
        {
            var coords = new[]
            {
                (Easting: 368991.12, Northing: 90881.94, Buffer: 10.0),
                (Easting: 369500.00, Northing: 91000.00, Buffer: 30.0),
                (Easting: 370000.00, Northing: 91500.00, Buffer: 40.0),
                (Easting: 370500.00, Northing: 92000.00, Buffer: 50.0),
            };

            var ordered = Enumerable.Range(1, 3)
                .Select(i => FloodLocationData.Create(
                    geom: new Point(coords[i].Easting, coords[i].Northing) { SRID = 27700 },
                    floodReportCount: i))
                .OrderBy(fl => fl.Id)
                .ToArray();

            ICollection<FloodLocation> collection0 = [];
            ICollection<FloodLocation> collection1 = [ordered[0]];
            ICollection<FloodLocation> collection2 = [ordered[0], ordered[1]];
            ICollection<FloodLocation> collection3 = [ordered[0], ordered[1], ordered[2]];

            return [
                new(coords[0].Easting, coords[0].Northing, coords[0].Buffer, collection0, new(
                    Results: collection0.ToSearchResults(),
                    TotalCount: 0,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 0
                )),
                new(coords[1].Easting, coords[1].Northing, coords[1].Buffer, collection1, new(
                    Results: collection1.ToSearchResults(),
                    TotalCount: 1,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 1
                )),
                new(coords[2].Easting, coords[2].Northing, coords[2].Buffer, collection2, new(
                    Results: collection2.ToSearchResults(),
                    TotalCount: 2,
                    PageSize: 10,
                    CurrentPage: 1,
                    TotalPages: 1
                )),
                new(coords[3].Easting, coords[3].Northing, coords[3].Buffer, collection3, new(
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

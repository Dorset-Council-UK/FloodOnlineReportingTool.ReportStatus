using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class SearchResultFloodReportData
{
    /// <summary>
    /// Unit test data for flood report searches.
    /// </summary>
    //public static IEnumerable<TheoryDataRow<ICollection<FloodReportSource>, PagedResult<SearchResultFloodReport>>> Four
    //{
    //    get
    //    {
    //        var floodReport1 = FloodReportData.Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000");
    //        var floodReport2 = FloodReportData.Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000");
    //        var floodReport3 = FloodReportData.Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000");

    //        ICollection<FloodReportSource> collection0 = [];
    //        ICollection<FloodReportSource> collection1 = [floodReport1];
    //        ICollection<FloodReportSource> collection2 = [floodReport1, floodReport2];
    //        ICollection<FloodReportSource> collection3 = [floodReport1, floodReport2, floodReport3];

    //        return [
    //            new(collection0, new(
    //                Results: collection0.ToSearchResults(),
    //                TotalCount: 0,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 0
    //            )),
    //            new(collection1, new(
    //                Results: collection1.ToSearchResults(),
    //                TotalCount: 1,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(collection2, new(
    //                Results: collection2.ToSearchResults(),
    //                TotalCount: 2,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(collection3, new(
    //                Results: collection3.ToSearchResults(),
    //                TotalCount: 3,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //        ];
    //    }
    //}

    ///// <summary>
    ///// Unit test data for UPRN search.
    ///// <see cref="long">UPRN</see>,
    ///// <see cref="ICollection&lt;&gt;">Expected Flood Reports</see>,
    ///// <see cref="PagedResult&lt;&gt;">Expected Paged Result</see>
    ///// </summary>
    //public static IEnumerable<TheoryDataRow<long, ICollection<FloodReportSource>, PagedResult<SearchResultFloodReport>>> Four_WithUprn
    //{
    //    get
    //    {
    //        long[] uprn = [100040634390, 10013290693, 10023242411, 10002062622];

    //        var floodReport1 = FloodReportData.Create(floodReportId: "11111111-1111-0000-0000-000000000000", uprn: uprn[1]);
    //        var floodReport2 = FloodReportData.Create(floodReportId: "11111111-2222-0000-0000-000000000000", uprn: uprn[2]);
    //        var floodReport3 = FloodReportData.Create(floodReportId: "11111111-3333-0000-0000-000000000000", uprn: uprn[3]);

    //        ICollection<FloodReportSource> collection0 = [];
    //        ICollection<FloodReportSource> collection1 = [floodReport1];
    //        ICollection<FloodReportSource> collection2 = [floodReport1, floodReport2];
    //        ICollection<FloodReportSource> collection3 = [floodReport1, floodReport2, floodReport3];

    //        return [
    //            new(uprn[0], collection0, new(
    //                Results: collection0.ToSearchResults(),
    //                TotalCount: 0,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 0
    //            )),
    //            new(uprn[1], collection1, new(
    //                Results: collection1.ToSearchResults(),
    //                TotalCount: 1,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(uprn[2], collection2, new(
    //                Results: collection2.ToSearchResults(),
    //                TotalCount: 2,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(uprn[3], collection3, new(
    //                Results: collection3.ToSearchResults(),
    //                TotalCount: 3,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //        ];
    //    }
    //}

    ///// <summary>
    ///// Unit test data for USRN search.
    ///// <see cref="long">USRN</see>,
    ///// <see cref="ICollection&lt;&gt;">Expected Flood Reports</see>,
    ///// <see cref="PagedResult&lt;&gt;">Expected Paged Result</see>
    ///// </summary>
    //public static IEnumerable<TheoryDataRow<long, ICollection<FloodReportSource>, PagedResult<SearchResultFloodReport>>> Four_WithUsrn
    //{
    //    get
    //    {
    //        long[] usrn = [20023242411, 43201527, 43201524, 81100003];

    //        var floodReport1 = FloodReportData.Create(floodReportId: "11111111-1111-0000-0000-000000000000", usrn: usrn[1]);
    //        var floodReport2 = FloodReportData.Create(floodReportId: "11111111-2222-0000-0000-000000000000", usrn: usrn[2]);
    //        var floodReport3 = FloodReportData.Create(floodReportId: "11111111-3333-0000-0000-000000000000", usrn: usrn[3]);

    //        ICollection<FloodReportSource> collection0 = [];
    //        ICollection<FloodReportSource> collection1 = [floodReport1];
    //        ICollection<FloodReportSource> collection2 = [floodReport1, floodReport2];
    //        ICollection<FloodReportSource> collection3 = [floodReport1, floodReport2, floodReport3];

    //        return [
    //            new(usrn[0], collection0, new(
    //                Results: collection0.ToSearchResults(),
    //                TotalCount: 0,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 0
    //            )),
    //            new(usrn[1], collection1, new(
    //                Results: collection1.ToSearchResults(),
    //                TotalCount: 1,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(usrn[2], collection2, new(
    //                Results: collection2.ToSearchResults(),
    //                TotalCount: 2,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(usrn[3], collection3, new(
    //                Results: collection3.ToSearchResults(),
    //                TotalCount: 3,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //        ];
    //    }
    //}

    ///// <summary>
    ///// Unit test data for flood reports within 50 metres search.
    ///// <see cref="double">Easting</see>, <see cref="double">Northing</see>, <see cref="double">Metres</see>,
    ///// <see cref="ICollection&lt;&gt;">Expected Flood Reports</see>,
    ///// <see cref="PagedResult&lt;&gt;">Expected Paged Result</see>
    ///// </summary>
    //public static IEnumerable<TheoryDataRow<double, double, double, ICollection<FloodReportSource>, PagedResult<SearchResultFloodReport>>> Four_Within50Metres
    //{
    //    get
    //    {
    //        var coords = new[]
    //        {
    //            (Easting: 368991.12, Northing: 90881.94, Metres: 10.0),
    //            (Easting: 369500.00, Northing: 91000.00, Metres: 30.0),
    //            (Easting: 370000.00, Northing: 91500.00, Metres: 40.0),
    //            (Easting: 370500.00, Northing: 92000.00, Metres: 50.0),
    //        };

    //        var floodReport1 = FloodReportData.Create(floodReportId: "11111111-1111-0000-0000-000000000000", easting: coords[1].Easting, northing: coords[1].Northing);
    //        var floodReport2 = FloodReportData.Create(floodReportId: "11111111-2222-0000-0000-000000000000", easting: coords[2].Easting, northing: coords[2].Northing);
    //        var floodReport3 = FloodReportData.Create(floodReportId: "11111111-3333-0000-0000-000000000000", easting: coords[3].Easting, northing: coords[3].Northing);

    //        ICollection<FloodReportSource> collection0 = [];
    //        ICollection<FloodReportSource> collection1 = [floodReport1];
    //        ICollection<FloodReportSource> collection2 = [floodReport1, floodReport2];
    //        ICollection<FloodReportSource> collection3 = [floodReport1, floodReport2, floodReport3];

    //        return [
    //            new(coords[0].Easting, coords[0].Northing, coords[0].Metres, collection0, new(
    //                Results: collection0.ToSearchResults(),
    //                TotalCount: 0,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 0
    //            )),
    //            new(coords[1].Easting, coords[1].Northing, coords[1].Metres, collection1, new(
    //                Results: collection1.ToSearchResults(),
    //                TotalCount: 1,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(coords[2].Easting, coords[2].Northing, coords[2].Metres, collection2, new(
    //                Results: collection2.ToSearchResults(),
    //                TotalCount: 2,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //            new(coords[3].Easting, coords[3].Northing, coords[3].Metres, collection3, new(
    //                Results: collection3.ToSearchResults(),
    //                TotalCount: 3,
    //                PageSize: 10,
    //                CurrentPage: 1,
    //                TotalPages: 1
    //            )),
    //        ];
    //    }
    //}
}

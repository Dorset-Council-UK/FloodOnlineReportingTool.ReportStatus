using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodRoadData
{
    //internal static readonly long Usrn = 20023242411;
    internal static readonly double Easting = 368991.12;
    internal static readonly double Northing = 90881.94;
    public static readonly DateTimeOffset FixedDate = new(2025, 12, 02, 16, 05, 0, TimeSpan.Zero);

    /// <summary>
    /// Creates a FloodRoad with an optional ID and a specified number of related FloodRoadFloodReports.
    /// </summary>
    public static FloodRoad Create(Guid? floodRoadId = null, long usrn = 20023242411, int floodReportCount = 0)
    {
        var id = floodRoadId ?? Guid.NewGuid();
        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood road",
            Usrn = usrn,
            Location = new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = [.. Enumerable.Range(0, floodReportCount).Select(_ => FloodRoadFloodReportData.Create(id))],
        };
    }

    public static IEnumerable<TheoryDataRow<FloodRoad>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodRoad>> Four_WithReports => [
        new(Create()),
        new(Create(floodReportCount: 1)),
        new(Create(floodReportCount: 2)),
        new(Create(floodReportCount: 3)),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>>> Collection_Four_WithReports => [
        new([]),
        new([Create(floodReportCount: 1)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2), Create(floodReportCount: 3)]),
    ];

    public static IEnumerable<TheoryDataRow<long, ICollection<FloodRoad>>> Collection_Four_WithUsrn => [
        new(20023242411, []),
        new(43201527, [Create(usrn: 43201527)]),
        new(43201524, [Create(usrn: 43201524), Create(usrn: 43201524)]),
        new(81100003, [Create(usrn: 81100003), Create(usrn: 81100003), Create(usrn: 81100003)]),
    ];
}

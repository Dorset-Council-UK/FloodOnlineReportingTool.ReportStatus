using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodLocationData
{
    internal static double Easting = 368991.12;
    internal static double Northing = 90881.94;
    public static readonly DateTimeOffset FixedDate = new(2025, 11, 25, 14, 16, 0, TimeSpan.Zero);

    /// <summary>
    /// Creates a FloodLocation with an optional ID and a specified number of related FloodLocationFloodReports.
    /// </summary>
    public static FloodLocation Create(Guid? floodLocationId = null, int floodReportCount = 0)
    {
        var id = floodLocationId ?? Guid.NewGuid();
        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood location",
            Location = new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = [.. Enumerable.Range(0, floodReportCount).Select(_ => FloodLocationFloodReportData.Create(id))],
        };
    }

    public static IEnumerable<TheoryDataRow<FloodLocation>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodLocation>> Four_WithReports => [
        new(Create()),
        new(Create(floodReportCount: 1)),
        new(Create(floodReportCount: 2)),
        new(Create(floodReportCount: 3)),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>>> Collection_Four_WithReports => [
    new([]),
        new([Create(floodReportCount: 1)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2), Create(floodReportCount: 3)]),
    ];
}

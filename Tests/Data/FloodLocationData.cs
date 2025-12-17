using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodLocationData
{
    internal static readonly double Easting = 368991.12;
    internal static readonly double Northing = 90881.94;
    public static readonly DateTimeOffset FixedDate = new(2025, 11, 25, 14, 16, 0, TimeSpan.Zero);

    /// <summary>
    /// Creates a FloodLocation with an optional ID, Geom, and a specified number of related FloodLocationFloodReports.
    /// </summary>
    public static FloodLocation Create(Guid? floodLocationId = null, Point? geom = null, int floodReportCount = 0)
    {
        var id = floodLocationId ?? Guid.NewGuid();
        var floodReports = Enumerable.Range(0, floodReportCount)
            .Select(_ => FloodLocationFloodReportData.Create(id))
            .OrderBy(fr => fr.FloodReportId)
            .ToList();

        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood location",
            Geom = geom ?? new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = floodReports,
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

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>>> Collection_Four
    {
        get
        {
            var ordered = Enumerable.Range(0, 3)
                .Select(_ => Create())
                .OrderBy(fl => fl.Id)
                .ToArray();

            return [
                new([]),
                new([ordered[0]]),
                new([ordered[0], ordered[1]]),
                new([ordered[0], ordered[1], ordered[2]]),
            ];
        }
    }

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocation>>> Collection_Four_WithReports
    {
        get
        {
            var ordered = Enumerable.Range(0, 3)
                .Select(i => Create(floodReportCount: i + 1))
                .OrderBy(fl => fl.Id)
                .ToArray();

            return [
                new([]),
                new([ordered[0]]),
                new([ordered[0], ordered[1]]),
                new([ordered[0], ordered[1], ordered[2]]),
            ];
        }
    }

    /// <summary>
    /// <see cref="double">Easting</see>, <see cref="double">Northing</see>, <see cref="double">Buffer</see>, <see cref="ICollection&lt;&gt;">Expected Flood Locations</see> with matching <see cref="Point"/>
    /// </summary>
    public static IEnumerable<TheoryDataRow<double, double, double, ICollection<FloodLocation>>> Collection_Four_WithPoint
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
                .Select(i => Create(
                    geom: new Point(coords[i].Easting, coords[i].Northing) { SRID = 27700 },
                    floodReportCount: i))
                .OrderBy(fl => fl.Id)
                .ToArray();

            return [
                new(coords[0].Easting, coords[0].Northing, coords[0].Buffer, []),
                new(coords[1].Easting, coords[1].Northing, coords[1].Buffer, [ordered[0]]),
                new(coords[2].Easting, coords[2].Northing, coords[2].Buffer, [ordered[0], ordered[1]]),
                new(coords[3].Easting, coords[3].Northing, coords[3].Buffer, [ordered[0], ordered[1], ordered[2]]),
            ];
        }
    }
}

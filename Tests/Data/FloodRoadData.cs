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
        var floodReports = Enumerable.Range(0, floodReportCount)
            .Select(_ => FloodRoadFloodReportData.Create(id))
            .OrderBy(fr => fr.FloodReportId)
            .ToList();

        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood road",
            Usrn = usrn,
            Geom = new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = floodReports,
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

    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>>> Collection_Four
    {
        get
        {
            var ordered = Enumerable.Range(0, 3)
                .Select(_ => Create())
                .OrderBy(fr => fr.Id)
                .ToArray();

            return [
                new([]),
                new([ordered[0]]),
                new([ordered[0], ordered[1]]),
                new([ordered[0], ordered[1], ordered[2]]),
            ];
        }
    }

    public static IEnumerable<TheoryDataRow<ICollection<FloodRoad>>> Collection_Four_WithReports
    {
        get
        {
            var ordered = Enumerable.Range(0, 3)
                .Select(i => Create(floodReportCount: i + 1))
                .OrderBy(fr => fr.Id)
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
    /// <see cref="long">USRN</see>, 
    /// <see cref="ICollection&lt;&gt;">All flood roads</see>, 
    /// <see cref="ICollection&lt;&gt;">Expected flood roads</see> with matching <see cref="Point"/>
    /// </summary>
    public static IEnumerable<TheoryDataRow<long, ICollection<FloodRoad>, ICollection<FloodRoad>>> Collection_Four_WithUsrn
    {
        get
        {
            var road1 = Create(usrn: 43201527);
            var road2 = Create(usrn: 43201524);
            var road3 = Create(usrn: 81100003);

            var allRoads = new[] { road1, road2, road3 }
                .OrderBy(fr => fr.Id)
                .ToArray();

            return [
                new(20023242411, allRoads, []),
                new(43201527, allRoads, [road1]),
                new(43201524, allRoads, [road2]),
                new(81100003, allRoads, [road3]),
            ];
        }
    }
}

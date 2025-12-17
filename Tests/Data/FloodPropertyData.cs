using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodPropertyData
{
    internal static readonly double Easting = 368991.12;
    internal static readonly double Northing = 90881.94;
    public static readonly DateTimeOffset FixedDate = new(2025, 12, 03, 10, 35, 0, TimeSpan.Zero);

    /// <summary>
    /// Creates a FloodProperty with an optional ID and a specified number of related FloodPropertyFloodReports.
    /// </summary>
    public static FloodProperty Create(Guid? floodPropertyId = null, long uprn = 10023242411, int floodReportCount = 0)
    {
        var id = floodPropertyId ?? Guid.NewGuid();
        var floodReports = Enumerable.Range(0, floodReportCount)
            .Select(_ => FloodPropertyFloodReportData.Create(id))
            .OrderBy(fr => fr.FloodReportId)
            .ToList();

        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood property",
            Uprn = uprn,
            Geom = new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = floodReports,
        };
    }

    public static IEnumerable<TheoryDataRow<FloodProperty>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodProperty>> Four_WithReports => [
        new(Create()),
        new(Create(floodReportCount: 1)),
        new(Create(floodReportCount: 2)),
        new(Create(floodReportCount: 3)),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>>> Collection_Four
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

    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>>> Collection_Four_WithReports
    {
        get
        {
            var ordered = Enumerable.Range(0, 3)
                .Select(_ => Create())
                .OrderBy(fp => fp.Id)
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
    /// <see cref="long">UPRN</see>, <see cref="ICollection&lt;&gt;">Expected Flood Properties</see> with matching <see cref="Point"/>
    /// </summary>
    public static IEnumerable<TheoryDataRow<long, ICollection<FloodProperty>>> Collection_Four_WithUprn
    {
        get
        {
            long[] uprn = [100040634390, 10013290693, 10023242411, 10002062622];

            var ordered = Enumerable.Range(1, 3)
                .Select(i => Create(uprn: uprn[i]))
                .OrderBy(fp => fp.Id)
                .ToArray();

            return [
                new(uprn[0], []),
                new(uprn[1], [ordered[0]]),
                new(uprn[2], [ordered[0], ordered[1]]),
                new(uprn[3], [ordered[0], ordered[1], ordered[2]]),
            ];
        }
    }
}

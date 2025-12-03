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
        return new()
        {
            Id = id,
            CreatedUtc = FixedDate,
            Title = "Flood property",
            Uprn = uprn,
            Location = new Point(Easting, Northing) { SRID = 27700 },
            FloodReports = [.. Enumerable.Range(0, floodReportCount).Select(_ => FloodPropertyFloodReportData.Create(id))],
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

    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodProperty>>> Collection_Four_WithReports => [
    new([]),
        new([Create(floodReportCount: 1)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2)]),
        new([Create(floodReportCount: 1), Create(floodReportCount: 2), Create(floodReportCount: 3)]),
    ];

    public static IEnumerable<TheoryDataRow<long, ICollection<FloodProperty>>> Collection_Four_WithUprn => [
    new(100040634390, []),
        new(10013290693, [Create(uprn: 10013290693)]),
        new(10023242411, [Create(uprn: 10023242411), Create(uprn: 10023242411)]),
        new(10002062622, [Create(uprn: 10002062622), Create(uprn: 10002062622), Create(uprn: 10002062622)]),
    ];
}

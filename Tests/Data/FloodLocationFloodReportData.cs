using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodLocationFloodReportData
{
    public static readonly DateTimeOffset FixedDate = new(2025, 11, 25, 14, 16, 0, TimeSpan.Zero);

    public static FloodLocationFloodReport Create(Guid? floodLocationId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodLocationId = floodLocationId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = FixedDate,
        };
    }

    public static IEnumerable<TheoryDataRow<FloodLocationFloodReport>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocationFloodReport>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];
}

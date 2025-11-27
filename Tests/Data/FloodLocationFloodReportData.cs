using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodLocationFloodReportData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 11, 25, 14, 16, 0, TimeSpan.Zero);
    public static readonly DateTimeOffset TestConfirmedDate = new(2025, 11, 26, 14, 46, 0, TimeSpan.Zero);
    public static readonly string TestConfirmedUserId = Guid.NewGuid().ToString();
    public static readonly string TestConfirmedUserName = "Test User";

    public static FloodLocationFloodReport Create(Guid? floodLocationId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodLocationId = floodLocationId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
        };
    }

    public static FloodLocationFloodReport CreateConfirmed(Guid? floodLocationId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodLocationId = floodLocationId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
            ConfirmedUtc = TestConfirmedDate,
            ConfirmedByUserId = TestConfirmedUserId,
            ConfirmedUserName = TestConfirmedUserName,
        };
    }

    public static IEnumerable<TheoryDataRow<FloodLocationFloodReport>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodLocationFloodReport>> FourConfirmed => [
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodLocationFloodReport>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<Guid, ICollection<FloodLocationFloodReport>>> ForFloodLocation
    {
        get
        {
            var floodLocationId1 = Guid.NewGuid();
            var floodLocationId2 = Guid.NewGuid();
            var floodLocationId3 = Guid.NewGuid();
            var floodLocationId4 = Guid.NewGuid();

            return [
                new(floodLocationId1, []),
                new(floodLocationId2, [Create(floodLocationId2)]),
                new(floodLocationId3, [Create(floodLocationId3), Create(floodLocationId3)]),
                new(floodLocationId4, [Create(floodLocationId4), Create(floodLocationId4), Create(floodLocationId4)]),
            ];
        }
    }
}

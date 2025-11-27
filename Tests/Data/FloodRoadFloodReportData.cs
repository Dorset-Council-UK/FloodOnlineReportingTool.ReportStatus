using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodRoadFloodReportData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 11, 26, 14, 24, 0, TimeSpan.Zero);
    public static readonly DateTimeOffset TestConfirmedDate = new(2025, 11, 27, 14, 24, 0, TimeSpan.Zero);
    public static readonly string TestConfirmedUserId = Guid.NewGuid().ToString();
    public static readonly string TestConfirmedUserName = "Test User";

    public static FloodRoadFloodReport Create(Guid? floodRoadId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodRoadId = floodRoadId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
        };
    }

    public static FloodRoadFloodReport CreateConfirmed(Guid? floodRoadId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodRoadId = floodRoadId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
            ConfirmedUtc = TestConfirmedDate,
            ConfirmedByUserId = TestConfirmedUserId,
            ConfirmedUserName = TestConfirmedUserName,
        };
    }

    public static IEnumerable<TheoryDataRow<FloodRoadFloodReport>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodRoadFloodReport>> FourConfirmed => [
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodRoadFloodReport>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<Guid, ICollection<FloodRoadFloodReport>>> ForFloodRoad
    {
        get
        {
            var floodRoadId1 = Guid.NewGuid();
            var floodRoadId2 = Guid.NewGuid();
            var floodRoadId3 = Guid.NewGuid();
            var floodRoadId4 = Guid.NewGuid();

            return [
                new(floodRoadId1, []),
                new(floodRoadId2, [Create(floodRoadId2)]),
                new(floodRoadId3, [Create(floodRoadId3), Create(floodRoadId3)]),
                new(floodRoadId4, [Create(floodRoadId4), Create(floodRoadId4), Create(floodRoadId4)]),
            ];
        }
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodPropertyFloodReportData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 11, 26, 14, 49, 0, TimeSpan.Zero);
    public static readonly DateTimeOffset TestConfirmedDate = new(2025, 11, 27, 14, 48, 0, TimeSpan.Zero);
    public static readonly string TestConfirmedUserId = Guid.NewGuid().ToString(); // Any test user ID
    public static readonly string TestConfirmedUserName = "Test User";

    public static FloodPropertyFloodReport Create(Guid? floodPropertyId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodPropertyId = floodPropertyId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
        };
    }

    public static FloodPropertyFloodReport CreateConfirmed(Guid? floodPropertyId = null, Guid? floodReportId = null)
    {
        return new()
        {
            FloodPropertyId = floodPropertyId ?? Guid.NewGuid(),
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
            ConfirmedUtc = TestConfirmedDate,
            ConfirmedByUserId = TestConfirmedUserId,
            ConfirmedUserName = TestConfirmedUserName,
        };
    }

    public static IEnumerable<TheoryDataRow<FloodPropertyFloodReport>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<FloodPropertyFloodReport>> FourConfirmed => [
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
        new(CreateConfirmed()),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodPropertyFloodReport>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<Guid, ICollection<FloodPropertyFloodReport>>> ForFloodProperty
    {
        get
        {
            var floodPropertyId1 = Guid.NewGuid();
            var floodPropertyId2 = Guid.NewGuid();
            var floodPropertyId3 = Guid.NewGuid();
            var floodPropertyId4 = Guid.NewGuid();

            return [
                new(floodPropertyId1, []),
                new(floodPropertyId2, [Create(floodPropertyId2)]),
                new(floodPropertyId3, [Create(floodPropertyId3), Create(floodPropertyId3)]),
                new(floodPropertyId4, [Create(floodPropertyId4), Create(floodPropertyId4), Create(floodPropertyId4)]),
            ];
        }
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodReportSourceData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 11, 26, 15, 31, 0, TimeSpan.Zero);

    public static FloodReportSource Create(Guid? floodReportId = null, Guid? sourceId = null, string? sourceName = null)
    {
        return new()
        {
            FloodReportId = floodReportId ?? Guid.NewGuid(),
            SourceId = sourceId ?? Guid.NewGuid(),
            CreatedUtc = TestCreatedDate,
            Name = sourceName ?? "Test Source",
        };
    }

    public static IEnumerable<TheoryDataRow<FloodReportSource>> Four => [
        new(Create()),
        new(Create()),
        new(Create()),
        new(Create()),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReportSource>>> Collection_Four => [
        new([]),
        new([Create()]),
        new([Create(), Create()]),
        new([Create(), Create(), Create()]),
    ];

    public static IEnumerable<TheoryDataRow<Guid, ICollection<FloodReportSource>>> ForFloodReport
    {
        get
        {
            var floodReportId1 = Guid.NewGuid();
            var floodReportId2 = Guid.NewGuid();
            var floodReportId3 = Guid.NewGuid();
            var floodReportId4 = Guid.NewGuid();

            return [
                new(floodReportId1, []),
                new(floodReportId2, [Create(floodReportId2)]),
                new(floodReportId3, [Create(floodReportId3), Create(floodReportId3)]),
                new(floodReportId4, [Create(floodReportId4), Create(floodReportId4), Create(floodReportId4)]),
            ];
        }
    }
}

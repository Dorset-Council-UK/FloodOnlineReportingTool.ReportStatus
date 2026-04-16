using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodReportSourceData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 11, 26, 15, 31, 0, TimeSpan.Zero);

    public static FloodReportSource Create(Guid? floodReportId = null, Guid? sourceId = null, string? name = null)
    {
        var reportId = floodReportId ?? Guid.NewGuid();

        return new FloodReportSource
        {
            Id = sourceId ?? Guid.NewGuid(),
            Reference = "HKFGHDSDFCW",
            ViewUri = new Uri($"https://example.com/floodreport/HKFGHDSDFCW"),
            CreatedUtc = TestCreatedDate,
            UpdatedUtc = null,
            EligibilityCheckId = null,
            Uprn = null,
            Usrn = null,
            Geom = new Point(123456, 654321),
            ImpactStartUtc = TestCreatedDate,
            ImpactDurationHours = 1,
            IsOnGoing = false,
            IsUninhabitable = false,
            VulnerableCount = 6,
            LocationDescription = "Test location description",
            Organisations = [],
            FloodCauses = [
                new FloodReportSourceFloodCause
                {
                    FloodReportId = reportId,
                    FloodReportSourceId = reportId,
                    CauseId = PrimaryCauseIds.River,
                    Name = "River",
                },
                new FloodReportSourceFloodCause
                {
                    FloodReportId = reportId,
                    FloodReportSourceId = reportId,
                    CauseId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                    Name = "Rainwater flowing over the ground",
                },
            ],
            FloodReportId = floodReportId,
            FloodReport = null
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

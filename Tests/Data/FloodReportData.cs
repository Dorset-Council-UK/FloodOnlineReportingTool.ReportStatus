using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodReportData
{
    internal static long FixedUprn = 10023242411;
    internal static long FixedUsrn = 20023242411;
    internal static double Easting = 368991.12;
    internal static double Northing = 90881.94;

    public static readonly FloodReport FloodReport1 = new ()
    {
        Id = Guid.Parse("11111111-1111-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-1111-0000-0000-000000000000"),
        Uprn = null,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReport2 = new()
    {
        Id = Guid.Parse("11111111-2222-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-2222-0000-0000-000000000000"),
        Uprn = null,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReport3 = new()
    {
        Id = Guid.Parse("11111111-3333-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-3333-0000-0000-000000000000"),
        Uprn = null,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUprn1 = new()
    {
        Id = Guid.Parse("11111111-1111-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-1111-0000-0000-000000000000"),
        Uprn = FixedUprn,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUprn2 = new()
    {
        Id = Guid.Parse("11111111-2222-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-2222-0000-0000-000000000000"),
        Uprn = FixedUprn,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUprn3 = new()
    {
        Id = Guid.Parse("11111111-3333-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-3333-0000-0000-000000000000"),
        Uprn = FixedUprn,
        Usrn = null,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUsrn1 = new()
    {
        Id = Guid.Parse("11111111-1111-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-1111-0000-0000-000000000000"),
        Uprn = null,
        Usrn = FixedUsrn,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-1111-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUsrn2 = new()
    {
        Id = Guid.Parse("11111111-2222-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-2222-0000-0000-000000000000"),
        Uprn = null,
        Usrn = FixedUsrn,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-2222-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static readonly FloodReport FloodReportWithUsrn3 = new()
    {
        Id = Guid.Parse("11111111-3333-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-3333-0000-0000-000000000000"),
        Uprn = null,
        Usrn = FixedUsrn,
        Easting = Easting,
        Northing = Northing,
        ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
        ImpactDurationHours = 5,
        Sources = [
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.River,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "River",
            },
            new()
            {
                FloodReportId = Guid.Parse("11111111-3333-0000-0000-000000000000"),
                SourceId = PrimaryCauseIds.RainwaterFlowingOverTheGround,
                CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
                UpdatedUtc = null,
                Name = "Rainwater flowing over the ground",
            },
        ],
    };

    public static IEnumerable<TheoryDataRow<FloodReport>> FloodReport_3 => [
        new(FloodReport1),
        new(FloodReport2),
        new(FloodReport3),
    ];

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReport_Collections_4 => [
        new([]),
        new([FloodReport1]),
        new([FloodReport1, FloodReport2]),
        new([FloodReport1, FloodReport2, FloodReport3]),
    ];

    public static IEnumerable<object[]> FloodReportWithUprn_Collections_4 => [
        [new List<FloodReport>()],
        [new List<FloodReport> { FloodReportWithUprn1 }],
        [new List<FloodReport> { FloodReportWithUprn1, FloodReportWithUprn2 }],
        [new List<FloodReport> { FloodReportWithUprn1, FloodReportWithUprn2, FloodReportWithUprn3 }],
    ];

    public static IEnumerable<object[]> FloodReportWithUsrn_Collections_4 => [
        [new List<FloodReport>()],
        [new List<FloodReport> { FloodReportWithUsrn1 }],
        [new List<FloodReport> { FloodReportWithUsrn1, FloodReportWithUsrn2 }],
        [new List<FloodReport> { FloodReportWithUsrn1, FloodReportWithUsrn2, FloodReportWithUsrn3 }],
    ];

    public static IEnumerable<object[]> FloodReport_Within50Metres_Collections
    {
        get
        {
            var fr1 = FloodReport1 with { Easting = 368991.11, Northing = 90881.11 };
            var fr2 = FloodReport2 with { Easting = 368976.22, Northing = 90779.22 };
            var fr3 = FloodReport3 with { Easting = 368950.33, Northing = 90750.33 };

            return [
                [new List<FloodReport> { fr1 }],
                [new List<FloodReport> { fr1, fr2 }],
                [new List<FloodReport> { fr1, fr2, fr3 }],
            ];
        }
    }
}

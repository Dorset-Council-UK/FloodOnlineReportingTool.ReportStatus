using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodReportData
{
    internal static long FixedUprn = 10023242411;
    internal static long FixedUsrn = 20023242411;

    public static readonly FloodReport FloodReport1 = new ()
    {
        Id = Guid.Parse("11111111-1111-0000-0000-000000000000"),
        CreatedUtc = new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        UpdatedUtc = null,
        EligibilityCheckId = Guid.Parse("22222222-1111-0000-0000-000000000000"),
        Uprn = null,
        Usrn = null,
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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
        Easting = 368991.12,
        Northing = 90881.94,
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

    public static IEnumerable<object[]> FloodReport_3 => [
        [FloodReport1],
        [FloodReport2],
        [FloodReport3],
    ];

    public static IEnumerable<object[]> FloodReport_Collections_4 => [
        [new List<FloodReport>()],
        [new List<FloodReport> { FloodReport1 }],
        [new List<FloodReport> { FloodReport1, FloodReport2 }],
        [new List<FloodReport> { FloodReport1, FloodReport2, FloodReport3 }],
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

    public static IEnumerable<object[]> FloodReport_Within50Metres_Collections => [
        [368991.12, 90881.94, new List<FloodReport> { FloodReport1 }],
        [368991.12, 90881.94, new List<FloodReport> { FloodReport1, FloodReport2 }],
        [368991.12, 90881.94, new List<FloodReport> { FloodReport1, FloodReport2, FloodReport3 }],
    ];
}

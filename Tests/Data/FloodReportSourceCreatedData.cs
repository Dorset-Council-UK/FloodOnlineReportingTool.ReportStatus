using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.Contracts.Shared.Models;

namespace Tests.Data;

public static class FloodReportSourceCreatedData
{
    public static readonly FloodReportSourceCreated Property1 = new(
        0,
        "ABCDEFG0111",
        new Uri("https://test.com/floodreport/ABCDEFG0111"), 
        new EligibilityCheckRecord(
            Guid.Parse("11111111-1111-1111-0000-000000000000"),
            10023242411,
            null,
            368991.11,
            90881.11,
            new DateTimeOffset(2024, 12, 25, 1, 1, 1, TimeSpan.Zero),
            6,
            false,
            false,
            0,
            "Test location description 1",
            [],
            [
                new(PrimaryCauseIds.River, "River"),
                new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
            ]
        ),
        false,
        false,
        []
    );

    public static readonly FloodReportSourceCreated Property2 = new(
        0,
        "ABCDEFG0222",
        new Uri("https://test.com/floodreport/ABCDEFG0222"),
        new EligibilityCheckRecord(
            Guid.Parse("11111111-1111-2222-0000-000000000000"),
            10023242411,
            null,
            368976.22,
            90779.22,
            new DateTimeOffset(2024, 1, 26, 2, 2, 2, TimeSpan.Zero),
            6,
            false,
            false,
            0,
            "Test location description 2",
            [],
            [
                new(PrimaryCauseIds.River, "River"),
                new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
            ]
        ),
        false,
        false,
        []
    );
    public static readonly FloodReportSourceCreated Property3 = new(
        0,
        "ABCDEFG0333",
        new Uri("https://test.com/floodreport/ABCDEFG0333"),
        new EligibilityCheckRecord(
            Guid.Parse("11111111-1111-3333-0000-000000000000"),
            10023242411,
            null,
            368950.33,
            90750.33,
            new DateTimeOffset(2024, 2, 24, 3, 3, 3, TimeSpan.Zero),
            6,
            false,
            false,
            0,
            "Test location description 3",
            [],
            [
                new(PrimaryCauseIds.River, "River"),
                new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
            ]
        ),
        false,
        false,
        []
    );
    public static readonly FloodReportSourceCreated Property4 = new(
        0,
        "ABCDEFG0444",
        new Uri("https://test.com/floodreport/ABCDEFG0444"),
        new EligibilityCheckRecord(
            Guid.Parse("11111111-1111-4444-0000-000000000000"),
            10023242411,
            null,
            368859.44,
            90504.44,
            new DateTimeOffset(2025, 3, 28, 4, 4, 4, TimeSpan.Zero),
            6,
            false,
            false,
            0,
            "Test location description 4",
            [],
            [
                new(PrimaryCauseIds.River, "River"),
                new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
            ]
        ),
        false,
        false,
        []
    );

    public static IEnumerable<TheoryDataRow<FloodReportSourceCreated, string>> FloodProperty_4 => [
        new(Property1, "Test location description 1"),
        new(Property2, "Test location description 2"),
        new(Property3, "Test location description 3"),
        new(Property4, "Test location description 4"),
    ];

    public static readonly FloodReportSourceVerifyContact Verify1 = new(
        Guid.Parse("019fc847-ad92-7083-aa4c-1d2e1bc2ee34")
    );
}

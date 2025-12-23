using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;

namespace Tests.Data;

public static class EligibilityCheckCreatedData
{
    public static readonly EligibilityCheckCreated Property1 = new(
        Guid.Parse("11111111-1111-0000-0000-000000000000"),
        "ABCDEFG0111",
        new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
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
    );
    public static readonly EligibilityCheckCreated Property2 = new(
        Guid.Parse("11111111-2222-0000-0000-000000000000"),
        "ABCDEFG0222",
        new DateTimeOffset(2025, 2, 2, 2, 2, 2, TimeSpan.Zero),
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
    );
    public static readonly EligibilityCheckCreated Property3 = new(
        Guid.Parse("11111111-3333-0000-0000-000000000000"),
        "ABCDEFG0333",
        new DateTimeOffset(2025, 3, 3, 3, 3, 3, TimeSpan.Zero),
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
    );
    public static readonly EligibilityCheckCreated Property4 = new(
        Guid.Parse("11111111-4444-0000-0000-000000000000"),
        "ABCDEFG0444",
        new DateTimeOffset(2025, 4, 4, 4, 4, 4, TimeSpan.Zero),
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
    );

    public static readonly EligibilityCheckCreated Road1 = new(
        Guid.Parse("11111111-1111-0000-0000-000000000000"),
        "ABCDEFG0111",
        new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
        null,
        43201527,
        368991.11,
        90881.11,
        new DateTimeOffset(2024, 12, 25, 1, 1, 1, TimeSpan.Zero),
        6,
        false,
        false,
        0,
        "Test road description 1",
        [],
        [
            new(PrimaryCauseIds.River, "River"),
            new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
        ]
    );
    public static readonly EligibilityCheckCreated Road2 = new(
        Guid.Parse("11111111-2222-0000-0000-000000000000"),
        "ABCDEFG0222",
        new DateTimeOffset(2025, 2, 2, 2, 2, 2, TimeSpan.Zero),
        null,
        43201524,
        368976.22,
        90779.22,
        new DateTimeOffset(2024, 1, 26, 2, 2, 2, TimeSpan.Zero),
        6,
        false,
        false,
        0,
        "Test road description 2",
        [],
        [
            new(PrimaryCauseIds.River, "River"),
            new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
        ]
    );
    public static readonly EligibilityCheckCreated Road3 = new(
        Guid.Parse("11111111-3333-0000-0000-000000000000"),
        "ABCDEFG0333",
        new DateTimeOffset(2025, 3, 3, 3, 3, 3, TimeSpan.Zero),
        null,
        62105003,
        368950.33,
        90750.33,
        new DateTimeOffset(2024, 2, 24, 3, 3, 3, TimeSpan.Zero),
        6,
        false,
        false,
        0,
        "Test road description 3",
        [],
        [
            new(PrimaryCauseIds.River, "River"),
            new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
        ]
    );
    public static readonly EligibilityCheckCreated Road4 = new(
        Guid.Parse("11111111-4444-0000-0000-000000000000"),
        "ABCDEFG0444",
        new DateTimeOffset(2025, 4, 4, 4, 4, 4, TimeSpan.Zero),
        null,
        81100003,
        368859.44,
        90504.44,
        new DateTimeOffset(2025, 3, 28, 4, 4, 4, TimeSpan.Zero),
        6,
        false,
        false,
        0,
        "Test road description 4",
        [],
        [
            new(PrimaryCauseIds.River, "River"),
            new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
        ]
    );

    public static IEnumerable<TheoryDataRow<EligibilityCheckCreated, string>> FloodProperty_4 => [
        new(Property1, "Test location description 1"),
        new(Property2, "Test location description 2"),
        new(Property3, "Test location description 3"),
        new(Property4, "Test location description 4"),
    ];

    public static IEnumerable<TheoryDataRow<EligibilityCheckCreated, string>> FloodRoad_4 => [
        new(Road1, "Test road description 1"),
        new(Road2, "Test road description 2"),
        new(Road3, "Test road description 3"),
        new(Road4, "Test road description 4"),
    ];
}

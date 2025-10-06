using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using System.Diagnostics;

namespace Tests.Data;

internal static class FloodReportData
{
    internal static long FixedUprn = 10023242411;
    internal static long FixedUsrn = 20023242411;

    //[DebuggerDisplay("{Id}, Uprn = {Uprn}, Usrn = {Usrn}, Easting = {Easting}, Northing = {Northing}")]
    internal static readonly FloodReport FloodReport1 = new ()
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

    internal static readonly FloodReport FloodReport2 = new()
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

    internal static readonly FloodReport FloodReport3 = new()
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

    internal static readonly FloodReport FloodReportWithUprn1 = new()
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

    internal static readonly FloodReport FloodReportWithUprn2 = new()
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

    internal static readonly FloodReport FloodReportWithUprn3 = new()
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

    internal static readonly FloodReport FloodReportWithUsrn1 = new()
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

    internal static readonly FloodReport FloodReportWithUsrn2 = new()
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

    internal static readonly FloodReport FloodReportWithUsrn3 = new()
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
}

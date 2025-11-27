using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Data;

public static class FloodReportData
{
    internal static long FixedUprn = 10023242411;
    internal static long FixedUsrn = 20023242411;
    internal static double Easting = 368991.12;
    internal static double Northing = 90881.94;

    private static FloodReport FloodReport1 => Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000");
    private static FloodReport FloodReport2 => Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000");
    private static FloodReport FloodReport3 => Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000");

    public static FloodReport Create(string? floodReportId = null, string? eligibilityCheckId = null, DateTimeOffset? createdUtc = null, long? uprn = null, long? usrn = null, double? easting = null, double? northing = null)
    {
        Guid? floodReportGuid = floodReportId is not null ? Guid.Parse(floodReportId) : null;
        Guid? eligibilityCheckGuid = eligibilityCheckId is not null ? Guid.Parse(eligibilityCheckId) : null;
        return Create(floodReportGuid, eligibilityCheckGuid, createdUtc, uprn, usrn, easting, northing);
    }

    public static FloodReport Create(Guid? floodReportId = null, Guid? eligibilityCheckId = null, DateTimeOffset? createdUtc = null, long? uprn = null, long? usrn = null, double? easting = null, double? northing = null)
    {
        return new()
        {
            Id = floodReportId ?? Guid.NewGuid(),
            CreatedUtc = createdUtc ?? new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
            EligibilityCheckId = eligibilityCheckId ?? Guid.NewGuid(),
            Uprn = uprn,
            Usrn = usrn,
            Easting = easting ?? Easting,
            Northing = northing ?? Northing,
            ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
            ImpactDurationHours = 5,
            Sources = [
                FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.River, "River"),
                FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
            ],
        };
    }

    public static IEnumerable<TheoryDataRow<FloodReport>> Three => [
        new(FloodReport1),
        new(FloodReport2),
        new(FloodReport3),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReport>>> Collection_Four => [
        new([]),
        new([FloodReport1]),
        new([FloodReport1, FloodReport2]),
        new([FloodReport1, FloodReport2, FloodReport3]),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReport>>> CollectionWithUprn_Four
    {
        get
        {
            var withUprn1 = Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000", uprn: FixedUprn);
            var withUprn2 = Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000", uprn: FixedUprn);
            var withUprn3 = Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000", uprn: FixedUprn);

            return [
                new([]),
                new([withUprn1]),
                new([withUprn1, withUprn2]),
                new([withUprn1, withUprn2, withUprn3]),
            ];
        }
    }

    public static IEnumerable<TheoryDataRow<ICollection<FloodReport>>> CollectionWithUsrn_Four
    {
        get
        {
            var withUsrn1 = Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000", usrn: FixedUsrn);
            var withUsrn2 = Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000", usrn: FixedUsrn);
            var withUsrn3 = Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000", usrn: FixedUsrn);

            return [
                new([]),
                new([withUsrn1]),
                new([withUsrn1, withUsrn2]),
                new([withUsrn1, withUsrn2, withUsrn3]),
            ];
        }
    }

    public static IEnumerable<TheoryDataRow<ICollection<FloodReport>>> Collection_Within50Metres_Three
    {
        get
        {
            var floodReport1 = Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000", easting: 368991.11, northing: 90881.11);
            var floodReport2 = Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000", easting: 368976.22, northing: 90779.22);
            var floodReport3 = Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000", easting: 368950.33, northing: 90750.33);

            return [
                new([floodReport1]),
                new([floodReport1, floodReport2]),
                new([floodReport1, floodReport2, floodReport3]),
            ];
        }
    }
}

using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FloodReportData
{
    internal static long FixedUprn = 10023242411;
    internal static long FixedUsrn = 20023242411;
    internal static double Easting = 368991.12;
    internal static double Northing = 90881.94;

    private static readonly FloodReportSource FloodReport1 = Create(floodReportId: "11111111-1111-0000-0000-000000000000", eligibilityCheckId: "22222222-1111-0000-0000-000000000000");
    private static readonly FloodReportSource FloodReport2 = Create(floodReportId: "11111111-2222-0000-0000-000000000000", eligibilityCheckId: "22222222-2222-0000-0000-000000000000");
    private static readonly FloodReportSource FloodReport3 = Create(floodReportId: "11111111-3333-0000-0000-000000000000", eligibilityCheckId: "22222222-3333-0000-0000-000000000000");

    public static FloodReportSource Create(string? floodReportId = null, string? eligibilityCheckId = null, DateTimeOffset? createdUtc = null, long? uprn = null, long? usrn = null, double? easting = null, double? northing = null)
    {
        Guid? floodReportGuid = floodReportId is not null ? Guid.Parse(floodReportId) : null;
        Guid? eligibilityCheckGuid = eligibilityCheckId is not null ? Guid.Parse(eligibilityCheckId) : null;
        return Create(floodReportGuid, eligibilityCheckGuid, createdUtc, uprn, usrn, easting, northing);
    }

    public static FloodReportSource Create(Guid? floodReportId = null, Guid? eligibilityCheckId = null, DateTimeOffset? createdUtc = null, long? uprn = null, long? usrn = null, double? easting = null, double? northing = null)
    {
        var reportId = floodReportId ?? Guid.NewGuid();
        var checkId = eligibilityCheckId ?? Guid.NewGuid();

        return new()
        {
            Id = checkId,
            FloodReportId = reportId,
            Reference = "TestReference",
            ViewUri = new Uri("https://example.com/floodreport"),
            CreatedUtc = createdUtc ?? new DateTimeOffset(2025, 1, 1, 1, 1, 1, TimeSpan.Zero),
            //EligibilityCheckId = checkId,
            Uprn = uprn,
            Usrn = usrn,
            Geom = new Point(easting ?? Easting, northing ?? Northing) { SRID = 27700 },
            ImpactStartUtc = new DateTimeOffset(2024, 12, 10, 10, 10, 10, TimeSpan.Zero),
            ImpactDurationHours = 5,
            Organisations = [],
            FloodCauses =
            [
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
        };
    }

    public static IEnumerable<TheoryDataRow<FloodReportSource>> Three => [
        new(FloodReport1),
        new(FloodReport2),
        new(FloodReport3),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReportSource>>> Collection_Four => [
        new([]),
        new([FloodReport1]),
        new([FloodReport1, FloodReport2]),
        new([FloodReport1, FloodReport2, FloodReport3]),
    ];

    public static IEnumerable<TheoryDataRow<ICollection<FloodReportSource>>> CollectionWithUprn_Four
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

    public static IEnumerable<TheoryDataRow<ICollection<FloodReportSource>>> CollectionWithUsrn_Four
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

    /// <summary>
    /// Unit test data for flood reports within 50 metres.
    /// <see cref="double">Easting</see>, <see cref="double">Northing</see>, <see cref="double">Metres</see>,
    /// <see cref="ICollection&lt;&gt;">Expected Flood Reports</see>,
    /// </summary>
    public static IEnumerable<TheoryDataRow<double, double, double, ICollection<FloodReportSource>>> Four_Within50Metres
    {
        get
        {
            var coords = new[]
            {
                (Easting: 368991.12, Northing: 90881.94, Metres: 10.0),
                (Easting: 369500.00, Northing: 91000.00, Metres: 30.0),
                (Easting: 370000.00, Northing: 91500.00, Metres: 40.0),
                (Easting: 370500.00, Northing: 92000.00, Metres: 50.0),
            };

            var floodReport1 = Create(floodReportId: "11111111-1111-0000-0000-000000000000", easting: coords[1].Easting, northing: coords[1].Northing);
            var floodReport2 = Create(floodReportId: "11111111-2222-0000-0000-000000000000", easting: coords[2].Easting, northing: coords[2].Northing);
            var floodReport3 = Create(floodReportId: "11111111-3333-0000-0000-000000000000", easting: coords[3].Easting, northing: coords[3].Northing);

            return [
                new(coords[0].Easting, coords[0].Northing, coords[0].Metres, []),
                new(coords[1].Easting, coords[1].Northing, coords[1].Metres, [floodReport1]),
                new(coords[2].Easting, coords[2].Northing, coords[2].Metres, [floodReport1, floodReport2]),
                new(coords[3].Easting, coords[3].Northing, coords[3].Metres, [floodReport1, floodReport2, floodReport3]),
            ];
        }
    }
}

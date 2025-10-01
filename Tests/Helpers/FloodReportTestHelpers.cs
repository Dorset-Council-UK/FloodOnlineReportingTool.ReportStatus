using Bogus;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Helpers;

internal static class FloodReportTestHelpers
{
    private static readonly IEnumerable<TheoryDataRow<FloodReport>> _test_FloodReports_10;
    private static readonly IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> _test_FloodReport_Collections_4;
    private static readonly IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> _test_FloodReportWithUprn_Collections_4;
    private static readonly IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> _test_FloodReportWithUsrn_Collections_4;

    public const long FixedUprn = 10023242411;
    public const long FixedUsrn = 43201527;
    public const double FixedEasting = 368991.12;
    public const double FixedNorthing = 90881.94;

    static FloodReportTestHelpers()
    {
        Randomizer.Seed = new Random(232589734);

        bool useUprn = false;
        long? uprn = null;
        bool useUsrn = false;
        long? usrn = null;
        double? easting = null;
        double? northing = null;

        var floodReportFaker = new Faker<FloodReport>("en_GB")
            .StrictMode(ensureRulesForAllProperties: true)
            .RuleFor(o => o.Id, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Date.RecentOffset().OrNull(f))
            .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
            .RuleFor(o => o.Uprn, f => useUprn ? uprn : f.Random.Long(1, 9999999999).OrNull(f))
            .RuleFor(o => o.Usrn, f => useUsrn ? usrn : f.Random.Long(1, 9999999999).OrNull(f))
            .RuleFor(o => o.Easting, f => easting ?? f.Random.Double(0, 700000))
            .RuleFor(o => o.Northing, f => northing ?? f.Random.Double(0, 1300000))
            .RuleFor(o => o.ImpactStartUtc, f => f.Date.PastOffset(5).OrNull(f))
            .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72));

        // 10 test FloodReports
        IReadOnlyCollection<FloodReport> floodReports_10 = floodReportFaker.Generate(10);
        _test_FloodReports_10 = floodReports_10
            .Select(fr => new TheoryDataRow<FloodReport>(fr))
            .AsEnumerable();

        // 4 test collections of FloodReports
        IReadOnlyCollection<IReadOnlyCollection<FloodReport>> floodReportCollections_4 = [
            floodReportFaker.Generate(0),
            floodReportFaker.Generate(1),
            floodReportFaker.Generate(5),
            floodReportFaker.Generate(10),
        ];
        _test_FloodReport_Collections_4 = floodReportCollections_4
            .Select(frc => new TheoryDataRow<IReadOnlyCollection<FloodReport>>(frc))
            .AsEnumerable();

        // 4 test collections of FloodReports with fixed UPRN
        useUprn = true;
        uprn = FixedUprn;
        IReadOnlyCollection<IReadOnlyCollection<FloodReport>> floodReportWithUprnCollections_4 = [
            floodReportFaker.Generate(0),
            floodReportFaker.Generate(1),
            floodReportFaker.Generate(5),
            floodReportFaker.Generate(10),
        ];
        _test_FloodReportWithUprn_Collections_4 = floodReportWithUprnCollections_4
            .Select(frc => new TheoryDataRow<IReadOnlyCollection<FloodReport>>(frc))
            .AsEnumerable();

        // 4 test collections of FloodReports with fixed USRN
        useUprn = true;
        uprn = null;
        useUsrn = true;
        usrn = FixedUsrn;
        IReadOnlyCollection<IReadOnlyCollection<FloodReport>> floodReportWithUsrnCollections_4 = [
            floodReportFaker.Generate(0),
            floodReportFaker.Generate(1),
            floodReportFaker.Generate(5),
            floodReportFaker.Generate(10),
        ];
        _test_FloodReportWithUsrn_Collections_4 = floodReportWithUsrnCollections_4
            .Select(frc => new TheoryDataRow<IReadOnlyCollection<FloodReport>>(frc))
            .AsEnumerable();

    }

    public static IEnumerable<TheoryDataRow<FloodReport>> FloodReports_10()
    {
        foreach (var testFloodReport in _test_FloodReports_10)
        {
            yield return testFloodReport;
        }
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReport_Collections_4()
    {
        foreach (var testFloodReport_Collection in _test_FloodReport_Collections_4)
        {
            yield return testFloodReport_Collection;
        }
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReportWithUprn_Collections_4()
    {
        foreach (var testFloodReportWithUprn_Collection in _test_FloodReportWithUprn_Collections_4)
        {
            yield return testFloodReportWithUprn_Collection;
        }
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReportWithUsrn_Collections_4()
    {
        foreach (var testFloodReportWithUsrn_Collection in _test_FloodReportWithUsrn_Collections_4)
        {
            yield return testFloodReportWithUsrn_Collection;
        }
    }
}
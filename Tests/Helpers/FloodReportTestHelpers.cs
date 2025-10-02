using Bogus;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Helpers;

internal record FloodReportGenerationOptions(
    bool RandomUprn = true,
    long? FixedUprn = null,
    bool RandomUsrn = true,
    long? FixedUsrn = null,
    bool RandomEasting = true,
    double? Easting = null,
    bool RandomNorthing = true,
    double? Northing = null
);

internal static class FloodReportTestHelpers
{
    static FloodReportTestHelpers()
    {
        Randomizer.Seed = new Random(232589734);
    }

    // make a series of fairly good flood report source names so that faker.PickRandom(count) can be used
    private static readonly IEnumerable<string> PrimaryCauseNames = [
        "River",
        "Stream / Watercourse",
        "Lake / Reservoirs",
        "The Sea",
        "Ditches and drainage channels",
        "Water rising out of the ground",
        "Foul drainage (Sewerage)",
        "Surface water drainage",
        "Blocked road drainage",
        "Bridge / culvert",
        "Waves caused by vehicles",
        "Rainwater flowing over the ground",
        "Not Sure",
    ];

    public static IEnumerable<TheoryDataRow<FloodReport>> CreateFloodReports(int count, FloodReportGenerationOptions? options = null)
    {
        options ??= new FloodReportGenerationOptions();

        var floodReportSourceFaker = new Faker<FloodReportSource>("en_GB")
            .RuleFor(o => o.FloodReportId, f => f.Random.Uuid())
            .RuleFor(o => o.SourceId, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Date.RecentOffset().OrNull(f))
            .RuleFor(o => o.Name, f => f.PickRandom(PrimaryCauseNames));

        var floodReportFaker = new Faker<FloodReport>("en_GB")
            //.StrictMode(ensureRulesForAllProperties: true)
            .RuleFor(o => o.Id, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Date.RecentOffset().OrNull(f))
            .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
            .RuleFor(o => o.Uprn, f =>
                options.RandomUprn
                    ? f.Random.Long(1, 9999999999).OrNull(f)
                    : options.FixedUprn)
            .RuleFor(o => o.Usrn, f =>
                options.RandomUsrn
                    ? f.Random.Long(1, 9999999999).OrNull(f)
                    : options.FixedUsrn)
            .RuleFor(o => o.Easting, f =>
                options.RandomEasting
                    ? f.Random.Double(0, 700000)
                    : options.Easting ?? 0)
            .RuleFor(o => o.Northing, f =>
                options.RandomNorthing
                    ? f.Random.Double(0, 1300000)
                    : options.Northing ?? 0)
            .RuleFor(o => o.ImpactStartUtc, f => f.Date.PastOffset(5).OrNull(f))
            .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72))
            .RuleFor(o => o.Sources, f => floodReportSourceFaker.Generate(f.Random.Int(1, 5)));

        var list = floodReportFaker.Generate(count);
        return list.Select(fr => new TheoryDataRow<FloodReport>(fr));
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> CreateFloodReport_Collections(int count, FloodReportGenerationOptions? options = null)
    {
        // each flood report should use CreateFloodReports with options
        // but with a random number of flood reports in them
        // count is the number of collections to create
        options ??= new FloodReportGenerationOptions();

        var list = new List<IReadOnlyCollection<FloodReport>>(count);
        

    }
}
using Bogus;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Helpers;

internal static class FloodReportTestHelpers
{
    internal static List<FloodReport> CreateFloodReports(int count, string locale = "en_GB", long? uprn = null, long? usrn = null, double? easting = null, double? northing = null)
    {
        Randomizer.Seed = new Random(232589734);

        var floodReportFaker = new Faker<FloodReport>(locale)
            .RuleFor(o => o.Id, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Random.Bool() ? f.Date.RecentOffset() : null)
            .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
            .RuleFor(o => o.Uprn, f => FixedLongRule(f, uprn))
            .RuleFor(o => o.Usrn, f => FixedLongRule(f, usrn))
            .RuleFor(o => o.Easting, f => easting ?? f.Random.Double(0, 700000))
            .RuleFor(o => o.Northing, f => northing ?? f.Random.Double(0, 1300000))
            .RuleFor(o => o.ImpactStartUtc, f => f.Random.Bool() ? f.Date.PastOffset(5) : null)
            .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72));

        return floodReportFaker.Generate(count);
    }

    private static long? FixedLongRule(Faker faker, long? value)
    {
        if (value is not null)
        {
            return value.Value;
        }

        return faker.Random.Bool() ? faker.Random.Long(1, 9999999999) : null;
    }

    internal static long[] CreateUprns(int count, string locale = "en_GB")
    {
        Randomizer.Seed = new Random(232589734);
        var faker = new Faker(locale);

        var uprns = new long[count];
        for (int i = 0; i < count; i++)
        {
            uprns[i] = faker.Random.Long(1, 9999999999);
        }

        return uprns;
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReportCollection()
    {
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(0));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(1));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(5));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(10));
    }

    public static IEnumerable<TheoryDataRow<FloodReport>> FloodReportCollection_100()
    {
        var floodReports = CreateFloodReports(100);
        foreach (var floodReport in floodReports)
        {
            yield return new TheoryDataRow<FloodReport>(floodReport);
        }
    }

    public static IEnumerable<TheoryDataRow<FloodReport>> FloodReportRecord()
    {
        var floodReports = CreateFloodReports(5);
        foreach (var floodReport in floodReports)
        {
            yield return new TheoryDataRow<FloodReport>(floodReport);
        }
    }

    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReportCollectionWithUprn()
    {
        Randomizer.Seed = new Random(232589734);
        var faker = new Faker("en_GB");
        var uprn = faker.Random.Long(1, 9999999999);

        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(0, uprn: uprn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(1, uprn: uprn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(5, uprn: uprn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(10, uprn: uprn));
    }


    public static IEnumerable<TheoryDataRow<IReadOnlyCollection<FloodReport>>> FloodReportCollectionWithUsrn()
    {
        Randomizer.Seed = new Random(232589734);
        var faker = new Faker("en_GB");
        var usrn = faker.Random.Long(1, 9999999999);

        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(0, usrn: usrn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(1, usrn: usrn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(5, usrn: usrn));
        yield return new TheoryDataRow<IReadOnlyCollection<FloodReport>>(CreateFloodReports(10, usrn: usrn));
    }
}
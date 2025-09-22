using Bogus;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Helpers;

internal static class FloodReportTestHelpers
{
    internal static List<FloodReport> Test100FloodReports => CreateFloodReports(100);

    public static IEnumerable<TheoryDataRow<FloodReport>> Test100TheoryFloodReports => Test100FloodReports.Select(o => new TheoryDataRow<FloodReport>(o));
    public static IEnumerable<TheoryDataRow<long>> Test10TheoryUprns => CreateUprns(10).Select(o => new TheoryDataRow<long>(o));

    //public static IEnumerable<TheoryDataRow<FloodReport>> Test100TheoryUprnFloodReports => CreateTheoryFloodReports(100, alwaysHasUprn: true);

    //public static IEnumerable<TheoryDataRow<FloodReport>> Test100TheoryUsrnFloodReports => CreateTheoryFloodReports(100, alwaysHasUsrn: true);

    //internal static List<FloodReport> CreateFloodReports(int count, string locale = "en_GB", bool sameUprn = false, bool sameUsrn = false)
    //{
    //    Randomizer.Seed = new Random(232589734);

    //    var fixedUprn = new Faker().Random.Long(1, 9999999999);
    //    var fixedUsrn = new Faker().Random.Long(1, 9999999999);

    //    var floodReportFaker = new Faker<FloodReport>(locale)
    //        .RuleFor(o => o.Id, f => f.Random.Uuid())
    //        .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
    //        .RuleFor(o => o.UpdatedUtc, f => f.Random.Bool() ? f.Date.RecentOffset() : null)
    //        .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
    //        .RuleFor(o => o.Uprn, f => FixedLongRule(f, sameUprn, fixedUprn))
    //        .RuleFor(o => o.Usrn, f => FixedLongRule(f, sameUsrn, fixedUsrn))
    //        .RuleFor(o => o.Easting, f => f.Random.Double(0, 700000))
    //        .RuleFor(o => o.Northing, f => f.Random.Double(0, 1300000))
    //        .RuleFor(o => o.ImpactStartUtc, f => f.Random.Bool() ? f.Date.PastOffset(5) : null)
    //        .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72));

    //    return floodReportFaker.Generate(count);
    //}
    internal static List<FloodReport> CreateFloodReports(int count, string locale = "en_GB", long? uprn = null, long? usrn = null)
    {
        Randomizer.Seed = new Random(232589734);

        var floodReportFaker = new Faker<FloodReport>(locale)
            .RuleFor(o => o.Id, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Random.Bool() ? f.Date.RecentOffset() : null)
            .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
            .RuleFor(o => o.Uprn, f => FixedLongRule(f, uprn))
            .RuleFor(o => o.Usrn, f => FixedLongRule(f, usrn))
            .RuleFor(o => o.Easting, f => f.Random.Double(0, 700000))
            .RuleFor(o => o.Northing, f => f.Random.Double(0, 1300000))
            .RuleFor(o => o.ImpactStartUtc, f => f.Random.Bool() ? f.Date.PastOffset(5) : null)
            .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72));

        return floodReportFaker.Generate(count);
    }

    internal static List<long> CreateUprns(int count, string locale = "en_GB")
    {
        Randomizer.Seed = new Random(232589734);

        var uprnFaker = new Faker<long>(locale)
            .RuleFor(o => o, f => f.Random.Long(1, 9999999999));

        return uprnFaker.Generate(count);
    }

    private static long? FixedLongRule(Faker faker, bool useFixed, long fixedValue)
    {
        if (useFixed)
        {
            return fixedValue;
        }

        return faker.Random.Bool() ? faker.Random.Long(1, 9999999999) : null;
    }

    private static long? FixedLongRule(Faker faker, long? value)
    {
        if (value is not null)
        {
            return value.Value;
        }

        return faker.Random.Bool() ? faker.Random.Long(1, 9999999999) : null;
    }

    internal static IEnumerable<TheoryDataRow<FloodReport>> CreateTheoryFloodReports(int count, string locale = "en_GB")
    {
        return CreateFloodReports(count, locale)
            .Select(o => new TheoryDataRow<FloodReport>(o));
    }
}
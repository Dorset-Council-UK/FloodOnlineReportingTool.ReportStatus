using Bogus;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace Tests.Helpers;

internal static class FloodReportTestHelpers
{
    internal static IEnumerable<TheoryDataRow<FloodReport>> CreateFloodReports(int count, string locale = "en_GB")
    {
        Randomizer.Seed = new Random(232589734);

        var floodReportFaker = new Faker<FloodReport>(locale)
            .RuleFor(o => o.Id, f => f.Random.Uuid())
            .RuleFor(o => o.CreatedUtc, f => f.Date.RecentOffset())
            .RuleFor(o => o.UpdatedUtc, f => f.Random.Bool() ? f.Date.RecentOffset() : null)
            .RuleFor(o => o.EligibilityCheckId, f => f.Random.Uuid())
            .RuleFor(o => o.Uprn, f => f.Random.Bool() ? f.Random.Long(1, 9999999999) : null)
            .RuleFor(o => o.Usrn, f => f.Random.Bool() ? f.Random.Long(1, 9999999999) : null)
            .RuleFor(o => o.Easting, f => f.Random.Double(0, 700000))
            .RuleFor(o => o.Northing, f => f.Random.Double(0, 1300000))
            .RuleFor(o => o.ImpactStartUtc, f => f.Random.Bool() ? f.Date.PastOffset(5) : null)
            .RuleFor(o => o.ImpactDurationHours, f => f.Random.Int(1, 72));

        return floodReportFaker
            .Generate(count)
            .Select(o => new TheoryDataRow<FloodReport>(o));
    }
}
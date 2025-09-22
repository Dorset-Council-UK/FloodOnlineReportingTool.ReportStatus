using Bogus;
using FloodOnlineReportingTool.Contracts;

namespace Tests.Helpers;

internal static class EligibilityCheckCreatedTestHelpers
{
    internal static IEnumerable<TheoryDataRow<EligibilityCheckCreated>> CreateMessages(int count, string locale = "en_GB")
    {
        Randomizer.Seed = new Random(232589734);

        var eligibilityCheckOrganisationFaker = new Faker<EligibilityCheckOrganisation>(locale)
            .CustomInstantiator(f => new(
                f.Random.Uuid(),
                f.Company.CompanyName(),
                f.Random.Uuid(),
                f.Commerce.ProductName()
            ));

        var eligibilityCheckSourceFaker = new Faker<EligibilityCheckFloodSource>(locale)
            .CustomInstantiator(f => new(
                f.Random.Uuid(),
                f.Commerce.ProductName()
            ));

        var eligibilityCheckCreatedFaker = new Faker<EligibilityCheckCreated>(locale)
            .CustomInstantiator(f => new(
                f.Random.Uuid(),
                f.Random.Hexadecimal(8, "").ToUpperInvariant(),
                f.Date.RecentOffset(),
                f.Random.Bool() ? f.Random.Long(1, 9999999999) : null,
                f.Random.Bool() ? f.Random.Long(1, 9999999999) : null,
                f.Random.Double(0, 700000),
                f.Random.Double(0, 1300000),
                f.Random.Bool() ? f.Date.PastOffset(5) : null,
                f.Random.Int(1, 72),
                f.Random.Bool(),
                f.Random.Bool(),
                f.Random.Bool() ? f.Random.Int(0, 99) : null,
                eligibilityCheckOrganisationFaker.GenerateBetween(1, 3),
                eligibilityCheckSourceFaker.GenerateBetween(1, 3)
            ));

        return eligibilityCheckCreatedFaker
            .Generate(count)
            .Select(o => new TheoryDataRow<EligibilityCheckCreated>(o));
    }
}
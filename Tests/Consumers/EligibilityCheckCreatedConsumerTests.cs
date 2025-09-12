using Bogus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Consumers;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Tests.Consumers;

public class EligibilityCheckCreatedConsumerTests
{
    public static IEnumerable<TheoryDataRow<EligibilityCheckCreated>> CreateMessages(int count, string locale = "en_GB")
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
                eligibilityCheckOrganisationFaker.GenerateBetween<EligibilityCheckOrganisation>(1, 3),
                eligibilityCheckSourceFaker.GenerateBetween<EligibilityCheckFloodSource>(1, 3)
            ));

        return eligibilityCheckCreatedFaker
            .Generate(count)
            .Select(o => new TheoryDataRow<EligibilityCheckCreated>(o));
    }

    public static IEnumerable<TheoryDataRow<EligibilityCheckCreated>> GoodMessages => CreateMessages(100);

    [Theory]
    [MemberData(nameof(GoodMessages))]
    public async Task Consume_ShouldCreateReport_WhenNotDuplicate(EligibilityCheckCreated message)
    {
        // Arrange
        var logger = Substitute.For<ILogger<EligibilityCheckCreatedConsumer>>();
        var reportService = Substitute.For<IReportService>();
        var reportDuplicateService = Substitute.For<IReportDuplicateService>();

        var consumer = new EligibilityCheckCreatedConsumer(logger, reportService, reportDuplicateService);
        var consumeContext = Substitute.For<ConsumeContext<EligibilityCheckCreated>>();
        consumeContext.Message.Returns(message);
        consumeContext.CancellationToken.Returns(CancellationToken.None);

        // All duplicate checks will return null, so it's not a duplicate
        reportService.GetReportsByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);
        reportService.GetReportsByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);
        reportService.GetReportsWithinMetres(Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await reportService.Received(1).CreateReport(message.ToReportDto(), Arg.Any<CancellationToken>());
        await reportDuplicateService.DidNotReceiveWithAnyArgs().CreateDuplicate(Arg.Any<Guid>(), Arg.Any<ReportDuplicateDto>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Consume_ShouldCreateDuplicate_WhenReportFoundByUprn()
    {
        
    }

    [Fact]
    public async Task Consume_ShouldCreateDuplicate_WhenReportFoundByUsrn()
    {
    }

    [Fact]
    public async Task Consume_ShouldCreateDuplicate_WhenReportIsNearby()
    {
    }
}

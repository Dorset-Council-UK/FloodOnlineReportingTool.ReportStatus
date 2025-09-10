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
    private static List<EligibilityCheckCreated> TestMessages(int count, string locale = "en_GB")
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

        //var message1 = eligibilityCheckCreatedFaker.Generate();
        //message1 = message1 with { Uprn = 12345678L, VulnerableCount = 9999 };
        return eligibilityCheckCreatedFaker.Generate(count);
    }

    private static IEnumerable<object[]> ValidEligibilityCheckCreatedCases()
    {
        // public record EligibilityCheckCreated
        // (Guid Id, string FloodReportReference, DateTimeOffset CreatedUtc, long? Uprn, long? Usrn,
        // double Easting, double Northing, DateTimeOffset? ImpactStartUTC, int ImpactDurationHours, bool IsOnGoing, bool IsUninhabitable, int? VulnerableCount,
        // IReadOnlyCollection<EligibilityCheckOrganisation> Organisations, IReadOnlyCollection<EligibilityCheckFloodSource> FloodSources);
        yield return new object[] { new EligibilityCheckCreated { Uprn = null, Usrn = null, VulnerableCount = 0, /*...*/ } };
        yield return new object[] { new EligibilityCheckCreated { Uprn = 1, Usrn = null, VulnerableCount = 10, /*...*/ } };
        yield return new object[] { new EligibilityCheckCreated { Uprn = null, Usrn = 2, VulnerableCount = 5, /*...*/ } };
    }

    [Theory]
    [MemberData(nameof(ValidEligibilityCheckCreatedCases))]
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
        reportService.GetReportByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns((Report)null);
        reportService.GetReportByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns((Report)null);
        reportService.GetReportWithinMetres(Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>(), Arg.Any<CancellationToken>()).Returns((Report)null);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await reportService.Received(1).CreateReport(Arg.Any<ReportDto>(), Arg.Any<CancellationToken>());
        await reportDuplicateService.DidNotReceiveWithAnyArgs().CreateDuplicate(default, default, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Consume_ShouldCreateDuplicate_WhenReportFoundByUprn()
    {
        // Arrange
        var logger = Substitute.For<ILogger<EligibilityCheckCreatedConsumer>>();
        var reportService = Substitute.For<IReportService>();
        var reportDuplicateService = Substitute.For<IReportDuplicateService>();

        var consumer = new EligibilityCheckCreatedConsumer(logger, reportService, reportDuplicateService);

        var messageSourceId = Guid.NewGuid();
        var message = new EligibilityCheckCreated
        {
            Uprn = 12345,
            Usrn = null,
            Easting = 0,
            Northing = 0,
            FloodSources = [new FloodSource { Id = messageSourceId }],
            IsUninhabitable = false,
            VulnerableCount = 0,
            CreatedUtc = DateTime.UtcNow,
            ImpactStartUTC = DateTime.UtcNow,
            // Add other required properties
        };

        var consumeContext = Substitute.For<ConsumeContext<EligibilityCheckCreated>>();
        consumeContext.Message.Returns(message);
        consumeContext.CancellationToken.Returns(CancellationToken.None);

        // Set up a report with a matching source ID
        var report = new Report
        {
            Id = 42,
            Sources = [new ReportSource { SourceId = messageSourceId }],
            CreatedUtc = DateTime.UtcNow,
            ImpactStartUtc = DateTime.UtcNow
        };

        reportService.GetReportByUprn(message.Uprn.Value, Arg.Any<CancellationToken>()).Returns(report);

        // CreateDuplicate returns a non-null value, indicating it succeeded
        reportDuplicateService.CreateDuplicate(
            report.Id,
            Arg.Any<ReportDuplicateDto>(),
            Arg.Any<CancellationToken>()
        ).Returns(new ReportDuplicate());

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await reportDuplicateService.Received(1).CreateDuplicate(report.Id, Arg.Any<ReportDuplicateDto>(), Arg.Any<CancellationToken>());
        await reportService.DidNotReceiveWithAnyArgs().CreateReport(default, default);
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

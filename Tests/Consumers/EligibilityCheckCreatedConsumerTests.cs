using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Consumers;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tests.Data;

namespace Tests.Consumers;

public class EligibilityCheckCreatedConsumerTests
{
    private readonly ReportStatusSettings _reportStatusSettings = new()
    {
        LocationBufferMetres = 50,
        ApiKey = "",
        AddressNameUrl = new Uri("https://localhost.com"),
        StreetNameUrl = new Uri("https://localhost.com"),
    };

    [Theory]
    [MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    public async Task Consume_CallsFloodPropertyService_ForFloodPropertyReport(EligibilityCheckCreated expectedEligibilityCheckCreated, string expectedLocationDescription)
    {
        // Arrange the ConsumeContext
        var context = Substitute.For<ConsumeContext<EligibilityCheckCreated>>();
        context.Message.Returns(expectedEligibilityCheckCreated);

        // Arrange the services
        var logger = Substitute.For<ILogger<EligibilityCheckCreatedConsumer>>();
        var settings = Substitute.For<IOptions<ReportStatusSettings>>();
        var gazetteerSearchService = Substitute.For<IGazetteerSearchService>();
        var floodPropertyService = Substitute.For<IFloodPropertyService>();
        var floodRoadService = Substitute.For<IFloodRoadService>();
        var floodLocationService = Substitute.For<IFloodLocationService>();

        settings.Value.Returns(_reportStatusSettings);
        gazetteerSearchService.GetAddressNameByUPRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>()).Returns("1 Test Street");
        gazetteerSearchService.GetStreetNameByUSRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>()).Returns("Test Road");

        var consumer = new EligibilityCheckCreatedConsumer(logger, settings, floodPropertyService, floodRoadService, floodLocationService, gazetteerSearchService);

        // Act
        await consumer.Consume(context);

        // Assert
        await floodPropertyService.Received(1).CreateFromEligibilityCheckCreated(context.Message, expectedLocationDescription, Arg.Any<CancellationToken>());
        await floodRoadService.Received(0).CreateFromEligibilityCheckCreated(context.Message, Arg.Any<string>(), Arg.Any<CancellationToken>());
        await floodLocationService.Received(0).CreateFromEligibilityCheckCreated(context.Message, Arg.Any<string>(), Arg.Any<double>(), Arg.Any<CancellationToken>());
        await gazetteerSearchService.Received(0).GetAddressNameByUPRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
        await gazetteerSearchService.Received(0).GetStreetNameByUSRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
    }
}

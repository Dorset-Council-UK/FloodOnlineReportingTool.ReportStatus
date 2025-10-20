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
        PathBase = "",
        AddressNameUrl = new Uri("https://localhost.com"),
        StreetNameUrl = new Uri("https://localhost.com"),
        AccessTokenIssueDurationMonths = 6,
        OSApiKey = "",
    };

    [Theory]
    [MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    public async Task Consume_CallsFloodPropertyService_ForFloodPropertyReport(EligibilityCheckCreated expectedEligibilityCheckCreated, string expectedLocationDescription)
    {
        // Arrange the ConsumeContext
        var hostInfo = Substitute.For<HostInfo>();
        hostInfo.ToString().Returns("localhost");
        var context = Substitute.For<ConsumeContext<EligibilityCheckCreated>>();
        context.Message.Returns(expectedEligibilityCheckCreated);
        context.CancellationToken.Returns(TestContext.Current.CancellationToken);
        context.Host.Returns(hostInfo);

        var refererUri = new UriBuilder("https", context.Host.ToString(), 443, "ConsumeEligibilityCheck").Uri;
        var testAddressName = "1 Test Street";
        var testRoadName = "Test Road";

        // Arrange the services
        var logger = Substitute.For<ILogger<EligibilityCheckCreatedConsumer>>();
        var settings = Substitute.For<IOptions<ReportStatusSettings>>();
        var gazetteerSearchService = Substitute.For<IGazetteerSearchService>();
        var floodPropertyService = Substitute.For<IFloodPropertyService>();
        var floodRoadService = Substitute.For<IFloodRoadService>();
        var floodLocationService = Substitute.For<IFloodLocationService>();

        settings.Value.Returns(_reportStatusSettings);
        gazetteerSearchService.GetAddressNameByUPRN(Arg.Any<long>(), refererUri, Arg.Any<CancellationToken>()).Returns(testAddressName);
        gazetteerSearchService.GetStreetNameByUSRN(Arg.Any<long>(), refererUri, Arg.Any<CancellationToken>()).Returns(testRoadName);

        var consumer = new EligibilityCheckCreatedConsumer(logger, settings, floodPropertyService, floodRoadService, floodLocationService, gazetteerSearchService);

        // Act
        await consumer.Consume(context);

        // Assert
        await floodPropertyService.Received(1).CreateFromEligibilityCheckCreated(context.Message, expectedLocationDescription, Arg.Any<CancellationToken>());
        await floodRoadService.DidNotReceiveWithAnyArgs().CreateFromEligibilityCheckCreated(Arg.Any<EligibilityCheckCreated>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        await floodLocationService.DidNotReceiveWithAnyArgs().CreateFromEligibilityCheckCreated(Arg.Any<EligibilityCheckCreated>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        await gazetteerSearchService.DidNotReceiveWithAnyArgs().GetAddressNameByUPRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
        await gazetteerSearchService.DidNotReceiveWithAnyArgs().GetStreetNameByUSRN(Arg.Any<long>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
    }
}

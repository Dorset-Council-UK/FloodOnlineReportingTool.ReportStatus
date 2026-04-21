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

public class FloodReportSourceCreatedConsumerTests
{
    private readonly ReportStatusSettings _reportStatusSettings = new()
    {
        LocationBufferMetres = 50,
        ApiKey = "",
        AddressNameUrl = new Uri("https://localhost.com"),
        StreetNameUrl = new Uri("https://localhost.com"),
    };

    [Theory]
    [MemberData(nameof(FloodReportSourceCreatedData.FloodProperty_4), MemberType = typeof(FloodReportSourceCreatedData))]
    public async Task Consume_CallsFloodPropertyService_ForFloodPropertyReport(FloodReportSourceCreated expectedFloodReportSourceCreated, string expectedLocationDescription)
    {
        // Arrange the ConsumeContext
        var context = Substitute.For<ConsumeContext<FloodReportSourceCreated>>();
        context.Message.Returns(expectedFloodReportSourceCreated);

        // Arrange the services
        var logger = Substitute.For<ILogger<FloodReportSourceCreatedConsumer>>();
        var settings = Substitute.For<IOptions<ReportStatusSettings>>();
        var floodReportService = Substitute.For<IFloodReportService>();


        settings.Value.Returns(_reportStatusSettings);
        var consumer = new FloodReportSourceCreatedConsumer(logger, settings, floodReportService);

        // Act
        await consumer.Consume(context);

        // Assert
        await floodReportService.Received(1).CreateFromFloodReportCreated(context.Message, Arg.Any<CancellationToken>());
    }
}

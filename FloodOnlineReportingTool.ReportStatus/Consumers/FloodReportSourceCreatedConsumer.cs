using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Options;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class FloodReportSourceCreatedConsumer(
    ILogger<FloodReportSourceCreatedConsumer> logger,
    IOptions<ReportStatusSettings> settings,
    IFloodReportService floodReportService
) : IConsumer<FloodReportSourceCreated>
{
    public async Task Consume(ConsumeContext<FloodReportSourceCreated> context)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(FloodReportSourceCreated));

        var message = context.Message;
        var cancellationToken = context.CancellationToken;

        await floodReportService.CreateFromFloodReportCreated(message, cancellationToken).ConfigureAwait(false);
    }
}

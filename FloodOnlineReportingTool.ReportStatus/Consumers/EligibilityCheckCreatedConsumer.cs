using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using MassTransit;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(
    ILogger<EligibilityCheckCreatedConsumer> logger,
    IReportService reportService,
    IReportDuplicateService reportDuplicateService
) : IConsumer<EligibilityCheckCreated>
{
    public async Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        var message = context.Message;

        logger.LogInformation("Consuming {Contract} with message {Message}", nameof(EligibilityCheckCreated), message);

        if (IsUninhabitable(message))
        {
            // Send a danger alert message
            // TODO Create a danger alert message and publish it
            logger.LogInformation("Area is uninhabitable");
            return;
        }

        var reportId = await reportService.GetReport(message, context.CancellationToken);
        if (reportId != null)
        {
            logger.LogInformation("Creating a duplicate report for report ID {ReportId} and publishing a message", reportId);
            await reportDuplicateService.CreateDuplicate(reportId.Value, message, context.CancellationToken);
            return;
        }

        logger.LogInformation("Creating a report and publishing a message");
        await reportService.CreateReport(message, context.CancellationToken);
    }

    private bool IsUninhabitable(EligibilityCheckCreated message)
    {
        // TODO Implement the logic to determine if the area is uninhabitable
        throw new NotImplementedException();
    }
}

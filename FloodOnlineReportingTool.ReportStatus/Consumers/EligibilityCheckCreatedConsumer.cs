using FloodOnlineReportingTool.Contracts;
using MassTransit;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(ILogger<EligibilityCheckCreatedConsumer> logger) : IConsumer<EligibilityCheckCreated>
{
    public Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        logger.LogInformation("Consuming {Contract} with message {Message}", nameof(EligibilityCheckCreated), context.Message);
        throw new NotImplementedException();
    }
}

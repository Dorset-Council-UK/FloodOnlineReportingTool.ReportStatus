using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using System.Text.Json;

namespace Messaging.Consumers;

public sealed class FloodSourceCreatedConsumer(
    ILogger<FloodSourceCreatedConsumer> logger,
    IServiceScopeFactory scopeFactory)
{

    private readonly IServiceScopeFactory scopeFactory = scopeFactory;
    private readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Web;

    public async Task HandleMessage(ProcessMessageEventArgs args)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(FloodReportSourceCreated));

        try
        {
            var message = JsonSerializer.Deserialize<FloodReportSourceCreated>(args.Message.Body, _jsonOptions);
            if (message is null)
            {
                logger.LogError("Reading inbox message {MessageId} failed", args.Message.MessageId);
                await args.DeadLetterMessageAsync(args.Message, "Deserialization returned null", cancellationToken: args.CancellationToken);
                return;
            }

            // InboxWorker / BackgroundService is a singleton, so we have to create a scope to our service
            await using var scope = scopeFactory.CreateAsyncScope();
            IFloodReportService floodReportService = scope.ServiceProvider.GetRequiredService<IFloodReportService>();
            await floodReportService.CreateFromFloodReportCreated(message, args.CancellationToken);

            await args.CompleteMessageAsync(args.Message, args.CancellationToken);

            logger.LogInformation("Processed inbox message {MessageId}", args.Message.MessageId);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Deserialization error for message {MessageId}", args.Message.MessageId);
            await args.DeadLetterMessageAsync(args.Message, ex.Message, ex.InnerException?.Message ?? "", cancellationToken: args.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", args.Message.MessageId);
            await args.DeadLetterMessageAsync(args.Message, ex.Message, ex.InnerException?.Message ?? "", cancellationToken: args.CancellationToken);
        }
    }
}

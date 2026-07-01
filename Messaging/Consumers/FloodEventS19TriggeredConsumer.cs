using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using System.Text.Json;

namespace Messaging.Consumers;

public sealed class FloodEventS19TriggeredConsumer(
    ILogger<FloodEventS19TriggeredConsumer> logger,
    IServiceScopeFactory scopeFactory)
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Web;

    public async Task HandleMessage(ProcessMessageEventArgs args)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(FloodEventS19Triggered));

        try
        {
            var message = JsonSerializer.Deserialize<FloodEventS19Triggered>(args.Message.Body, _jsonOptions);
            if (message is null)
            {
                logger.LogError("Reading inbox message {MessageId} failed", args.Message.MessageId);
                await args.DeadLetterMessageAsync(args.Message, "Deserialization returned null", cancellationToken: args.CancellationToken);
                return;
            }

            // InboxWorker / BackgroundService is a singleton, so we have to create a scope to our service
            await using var scope = _scopeFactory.CreateAsyncScope();
            // TODO: Implement this

            //await args.CompleteMessageAsync(args.Message, args.CancellationToken); // Don't complete yet until we implement the processing logic

            logger.LogInformation("Processed inbox message {MessageId}", args.Message.MessageId);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Deserialization error for message {MessageId}", args.Message.MessageId);
            // Decided to just dead-letter but we may want to consider doing additional steps here. 
            // Could indicate a bad integration from a third party or something similar so we might want to 
            // notify a user. 
            await args.DeadLetterMessageAsync(args.Message, ex.Message, ex.InnerException?.Message ?? "", cancellationToken: args.CancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", args.Message.MessageId);
            // Dead-letter the message here
            await args.DeadLetterMessageAsync(args.Message, ex.Message, ex.InnerException?.Message ?? "", cancellationToken: args.CancellationToken);
        }
    }
}

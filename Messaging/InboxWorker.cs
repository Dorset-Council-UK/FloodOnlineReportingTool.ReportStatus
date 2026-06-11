using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using System.Diagnostics;
using System.Text.Json;

namespace Inbox;

public sealed class InboxWorker(
    ILogger<InboxWorker> logger,
    IConfiguration configuration,
    IServiceProvider serviceProvider,
    ServiceBusClient serviceBusClient
) : BackgroundService
{
    public const string ActivitySourceName = "InboxWorker";
    private static readonly ActivitySource _activitySource = new(ActivitySourceName);
    private readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Web;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Inbox worker started...");

        string topicName = GetTopicName(nameof(FloodReportSourceCreated));
        string subscriptionName = configuration["Inbox:SubscriptionName"]
            ?? throw new InvalidOperationException("Missing configuration setting: 'Inbox:SubscriptionName' is required.");

        ServiceBusProcessorOptions options = new()
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 2
        };

        await using ServiceBusProcessor processor = serviceBusClient.CreateProcessor(topicName, subscriptionName, options);
        processor.ProcessMessageAsync += HandleMessageAsync;
        processor.ProcessErrorAsync += HandleErrorAsync;

        await processor.StartProcessingAsync(cancellationToken);

        try
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // expected on shutdown
        }
        finally
        {
            await processor.StopProcessingAsync(CancellationToken.None);
            processor.ProcessMessageAsync -= HandleMessageAsync;
            processor.ProcessErrorAsync -= HandleErrorAsync;
        }
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        using var activity = _activitySource.StartActivity("Processing inbox message", ActivityKind.Consumer);

        logger.LogInformation("Consuming contract: {Contract}", nameof(FloodReportSourceCreated));

        try
        {
            var message = JsonSerializer.Deserialize<FloodReportSourceCreated>(args.Message.Body, _jsonOptions);
            if (message is null) {
                logger.LogError("Reading inbox message {MessageId} failed", args.Message.MessageId);
                return;
            }
            await using var scope = serviceProvider.CreateAsyncScope();
            var floodReportService = scope.ServiceProvider.GetRequiredService<IFloodReportService>();
            await floodReportService.CreateFromFloodReportCreated(message, args.CancellationToken);

            await args.CompleteMessageAsync(args.Message, args.CancellationToken);

            logger.LogInformation("Processed inbox message {MessageId}", args.Message.MessageId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing message {MessageId}", args.Message.MessageId);
            // Consider whether to abandon or dead-letter the message here depending on the exception type and retry strategy
             //await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
        }
        
    }

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, "Inbox worker error on {EntityPath}: {ErrorSource}", args.EntityPath, args.ErrorSource);
        // Should this be dead lettering?
        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the topic name for the specified message type.
    /// </summary>
    /// <remarks>Optional suffix can be set via configuration (Inbox:TopicSuffix) and will be appended to the topic name. Useful for testing.</remarks>
    /// <exception cref="NotSupportedException">Thrown if the message type is not supported.</exception>
    private string GetTopicName(string messageType)
    {
        var topicName = messageType switch
        {
            nameof(FloodReportSourceCreated) => TopicNames.FloodReportSourceCreated,
            _ => throw new NotSupportedException($"Message type '{messageType}' is not supported."),
        };

        string? suffix = configuration["Inbox:TopicSuffix"];
        return string.IsNullOrWhiteSpace(suffix) ? topicName : $"{topicName}{suffix}";
    }
}

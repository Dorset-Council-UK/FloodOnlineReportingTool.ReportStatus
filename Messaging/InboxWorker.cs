using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using System.Diagnostics;
using System.Text.Json;

namespace Messaging;

public sealed class InboxWorker(
    ILogger<InboxWorker> logger,
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ServiceBusClient serviceBusClient
) : BackgroundService
{
    public const string ActivitySourceName = "InboxWorker";
    private readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Web;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
    var processors = new List<ServiceBusProcessor>();
        var handlers = new Dictionary<ServiceBusProcessor, Func<ProcessMessageEventArgs, Task>>();

        string subscriptionName = configuration["Inbox:SubscriptionName"]
            ?? throw new InvalidOperationException("Missing configuration setting: 'Inbox:SubscriptionName' is required.");

        try
        {
            var serviceBusOptions = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 2,
            };
            // Start all processors FIRST
            string reportSourceTopic = GetTopicName(nameof(FloodReportSourceCreated));
            string investigationTriggerTopic = GetTopicName(nameof(InvestigationTriggered));

            var reportStatustProcessor = serviceBusClient.CreateProcessor(reportSourceTopic, subscriptionName,
                    options: serviceBusOptions);
            reportStatustProcessor.ProcessMessageAsync += ProcessFloodReportSourceCreated;
            reportStatustProcessor.ProcessErrorAsync += HandleErrorAsync;
            await reportStatustProcessor.StartProcessingAsync(cancellationToken);
            processors.Add(reportStatustProcessor);
            logger.LogInformation("Started processor for topic: {Topic}", reportSourceTopic);

            var investigationTriggerProcessor = serviceBusClient.CreateProcessor(investigationTriggerTopic, subscriptionName,
                    options: serviceBusOptions);
            investigationTriggerProcessor.ProcessMessageAsync += ProcessInvestigationTriggered;
            investigationTriggerProcessor.ProcessErrorAsync += HandleErrorAsync;
            await investigationTriggerProcessor.StartProcessingAsync(cancellationToken);
            processors.Add(investigationTriggerProcessor);
            logger.LogInformation("Started processor for topic: {Topic}", investigationTriggerTopic);

            logger.LogInformation("All processors started, waiting for cancellation...");

            // Wait for cancellation
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Expected on shutdown
        }
        finally
        {
            // Stop all processors
            foreach (var processor in processors)
            {
                await processor.StopProcessingAsync(CancellationToken.None);
                if (handlers.TryGetValue(processor, out var handler))
                {
                    processor.ProcessMessageAsync -= handler;
                }

                processor.ProcessErrorAsync -= HandleErrorAsync;
                await processor.DisposeAsync();
            }

            logger.LogInformation("Service bus processors stopped");
        }
    }

    private async Task ProcessFloodReportSourceCreated(ProcessMessageEventArgs args)
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

    private async Task ProcessInvestigationTriggered(ProcessMessageEventArgs args)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(InvestigationTriggered));

        try
        {
            var message = JsonSerializer.Deserialize<InvestigationTriggered>(args.Message.Body, _jsonOptions);
            if (message is null)
            {
                logger.LogError("Reading inbox message {MessageId} failed", args.Message.MessageId);
                await args.DeadLetterMessageAsync(args.Message, "Deserialization returned null", cancellationToken: args.CancellationToken);
                return;
            }

            // InboxWorker / BackgroundService is a singleton, so we have to create a scope to our service
            await using var scope = scopeFactory.CreateAsyncScope();
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

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, "Inbox worker error on Entity path: {EntityPath} Source: {ErrorSource}", args.EntityPath, args.ErrorSource);
        // Should this be dead lettering?
        return Task.CompletedTask;
    }

    private string GetTopicName(string messageType)
    {
        var topicName = messageType switch
        {
            nameof(FloodReportSourceCreated) => TopicNames.FloodReportSourceCreated,
            nameof(InvestigationTriggered) => TopicNames.InvestigationTriggered,
            _ => throw new NotSupportedException($"Message type {messageType} is not supported."),
        };

        string? suffix = configuration["Messaging:TopicSuffix"];
        return string.IsNullOrWhiteSpace(suffix) ? topicName : $"{topicName}{suffix}";
    }
}

using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using Messaging.Consumers;
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
            
            var reportStatusProcessor = serviceBusClient.CreateProcessor(
                reportSourceTopic, 
                subscriptionName,
                options: serviceBusOptions);
            reportStatusProcessor.ProcessMessageAsync += ProcessFloodReportSourceCreated;
            reportStatusProcessor.ProcessErrorAsync += HandleErrorAsync;
            await reportStatusProcessor.StartProcessingAsync(cancellationToken);
            processors.Add(reportStatusProcessor);
            logger.LogInformation("Started processor for topic: {Topic}", reportSourceTopic);

            string investigationTriggerTopic = GetTopicName(nameof(InvestigationTriggered));
            var investigationTriggerProcessor = serviceBusClient.CreateProcessor(
                investigationTriggerTopic,
                subscriptionName,
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
        await using var scope = scopeFactory.CreateAsyncScope();
        var consumer = scope.ServiceProvider.GetRequiredService<FloodSourceCreatedConsumer>();
        await consumer.HandleFloodSourceCreated(args);
    }

    private async Task ProcessInvestigationTriggered(ProcessMessageEventArgs args)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var consumer = scope.ServiceProvider.GetRequiredService<InvestigationTriggeredConsumer>();
        await consumer.HandleInvestigationTriggered(args);
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

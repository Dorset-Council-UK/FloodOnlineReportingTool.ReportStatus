using System.Reflection;
using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using Tests.Data;

namespace Tests.Integration;

/// <summary>
/// Real service bus integration tests.
/// These tests publish actual messages to Azure Service Bus and are intended as on-demand
/// smoke tests to verify topic/subscription topology after infrastructure changes.
///
/// They are skipped by default.
/// Add a connection string named "service-bus" in user secrets to run them.
/// </summary>
public class ServiceBusPublishTests
{
    private static readonly (string? SkipReason, string? ConnectionString) _settings = ReadSettings();

    private static (string? SkipReason, string? ConnectionString) ReadSettings()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        string? connectionString = config.GetConnectionString("service-bus");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return ("No connection string named 'service-bus'.", null);
        }

        if (!connectionString.Contains("Endpoint=", StringComparison.OrdinalIgnoreCase))
        {
            return ("Service Bus integration tests currently require a full Service Bus connection string (with Endpoint=).", null);
        }

        return (null, connectionString);
    }

    private static async Task PublishToTopicAsync<T>(T payload, string topicName, CancellationToken cancellationToken)
    {
        await using var client = new ServiceBusClient(_settings.ConnectionString!);
        await using var sender = client.CreateSender(topicName);

        var message = new ServiceBusMessage(BinaryData.FromObjectAsJson(payload))
        {
            ContentType = MediaTypeNames.Application.Json,
            Subject = typeof(T).Name,
            MessageId = Guid.CreateVersion7().ToString(),
        };

        await sender.SendMessageAsync(message, cancellationToken);
    }

    /// <summary>
    /// Publishes a real <see cref="FloodReportSourceCreated"/> message to Azure Service Bus.
    /// Use this to verify the topic and subscription topology is correct after infrastructure changes.
    /// The receiving system (fort-report-status subscription) should pick this up and process it.
    /// </summary>
    [Fact]
    public async Task Publish_FloodReportSourceCreated_ToRealServiceBus()
    {
        var (skipReason, _) = _settings;
        if (skipReason is not null)
        {
            Assert.Skip(skipReason);
        }

        await PublishToTopicAsync(
            FloodReportSourceCreatedData.Property1,
            TopicNames.FloodReportSourceCreated,
            TestContext.Current.CancellationToken);
    }

    /// <summary>
    /// Publishes a minimal <see cref="FloodReportCreated"/> message directly to Azure Service Bus.
    /// Use this to verify the FloodReportCreated topic is reachable and any downstream subscribers
    /// (e.g. the third system) can receive it — without needing to trigger the full consumer pipeline.
    /// </summary>
    [Fact]
    public async Task Publish_FloodReportCreated_ToRealServiceBus()
    {
        var (skipReason, _) = _settings;
        if (skipReason is not null)
        {
            Assert.Skip(skipReason);
        }

        var message = new FloodReportCreated(
            Id: Guid.CreateVersion7(),
            Reference: "TEST-INTG-01",
            CreatedUtc: DateTimeOffset.UtcNow,
            Easting: 368991.12,
            Northing: 90881.94,
            HasContacts: false,
            ContactRecordTypes: []);

        await PublishToTopicAsync(
            message,
            TopicNames.FloodReportCreated,
            TestContext.Current.CancellationToken);
    }
}

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
/// They are skipped by default.
/// Add a connection string named "ServiceBus" in user secrets to run them.
/// Also add a "TopicSuffix" if you are creating personal topics for testing (e.g. "-aaron") to avoid interfering with other environments.
/// </summary>
public class ServiceBusPublishTests
{
    private static readonly (string? SkipReason, (string? ConnectionString, string? TopicSuffix) Values) _settings = ReadSettings();

    private static (string? SkipReason, (string? ConnectionString, string? TopicSuffix)) ReadSettings()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        string? connectionString = config.GetValue<string>("ServiceBus");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return ("No connection string named 'ServiceBus'.", (null, null));
        }

        if (!connectionString.Contains("Endpoint=", StringComparison.OrdinalIgnoreCase))
        {
            return ("Service Bus integration tests currently require a full Service Bus connection string (with Endpoint=).", (null, null));
        }

        string? topicSuffix = config.GetValue<string>("TopicSuffix")?.Trim();
        if (topicSuffix is null)
        {
            return ("No topic suffix configured. Add a 'TopicSuffix' value in user secrets.", (null, null));
        }

        string? runServiceBusTests = config.GetValue<string>("RunServiceBusTests");
        if (!string.Equals(runServiceBusTests, "true", StringComparison.OrdinalIgnoreCase))
        {
            return ("RunServiceBusTests is not set to 'true'.", (null, null));
        }


        return (null, (connectionString, topicSuffix));
    }

    private static async Task PublishToTopicAsync<T>(T payload, string topicName, CancellationToken cancellationToken)
    {
        await using var client = new ServiceBusClient(_settings.Values.ConnectionString!);

        string? suffix = _settings.Values.TopicSuffix;
        string modifiedTopicName = string.IsNullOrWhiteSpace(suffix) ? topicName : $"{topicName}{suffix}";
        await using var sender = client.CreateSender(modifiedTopicName);

        var message = new ServiceBusMessage(BinaryData.FromObjectAsJson(payload))
        {
            ContentType = MediaTypeNames.Application.Json,
            Subject = modifiedTopicName,
            MessageId = Guid.CreateVersion7().ToString(),
        };

        await sender.SendMessageAsync(message, cancellationToken);
    }

    /// <summary>
    /// Publishes a real <see cref="FloodReportSourceCreated"/> message to Azure Service Bus.
    /// Use this to verify the topic and subscription topology is correct after infrastructure changes.
    /// The receiving system (fort-status subscription) should pick this up and process it.
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
            Reference: "TEST-INTG-01",
            Easting: 368991.12,
            Northing: 90881.94,
            HasContacts: false,
            ContactRecordTypes: []);

        await PublishToTopicAsync(
            message,
            TopicNames.FloodReportCreated,
            TestContext.Current.CancellationToken);
    }

    /// <summary>
    /// Publishes a real <see cref="FloodReportSourceVerifyContact"/> message to Azure Service Bus.
    /// Use this to verify the topic and subscription topology is correct after infrastructure changes.
    /// The receiving system (fort-public subscription) should pick this up and process it.
    /// </summary>
    [Fact]
    public async Task Publish_FloodReportSourceVerifyContact_ToRealServiceBus()
    {
        var (skipReason, _) = _settings;
        if (skipReason is not null)
        {
            Assert.Skip(skipReason);
        }

        await PublishToTopicAsync(
            FloodReportSourceCreatedData.Verify1,
            TopicNames.FloodReportSourceVerifyContact,
            TestContext.Current.CancellationToken);
    }
}

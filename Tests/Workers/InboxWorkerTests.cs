using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using Messaging;
using Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Reflection;

namespace Tests.Workers;

public class InboxWorkerTests
{
    private static IConfiguration CreateConfiguration(string? subscriptionName = "test-subscription", string? topicSuffix = "")
    {
        var settings = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            ["Inbox:SubscriptionName"] = subscriptionName,
        };

        if (!string.IsNullOrWhiteSpace(topicSuffix))
        {
            settings["Messaging:TopicSuffix"] = topicSuffix;
        }

        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }

    private static ServiceBusClient CreateServiceBusClient()
    {
        return new ServiceBusClient("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsInvalidOperationException_WhenSubscriptionNameMissing()
    {
        var logger = Substitute.For<ILogger<InboxWorker>>();
        var configuration = CreateConfiguration(subscriptionName: null);
        IEnumerable<IInboxConsumer> consumers = Substitute.For<IEnumerable<IInboxConsumer>>();
        await using var serviceBusClient = CreateServiceBusClient();

        var worker = new InboxWorker(logger, configuration, consumers, serviceBusClient);
        var executeAsyncMethod = typeof(InboxWorker).GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("ExecuteAsync method not found");

        var executeTask = executeAsyncMethod.Invoke(worker, [TestContext.Current.CancellationToken]) as Task
            ?? throw new InvalidOperationException("ExecuteAsync invocation returned null");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await executeTask);

        Assert.Equal("Missing configuration setting: 'Inbox:SubscriptionName' is required.", exception.Message);
    }

    [Fact]
    public async Task GetTopicName_AppliesTopicSuffix_WhenTopicSuffixConfigured()
    {
        var logger = Substitute.For<ILogger<InboxWorker>>();
        var configuration = CreateConfiguration(topicSuffix: "-test");
        IEnumerable<IInboxConsumer> consumers = Substitute.For<IEnumerable<IInboxConsumer>>();
        await using var serviceBusClient = CreateServiceBusClient();

        var worker = new InboxWorker(logger, configuration, consumers, serviceBusClient);
        var getTopicNameMethod = typeof(InboxWorker).GetMethod("GetTopicName", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("GetTopicName method not found");

        var topicName = getTopicNameMethod.Invoke(worker, [nameof(FloodReportSourceCreated)]) as string;

        Assert.NotNull(topicName);
        Assert.EndsWith("-test", topicName, StringComparison.Ordinal);
        Assert.Contains(TopicNames.FloodReportSourceCreated, topicName, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetTopicName_ThrowsNotSupportedException_WhenMessageTypeUnknown()
    {
        var logger = Substitute.For<ILogger<InboxWorker>>();
        var configuration = CreateConfiguration();
        IEnumerable<IInboxConsumer> consumers = Substitute.For<IEnumerable<IInboxConsumer>>();
        await using var serviceBusClient = CreateServiceBusClient();

        var worker = new InboxWorker(logger, configuration, consumers, serviceBusClient);
        var getTopicNameMethod = typeof(InboxWorker).GetMethod("GetTopicName", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("GetTopicName method not found");

        var exception = Assert.Throws<TargetInvocationException>(() =>
            getTopicNameMethod.Invoke(worker, ["UnknownMessageType"]));

        Assert.IsType<NotSupportedException>(exception.InnerException);
        Assert.Contains("UnknownMessageType", exception.InnerException?.Message ?? string.Empty, StringComparison.Ordinal);
    }
}

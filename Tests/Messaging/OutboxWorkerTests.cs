using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Reflection;

namespace Tests.Messaging;

public class OutboxWorkerTests
{
    private static IConfiguration CreateConfiguration(string? topicSuffix = "")
    {
        var settings = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        if (!string.IsNullOrWhiteSpace(topicSuffix))
        {
            settings["Messaging:TopicSuffix"] = topicSuffix;
        }

        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }

    private static IHostEnvironment CreateHostEnvironment(string environmentName = "Testing")
    {
        var hostEnvironment = Substitute.For<IHostEnvironment>();
        hostEnvironment.EnvironmentName.Returns(environmentName);
        return hostEnvironment;
    }

    private static OutboxWorker CreateWorker(IConfiguration? configuration = null)
    {
        return new OutboxWorker(
            Substitute.For<ILogger<OutboxWorker>>(),
            configuration ?? CreateConfiguration(),
            Substitute.For<IServiceProvider>(),
            CreateHostEnvironment(),
            new ServiceBusClient("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="));
    }

    [Fact]
    public void GetTopicName_ReturnsExpectedTopic_WhenMessageTypeSupported()
    {
        var worker = CreateWorker();
        var getTopicNameMethod = typeof(OutboxWorker).GetMethod("GetTopicName", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("GetTopicName method not found");

        var topicName = getTopicNameMethod.Invoke(worker, [nameof(FloodReportCreated)]) as string;

        Assert.Equal(TopicNames.FloodReportCreated, topicName);
    }

    [Fact]
    public void GetTopicName_AppliesTopicSuffix_WhenConfigured()
    {
        var worker = CreateWorker(CreateConfiguration(topicSuffix: "-test"));
        var getTopicNameMethod = typeof(OutboxWorker).GetMethod("GetTopicName", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("GetTopicName method not found");

        var topicName = getTopicNameMethod.Invoke(worker, [nameof(FloodReportCreated)]) as string;

        Assert.NotNull(topicName);
        Assert.EndsWith("-test", topicName, StringComparison.Ordinal);
        Assert.Contains(TopicNames.FloodReportCreated, topicName, StringComparison.Ordinal);
    }

    [Fact]
    public void GetTopicName_ThrowsNotSupportedException_WhenMessageTypeUnknown()
    {
        var worker = CreateWorker();
        var getTopicNameMethod = typeof(OutboxWorker).GetMethod("GetTopicName", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("GetTopicName method not found");

        var exception = Assert.Throws<TargetInvocationException>(() =>
            getTopicNameMethod.Invoke(worker, ["UnknownMessageType"]));

        Assert.IsType<NotSupportedException>(exception.InnerException);
        Assert.Contains("UnknownMessageType", exception.InnerException?.Message ?? string.Empty, StringComparison.Ordinal);
    }
}
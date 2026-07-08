using Azure.Messaging.ServiceBus;
using Messaging;
using Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Reflection;

namespace Tests.Messaging;

public class InboxWorkerTests
{
    private static ServiceBusClient CreateServiceBusClient()
    {
        return new ServiceBusClient("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");
    }

    [Fact]
    public async Task ExecuteAsync_ThrowsInvalidOperationException_WhenSubscriptionNameMissing()
    {
        var logger = Substitute.For<ILogger<InboxWorker>>();
        var configuration = Substitute.For<IConfiguration>();
        configuration["Inbox:SubscriptionName"].Returns((string?)null);
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

    //[Fact]
    //public async Task ExecuteAsync_Throws_WhenSubscriptionNameMissing()
    //{
    //    var logger = Substitute.For<ILogger<InboxWorker>>();
    //    var configuration = Substitute.For<IConfiguration>();
    //    configuration["Inbox:SubscriptionName"].Returns((string?)null);
    //    var consumers = Substitute.For<IEnumerable<IInboxConsumer>>();
    //    await using var serviceBusClient = CreateServiceBusClient();

    //    var inboxWorker = new InboxWorker(logger, configuration, consumers, serviceBusClient);
    //}
}

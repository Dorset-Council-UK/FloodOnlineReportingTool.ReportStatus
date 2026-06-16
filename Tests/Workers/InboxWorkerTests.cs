using Azure.Messaging.ServiceBus;
using Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Reflection;

namespace Tests.Workers;

public class InboxWorkerTests
{
    [Fact]
    public async Task ExecuteAsync_ThrowsInvalidOperationException_WhenSubscriptionNameMissing()
    {
        var logger = Substitute.For<ILogger<InboxWorker>>();
        var configuration = new ConfigurationBuilder().AddInMemoryCollection([]).Build();
        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        await using var serviceBusClient = new ServiceBusClient("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");

        var worker = new InboxWorker(logger, configuration, serviceScopeFactory, serviceBusClient);
        var executeAsyncMethod = typeof(InboxWorker).GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        ArgumentNullException.ThrowIfNull(executeAsyncMethod);

        var executeTask = executeAsyncMethod.Invoke(worker, [TestContext.Current.CancellationToken]) as Task;
        ArgumentNullException.ThrowIfNull(executeTask);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await executeTask);

        Assert.Equal("Missing configuration setting: 'Inbox:SubscriptionName' is required.", exception.Message);
    }
}

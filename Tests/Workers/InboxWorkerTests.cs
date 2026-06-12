using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Messaging;
using Microsoft.Extensions.Configuration;
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
        var floodReportService = Substitute.For<IFloodReportService>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        await using var serviceBusClient = new ServiceBusClient("Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");

        var worker = new InboxWorker(logger, configuration, floodReportService, serviceProvider, serviceBusClient);
        var executeAsyncMethod = typeof(InboxWorker).GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        ArgumentNullException.ThrowIfNull(executeAsyncMethod);

        var executeTask = executeAsyncMethod.Invoke(worker, [TestContext.Current.CancellationToken]) as Task;
        ArgumentNullException.ThrowIfNull(executeTask);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await executeTask);

        Assert.Equal("Missing configuration setting: 'Inbox:SubscriptionName' is required.", exception.Message);
    }
}

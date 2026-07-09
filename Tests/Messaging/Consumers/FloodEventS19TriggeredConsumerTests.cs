using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace Tests.Messaging.Consumers;

public class FloodEventS19TriggeredConsumerTests
{
    private readonly ILogger<FloodEventS19TriggeredConsumer> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FloodEventS19TriggeredConsumerTests()
    {
        _logger = Substitute.For<ILogger<FloodEventS19TriggeredConsumer>>();
        _configuration = Substitute.For<IConfiguration>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
    }

    private void SetupServiceScopeFactory(bool throwError = false)
    {
        var floodEventService = Substitute.For<IFloodEventService>();
        floodEventService
            .TriggerFromFloodEventS19Triggered(Arg.Any<FloodEventS19Triggered>(), TestContext.Current.CancellationToken)
            .Returns(Task.CompletedTask);

        var serviceProvider = Substitute.For<IServiceProvider>();
        if (throwError)
        {
            serviceProvider
                .GetService(typeof(IFloodEventService))
                .Returns(_ => throw new InvalidOperationException("No service for type 'IFloodEventService' has been registered."));
        }
        else
        {
            serviceProvider
                .GetService(typeof(IFloodEventService))
                .Returns(floodEventService);
        }

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);
        _serviceScopeFactory.CreateScope().Returns(scope);
    }

    [Fact]
    public Task ContractName_Returns_FloodEventS19TriggeredContract_Name()
    {
        // Arrange
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        // Act
        var contractName = consumer.ContractName;

        // Assert
        Assert.Equal(nameof(FloodEventS19Triggered), contractName);
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(null, TopicNames.FloodEventS19Triggered)]
    [InlineData("", TopicNames.FloodEventS19Triggered)]
    [InlineData("  ", TopicNames.FloodEventS19Triggered)]
    [InlineData("-testing", $"{TopicNames.FloodEventS19Triggered}-testing")]
    [InlineData("  -testing", $"{TopicNames.FloodEventS19Triggered}-testing")]
    [InlineData("-testing  ", $"{TopicNames.FloodEventS19Triggered}-testing")]
    public Task TopicName_Returns_ExpectedTopicName_ForConfiguredSuffix(string? suffix, string expectedTopicName)
    {
        // Arrange
        _configuration["Messaging:TopicSuffix"] = suffix;
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        // Act
        var topicName = consumer.TopicName;

        // Assert
        Assert.Equal(expectedTopicName, topicName);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task HandleMessage_DeadLetter_WhenJsonDeserializationNull()
    {
        // Arrange
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromString("null"));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.DeadLetterMessageAsync(default, default, default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).DeadLetterMessageAsync(message, "Message could not be read", Arg.Any<string>(), TestContext.Current.CancellationToken);
        await args.DidNotReceiveWithAnyArgs().CompleteMessageAsync(default, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task HandleMessage_DeadLetter_WhenJsonDeserializationThrows()
    {
        // Arrange
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: null);
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.DeadLetterMessageAsync(default, default, default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).DeadLetterMessageAsync(message, "JSON exception", Arg.Any<string>(), TestContext.Current.CancellationToken);
        await args.DidNotReceiveWithAnyArgs().CompleteMessageAsync(default, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task HandleMessage_DeadLetter_WhenHandlerThrows()
    {
        // Arrange
        SetupServiceScopeFactory(throwError: true);
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodEventS19Triggered = new FloodEventS19Triggered(
            Reference: "REF-123",
            TriggeredUtc: DateTimeOffset.UtcNow
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodEventS19Triggered, JsonSerializerOptions.Web));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.DeadLetterMessageAsync(default, default, default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).DeadLetterMessageAsync(message, "Unexpected exception", "No service for type 'IFloodEventService' has been registered.", TestContext.Current.CancellationToken);
        await args.DidNotReceiveWithAnyArgs().CompleteMessageAsync(default, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task HandleMessage_CompleteMessage_WhenPayloadValid()
    {
        // Arrange
        SetupServiceScopeFactory();
        var consumer = new FloodEventS19TriggeredConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodEventS19Triggered = new FloodEventS19Triggered(
            Reference: "REF-123",
            TriggeredUtc: DateTimeOffset.UtcNow
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodEventS19Triggered, JsonSerializerOptions.Web));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).CompleteMessageAsync(message, TestContext.Current.CancellationToken);
    }
}

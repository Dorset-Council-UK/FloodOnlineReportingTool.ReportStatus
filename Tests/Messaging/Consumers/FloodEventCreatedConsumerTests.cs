using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.Database.Services;
using Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace Tests.Messaging.Consumers;

public class FloodEventCreatedConsumerTests
{
    private readonly ILogger<FloodEventCreatedConsumer> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FloodEventCreatedConsumerTests()
    {
        _logger = Substitute.For<ILogger<FloodEventCreatedConsumer>>();
        _configuration = Substitute.For<IConfiguration>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
    }

    private void SetupServiceScopeFactory(bool throwError = false)
    {
        var floodEventService = Substitute.For<IFloodEventService>();
        floodEventService
            .CreateFromFloodEventCreated(Arg.Any<FloodEventCreated>(), TestContext.Current.CancellationToken)
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
    public Task ContractName_Returns_FloodEventCreatedContract_Name()
    {
        // Arrange
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        // Act
        var contractName = consumer.ContractName;

        // Assert
        Assert.Equal(nameof(FloodEventCreated), contractName);
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(null, TopicNames.FloodEventCreated)]
    [InlineData("", TopicNames.FloodEventCreated)]
    [InlineData("  ", TopicNames.FloodEventCreated)]
    [InlineData("-testing", $"{TopicNames.FloodEventCreated}-testing")]
    [InlineData("  -testing", $"{TopicNames.FloodEventCreated}-testing")]
    [InlineData("-testing  ", $"{TopicNames.FloodEventCreated}-testing")]
    public Task TopicName_Returns_ExpectedTopicName_ForConfiguredSuffix(string? suffix, string expectedTopicName)
    {
        // Arrange
        _configuration["Messaging:TopicSuffix"] = suffix;
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodEventCreated = new FloodEventCreated(
            Reference: "REF-123",
            Title: "TEST Flood Event",
            RecordStatus: Guid.NewGuid(),
            FloodStatus: Guid.NewGuid(),
            InvestigationStatus: Guid.NewGuid(),
            FloodStartDate: DateTimeOffset.UtcNow.AddDays(-1),
            FloodEndDate: DateTimeOffset.UtcNow,
            FloodCauses: [],
            Organisations: [],
            StatusFilter: [],
            LinkedFloodReports: []
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodEventCreated, JsonSerializerOptions.Web));
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
        var consumer = new FloodEventCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodEventCreated = new FloodEventCreated(
            Reference: "REF-123",
            Title: "TEST Flood Event",
            RecordStatus: Guid.NewGuid(),
            FloodStatus: Guid.NewGuid(),
            InvestigationStatus: Guid.NewGuid(),
            FloodStartDate: DateTimeOffset.UtcNow.AddDays(-1),
            FloodEndDate: DateTimeOffset.UtcNow,
            FloodCauses: [],
            Organisations: [],
            StatusFilter: [],
            LinkedFloodReports: []
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodEventCreated, JsonSerializerOptions.Web));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).CompleteMessageAsync(message, TestContext.Current.CancellationToken);
    }
}

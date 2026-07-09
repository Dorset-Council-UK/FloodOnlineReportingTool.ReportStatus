using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Messaging.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace Tests.Messaging.Consumers;

public class FloodSourceCreatedConsumerTests
{
    private readonly ILogger<FloodSourceCreatedConsumer> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FloodSourceCreatedConsumerTests()
    {
        _logger = Substitute.For<ILogger<FloodSourceCreatedConsumer>>();
        _configuration = Substitute.For<IConfiguration>();
        _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
    }

    private void SetupServiceScopeFactory(bool throwError = false)
    {
        var floodEventService = Substitute.For<IFloodReportService>();
        floodEventService
            .CreateFromFloodReportCreated(Arg.Any<FloodReportSourceCreated>(), TestContext.Current.CancellationToken)
            .Returns(Task.CompletedTask);

        var serviceProvider = Substitute.For<IServiceProvider>();
        if (throwError)
        {
            serviceProvider
                .GetService(typeof(IFloodReportService))
                .Returns(_ => throw new InvalidOperationException("No service for type 'IFloodReportService' has been registered."));
        }
        else
        {
            serviceProvider
                .GetService(typeof(IFloodReportService))
                .Returns(floodEventService);
        }

        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(serviceProvider);
        _serviceScopeFactory.CreateScope().Returns(scope);
    }

    [Fact]
    public Task ContractName_Returns_FloodReportSourceCreatedContract_Name()
    {
        // Arrange
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        // Act
        var contractName = consumer.ContractName;

        // Assert
        Assert.Equal(nameof(FloodReportSourceCreated), contractName);
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(null, TopicNames.FloodReportSourceCreated)]
    [InlineData("", TopicNames.FloodReportSourceCreated)]
    [InlineData("  ", TopicNames.FloodReportSourceCreated)]
    [InlineData("-testing", $"{TopicNames.FloodReportSourceCreated}-testing")]
    [InlineData("  -testing", $"{TopicNames.FloodReportSourceCreated}-testing")]
    [InlineData("-testing  ", $"{TopicNames.FloodReportSourceCreated}-testing")]
    public Task TopicName_Returns_ExpectedTopicName_ForConfiguredSuffix(string? suffix, string expectedTopicName)
    {
        // Arrange
        _configuration["Messaging:TopicSuffix"] = suffix;
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

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
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodReportSourceCreated = new FloodReportSourceCreated(
            Buffer: 20,
            Reference: "REF-123",
            ViewUri: new Uri("https://test.com/floodreport/REF-123"),
            EligibilityCheckRecord: new EligibilityCheckRecord(
                Id: Guid.NewGuid(),
                Uprn: 10023242411,
                Usrn: null,
                Easting: 368991.11,
                Northing: 90881.11,
                ImpactStartUTC: DateTimeOffset.UtcNow.AddDays(-1),
                ImpactDurationHours: 6,
                IsOnGoing: false,
                IsUninhabitable: false,
                VulnerableCount: 0,
                LocationDescription: "Test location description 1",
                Organisations: [],
                FloodSources: [
                    new(PrimaryCauseIds.River, "River"),
                    new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
                ]
            ),
            HasInvestigation: false,
            HasContacts: false,
            ContactRecordTypes: []
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodReportSourceCreated, JsonSerializerOptions.Web));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.DeadLetterMessageAsync(default, default, default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).DeadLetterMessageAsync(message, "Unexpected exception", "No service for type 'IFloodReportService' has been registered.", TestContext.Current.CancellationToken);
        await args.DidNotReceiveWithAnyArgs().CompleteMessageAsync(default, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task HandleMessage_CompleteMessage_WhenPayloadValid()
    {
        // Arrange
        SetupServiceScopeFactory();
        var consumer = new FloodSourceCreatedConsumer(_logger, _configuration, _serviceScopeFactory);

        var floodReportSourceCreated = new FloodReportSourceCreated(
            Buffer: 20,
            Reference: "REF-123",
            ViewUri: new Uri("https://test.com/floodreport/REF-123"),
            EligibilityCheckRecord: new EligibilityCheckRecord(
                Id: Guid.NewGuid(),
                Uprn: 10023242411,
                Usrn: null,
                Easting: 368991.11,
                Northing: 90881.11,
                ImpactStartUTC: DateTimeOffset.UtcNow.AddDays(-1),
                ImpactDurationHours: 6,
                IsOnGoing: false,
                IsUninhabitable: false,
                VulnerableCount: 0,
                LocationDescription: "Test location description 1",
                Organisations: [],
                FloodSources: [
                    new(PrimaryCauseIds.River, "River"),
                    new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
                ]
            ),
            HasInvestigation: false,
            HasContacts: false,
            ContactRecordTypes: []
        );
        var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(floodReportSourceCreated, JsonSerializerOptions.Web));
        var args = Substitute.For<ProcessMessageEventArgs>(message, Substitute.For<ServiceBusReceiver>(), TestContext.Current.CancellationToken);
        args.CompleteMessageAsync(default, TestContext.Current.CancellationToken).ReturnsForAnyArgs(Task.CompletedTask);

        // Act
        await consumer.HandleMessage(args);

        // Assert
        await args.Received(1).CompleteMessageAsync(message, TestContext.Current.CancellationToken);
    }
}

using System.Reflection;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Tests.Data;

namespace Tests.Integration;

/// <summary>
/// Real service bus integration tests.
/// These tests publish actual messages to Azure Service Bus and are intended as on-demand
/// smoke tests to verify topic/subscription topology after infrastructure changes.
///
/// They are skipped by default.
/// Set "Messaging:RunServiceBusTests" to "true" to run them.
///
/// </summary>
public class ServiceBusPublishTests
{
    // Read once from user secrets — if not set to true, all tests in this class are skipped.
    private static readonly (string? SkipReason, string? ConnectionString) _settings = ReadSettings();

    private static (string? SkipReason, string? ConnectionString) ReadSettings()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        var enabled = config[$"{MessagingSettings.SectionName}:RunServiceBusTests"];
        if (!string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase))
        {
            return ("Set Messaging:RunServiceBusTests=true in user secrets to run real service bus integration tests.", null);
        }

        var connectionString = config[$"{MessagingSettings.SectionName}:{nameof(MessagingSettings.ConnectionString)}"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return ($"No connection string found in user secrets under '{MessagingSettings.SectionName}:{nameof(MessagingSettings.ConnectionString)}'.", null);
        }

        return (null, connectionString);
    }

    private static IBusControl BuildBus(string connectionString)
    {
        return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
        {
            if (connectionString.Contains("Endpoint=", StringComparison.OrdinalIgnoreCase))
            {
                cfg.Host(connectionString);
            }
            else
            {
                // Managed Identity / namespace URI
                cfg.Host(new Uri(connectionString));
            }

            cfg.Message<FloodReportSourceCreated>(m => m.SetEntityName(TopicNames.FloodReportSourceCreated));
            cfg.Message<FloodReportCreated>(m => m.SetEntityName(TopicNames.FloodReportCreated));
            cfg.Message<FloodReportUpdated>(m => m.SetEntityName(TopicNames.FloodReportUpdated));
        });
    }

    /// <summary>
    /// Publishes a real <see cref="FloodReportSourceCreated"/> message to Azure Service Bus.
    /// Use this to verify the topic and subscription topology is correct after infrastructure changes.
    /// The receiving system (fort-report-status subscription) should pick this up and process it.
    /// </summary>
    [Fact]
    public async Task Publish_FloodReportSourceCreated_ToRealServiceBus()
    {
        var (skipReason, connectionString) = _settings;
        if (skipReason is not null)
        {
            Assert.Skip(skipReason);
        }

        var bus = BuildBus(connectionString!);
        await bus.StartAsync(TestContext.Current.CancellationToken);

        try
        {
            await bus.Publish(FloodReportSourceCreatedData.Property1, TestContext.Current.CancellationToken);
        }
        finally
        {
            await bus.StopAsync(TestContext.Current.CancellationToken);
        }
    }

    /// <summary>
    /// Publishes a minimal <see cref="FloodReportCreated"/> message directly to Azure Service Bus.
    /// Use this to verify the FloodReportCreated topic is reachable and any downstream subscribers
    /// (e.g. the third system) can receive it — without needing to trigger the full consumer pipeline.
    /// </summary>
    [Fact]
    public async Task Publish_FloodReportCreated_ToRealServiceBus()
    {
        var (skipReason, connectionString) = _settings;
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
            ContactRecordTypes: []
        );

        var bus = BuildBus(connectionString!);
        await bus.StartAsync(TestContext.Current.CancellationToken);

        try
        {
            await bus.Publish(message, TestContext.Current.CancellationToken);
        }
        finally
        {
            await bus.StopAsync(TestContext.Current.CancellationToken);
        }
    }
}

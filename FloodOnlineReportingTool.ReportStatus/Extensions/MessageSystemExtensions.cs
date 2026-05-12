using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Topics;
using FloodOnlineReportingTool.ReportStatus.Consumers;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class MessageSystemExtensions
{
    /// <summary>
    /// Add the message system.
    /// </summary>
    public static IServiceCollection AddMessageSystem(this IServiceCollection services, MessagingSettings messagingSettings)
    {
        // Default subscription name; can be overridden via MessagingSettings.SubscriptionNameOverride
        // to support isolated environments such as per-developer or per-PR deployments.
        const string defaultFloodReportSourceCreatedSubscription = "floodreport-reportstatus";

        var floodReportSourceCreatedSubscription = messagingSettings.SubscriptionNameOverride is not null
            ? messagingSettings.SubscriptionNameOverride
            : defaultFloodReportSourceCreatedSubscription;

        if (!messagingSettings.Enabled)
        {
            return services;
        }

        services.AddMassTransit(o =>
        {
            o.SetKebabCaseEndpointNameFormatter();

            o.AddConsumer<FloodReportSourceCreatedConsumer>();

            // Transactional Outbox Configuration https://masstransit.io/documentation/configuration/middleware/outbox
            o.AddEntityFrameworkOutbox<ApplicationDbContext>(config =>
            {
                config.UsePostgres();
                config.UseBusOutbox();
            });

            // Using Service Bus Dead-letter Queues https://masstransit.io/documentation/configuration/transports/azure-service-bus#using-service-bus-dead-letter-queues
            o.AddConfigureEndpointsCallback((_, config) =>
            {
                if (config is IServiceBusReceiveEndpointConfigurator sb)
                {
                    sb.ConfigureDeadLetterQueueDeadLetterTransport();
                    sb.ConfigureDeadLetterQueueErrorTransport();
                }
            });

            // Azure Service Bus Configuration https://masstransit.io/documentation/configuration/transports/azure-service-bus
            // Azure Service Bus Transport https://masstransit.io/documentation/transports/azure-service-bus
            o.UsingAzureServiceBus((context, config) =>
            {
                if (messagingSettings.ConnectionString.Contains("Endpoint=", StringComparison.OrdinalIgnoreCase))
                {
                    config.Host(messagingSettings.ConnectionString);
                }
                else
                {
                    // Using Azure Managed Identity https://masstransit.io/documentation/configuration/transports/azure-service-bus#using-azure-managed-identity
                    config.Host(new Uri(messagingSettings.ConnectionString));
                }

                config.Message<FloodReportSourceCreated>(m =>
                {
                    m.SetEntityName(TopicNames.FloodReportSourceCreated);
                });
                config.Message<FloodReportCreated>(m =>
                {
                    m.SetEntityName(TopicNames.FloodReportCreated);
                });
                config.Message<FloodReportUpdated>(m =>
                {
                    m.SetEntityName(TopicNames.FloodReportUpdated);
                });

                // Subscriptions https://masstransit.io/documentation/transports/azure-service-bus#subscriptions
                config.SubscriptionEndpoint<FloodReportSourceCreated>(floodReportSourceCreatedSubscription, e =>
                {
                    e.ConfigureConsumer<FloodReportSourceCreatedConsumer>(context);
                });

                // Finalise all remaining endpoint topology
                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

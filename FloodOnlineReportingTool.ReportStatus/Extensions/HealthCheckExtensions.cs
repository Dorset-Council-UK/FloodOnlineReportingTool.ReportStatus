using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.AspNetCore.Builder;

internal static class HealthCheckExtensions
{
    private static readonly IEnumerable<string> tagsDatabase = ["database"];
    private static readonly IEnumerable<string> tagsServiceBusQueue = ["messaging", "azure", "service bus", "queue"];
    private static readonly IEnumerable<string> tagsServiceBusThreshold = ["messaging", "azure", "service bus", "threshold"];
    private static readonly IEnumerable<string> tagsServiceBusSubscription = ["messaging", "azure", "service bus", "subscription"];
    private static readonly IEnumerable<string> tagsServiceBusTopic = ["messaging", "azure", "service bus", "topic"];
    private static readonly IEnumerable<string> tagsLive = ["live"];

    public static IServiceCollection AddReportStatusHealthChecks(this IServiceCollection services, MessagingSettings messagingSettings)
    {
        var builder = services
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(tags: tagsDatabase)
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), tagsLive);

        if (messagingSettings.Enabled)
        {
            // Add Azure Service Bus checks
            builder
                .AddAzureServiceBusQueue(messagingSettings.ConnectionString, messagingSettings.QueueName, tags: tagsServiceBusQueue)
                .AddAzureServiceBusQueueMessageCountThreshold(messagingSettings.ConnectionString, messagingSettings.QueueName, tags: tagsServiceBusThreshold)
                .AddAzureServiceBusSubscription(messagingSettings.ConnectionString, messagingSettings.TopicName, messagingSettings.SubscriptionName, tags: tagsServiceBusSubscription)
                .AddAzureServiceBusTopic(messagingSettings.ConnectionString, messagingSettings.TopicName, tags: tagsServiceBusTopic);
        }

        return services;
    }

    public static void MapReportStatusHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live"),
        });
    }
}

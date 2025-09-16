using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using System.Reflection;

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
        if (!messagingSettings.Enabled)
        {
            return services;
        }

        services.AddMassTransit(o =>
        {
            var assembly = typeof(Program).Assembly;

            o.SetKebabCaseEndpointNameFormatter();
            o.AddConsumers(assembly);

            // Add the outbox pattern
            o.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            o.UsingAzureServiceBus((context, config) =>
            {
                if (messagingSettings.ConnectionString.Contains("Endpoint=", StringComparison.OrdinalIgnoreCase))
                {
                    config.Host(messagingSettings.ConnectionString);
                }
                else
                {
                    config.Host(new Uri(messagingSettings.ConnectionString));
                }

                var subscriptionName = SubscriptionName(assembly);

                config.SubscriptionEndpoint<EligibilityCheckCreated>(subscriptionName, o => { });
            });
        });

        return services;
    }

    private static string SubscriptionName(Assembly assembly)
    {
        var name = assembly.GetName().Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ConfigurationMissingException("The assembly name cannot be null or empty. Ensure the assembly has a valid name.");
        }
        
        return name.ToLowerInvariant().Replace('.', '-');
    }
}

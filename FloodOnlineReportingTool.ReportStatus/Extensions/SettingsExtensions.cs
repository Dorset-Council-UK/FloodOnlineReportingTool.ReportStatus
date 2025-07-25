using FloodOnlineReportingTool.ReportStatus.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Settings;
using Microsoft.Identity.Web;

namespace Microsoft.AspNetCore.Builder;

internal static class SettingsExtensions
{
    internal static (MicrosoftIdentityOptions, MessagingSettings) AddReportStatusSettings(this IServiceCollection services, IConfigurationManager configuration)
    {
        // Sections
        var messagingSection = configuration.GetSection(MessagingSettings.SectionName)
            ?? throw new ConfigurationMissingException($"Configuration section '{MessagingSettings.SectionName}' is missing.");
        var identitySection = configuration.GetSection(Constants.AzureAd)
            ?? throw new ConfigurationMissingException($"Configuration section '{Constants.AzureAd}' is missing.");

        // Configure settings
        services.Configure<MessagingSettings>(messagingSection);

        // Settings
        var messagingSettings = messagingSection.Get<MessagingSettings>()
            ?? throw new ConfigurationMissingException($"Configuration section '{MessagingSettings.SectionName}' is not properly defined.");
        var identityOptions = identitySection.Get<MicrosoftIdentityOptions>()
            ?? throw new ConfigurationMissingException($"Configuration section '{Constants.AzureAd}' is not properly defined.");

        return (identityOptions, messagingSettings);
    }
}

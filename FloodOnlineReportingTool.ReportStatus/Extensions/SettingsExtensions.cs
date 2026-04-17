using FloodOnlineReportingTool.ReportStatus.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Settings;
using Microsoft.Identity.Web;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class SettingsExtensions
{
    internal static (MicrosoftIdentityOptions, MessagingSettings) AddReportStatusSettings(this IServiceCollection services, IConfigurationManager configuration)
    {
        // Sections
        var messagingSection = configuration.GetRequiredSection(MessagingSettings.SectionName);
        var identitySection = configuration.GetRequiredSection(Constants.AzureAd);
        var reportStatusSection = configuration.GetRequiredSection(ReportStatusSettings.SectionName);

        // Configure settings
        services.Configure<MessagingSettings>(messagingSection);
        services.Configure<ReportStatusSettings>(reportStatusSection);

        // Settings
        var messagingSettings = messagingSection.Get<MessagingSettings>()
            ?? throw new ConfigurationMissingException($"Configuration section '{MessagingSettings.SectionName}' is not properly defined.");
        var identityOptions = identitySection.Get<MicrosoftIdentityOptions>()
            ?? throw new ConfigurationMissingException($"Configuration section '{Constants.AzureAd}' is not properly defined.");
        if (reportStatusSection.Get<ReportStatusSettings>() is null)
        {
            throw new ConfigurationMissingException($"Configuration section '{ReportStatusSettings.SectionName}' is not properly defined.");
        }

        return (identityOptions, messagingSettings);
    }
}

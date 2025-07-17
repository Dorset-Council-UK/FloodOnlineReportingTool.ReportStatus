using FloodOnlineReportingTool.ReportStatus.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Settings;

namespace Microsoft.AspNetCore.Builder;

internal static class SettingsExtensions
{
    internal static MessagingSettings AddReportStatusSettings(this IServiceCollection services, IConfigurationManager configuration)
    {
        // Sections
        var messagingSection = configuration.GetSection(MessagingSettings.SectionName)
            ?? throw new ConfigurationMissingException($"Configuration section '{MessagingSettings.SectionName}' is missing.");

        // Configure settings
        services.Configure<MessagingSettings>(messagingSection);

        // Settings
        var messagingSettings = messagingSection.Get<MessagingSettings>()
            ?? throw new ConfigurationMissingException($"Configuration section '{MessagingSettings.SectionName}' is not properly defined.");

        return messagingSettings;
    }
}

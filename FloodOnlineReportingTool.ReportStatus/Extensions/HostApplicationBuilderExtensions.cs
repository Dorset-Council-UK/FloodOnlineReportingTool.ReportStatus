using FloodOnlineReportingTool.ReportStatus.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Options;
using Microsoft.Identity.Web;

namespace Microsoft.Extensions.Hosting;

internal static class HostApplicationBuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        internal MicrosoftIdentityOptions AddReportStatusIdentityOptions()
        {
            var section = builder.Configuration.GetRequiredSection(Constants.AzureAd);
            var identityOptions = section.Get<MicrosoftIdentityOptions>()
                ?? throw new ConfigurationMissingException($"Configuration section '{Constants.AzureAd}' is not properly defined.");
            return identityOptions;
        }

        internal ReportStatusOptions AddReportStatusOptions()
        {
            var section = builder.Configuration.GetRequiredSection(ReportStatusOptions.SectionName);
            builder.Services.Configure<ReportStatusOptions>(section);
            var reportStatusOptions = section.Get<ReportStatusOptions>()
                ?? throw new ConfigurationMissingException($"Configuration section '{ReportStatusOptions.SectionName}' is not properly defined.");
            return reportStatusOptions;
        }
    }
}

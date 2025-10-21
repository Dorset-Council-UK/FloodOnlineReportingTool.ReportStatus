using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Options;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(
    ILogger<EligibilityCheckCreatedConsumer> logger,
    IOptions<ReportStatusSettings> settings,
    IFloodPropertyService floodPropertyService,
    IFloodRoadService floodRoadService,
    IFloodLocationService floodLocationService,
    IGazetteerSearchService gazetteerSearchService
) : IConsumer<EligibilityCheckCreated>
{
    private readonly ReportStatusSettings _settings = settings.Value;

    public async Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(EligibilityCheckCreated));

        var message = context.Message;
        var cancellationToken = context.CancellationToken;

        // Check if the report triggers a vulnerability warning and sent to subscribers if required
        // Note! This is not an incident management tool but alerts are still sent to help with prioritisation of responses. 
        CheckVulnerability(message);

        // Determine if we have a property, road or location submission
        var reportType = message switch
        {
            { Uprn: not null } => FloodReportType.Property,
            { Usrn: not null } => FloodReportType.Road,
            _ => FloodReportType.Location,
        };

        string locationDescription = await LocationDescriptionForReportType(reportType, message, context.Host, cancellationToken);

        switch (reportType)
        {
            case FloodReportType.Property:
                await floodPropertyService.CreateFromEligibilityCheckCreated(message, locationDescription, cancellationToken).ConfigureAwait(false);
                break;
            case FloodReportType.Road:
                await floodRoadService.CreateFromEligibilityCheckCreated(message, locationDescription, cancellationToken).ConfigureAwait(false);
                break;
            case FloodReportType.Location:
                await floodLocationService.CreateFromEligibilityCheckCreated(message, locationDescription, _settings.LocationBufferMetres, cancellationToken).ConfigureAwait(false);
                break;
        }
    }

    private void CheckVulnerability(EligibilityCheckCreated message)
    {
        // TODO Implement vulnerability check logic
        if (message.IsUninhabitable && message.IsOnGoing && message.VulnerableCount > 1)
        {
            logger.LogInformation("There are {VulnerableCount} vulnerable people in an uninhabitable and ongoing flood area", message.VulnerableCount);
        }
    }

    /// <summary>
    /// Returns a location description for a flood report based on the report type and message data.
    /// <para>If the message contains a <c>LocationDescription</c> that is used.</para>
    /// <para>For <see cref="FloodReportType.Property" />, attempts to resolve the address name using the UPRN via <see cref="IGazetteerSearchService"/>.</para>
    /// <para>For <see cref="FloodReportType.Road" />, attempts to resolve the street name using the USRN via <see cref="IGazetteerSearchService"/>.</para>
    /// <para>For <see cref="FloodReportType.Location" /> or if no description can be resolved, returns a default message.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> containing the most appropriate location description for the report.
    /// </returns>
    private async Task<string> LocationDescriptionForReportType(FloodReportType floodReportType, EligibilityCheckCreated message, HostInfo hostInfo, CancellationToken cancellationToken)
    {
        var locationDescription = message.LocationDescription?.Trim();
        if (!string.IsNullOrWhiteSpace(locationDescription)) return locationDescription;

        var refererUri = new UriBuilder("https", hostInfo.ToString(), 443, "ConsumeEligibilityCheck").Uri;

        switch (floodReportType)
        {
            case FloodReportType.Property:
                var addressName = await gazetteerSearchService
                    .GetAddressNameByUPRN(message.Uprn!.Value, refererUri, cancellationToken)
                    .ConfigureAwait(false);
                return string.IsNullOrWhiteSpace(addressName) ? "No address provided" : addressName;

            case FloodReportType.Road:
                var streetName = await gazetteerSearchService
                    .GetStreetNameByUSRN(message.Usrn!.Value, refererUri, cancellationToken)
                    .ConfigureAwait(false);
                return string.IsNullOrWhiteSpace(streetName) ? "No road name provided" : streetName;

            case FloodReportType.Location:
            default:
                return "No description provided";
        }
    }
}

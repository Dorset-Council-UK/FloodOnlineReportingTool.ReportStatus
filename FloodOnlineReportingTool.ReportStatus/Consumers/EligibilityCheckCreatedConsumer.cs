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

        // TODO use location description when calling floodPropertyService.CreateFromEligibilityCheckCreated and floodRoadService.CreateFromEligibilityCheckCreated
        // Location description
        string locationDescription = "";
        if (message.LocationDescription is null)
        {
            var refererUri = new UriBuilder("https", context.Host.ToString(), 443, "ConsumeEligibilityCheck").Uri;

            // Get the address name for Property or the streey name for Road
            locationDescription = reportType switch
            {
                FloodReportType.Property => await gazetteerSearchService.GetAddressNameByUPRN(message.Uprn!.Value, refererUri, cancellationToken).ConfigureAwait(false),
                FloodReportType.Road => await gazetteerSearchService.GetStreetNameByUSRN(message.Usrn!.Value, refererUri, cancellationToken).ConfigureAwait(false),
                _ => "",
            };
        }

        switch (reportType)
        {
            case FloodReportType.Property:
                await floodPropertyService.CreateFromEligibilityCheckCreated(message, cancellationToken).ConfigureAwait(false);
                break;
            case FloodReportType.Road:
                // TODO new logic to be created
                // var reports = await floodRoadService.GetFloodRoadsByUsrn(message.Usrn!.Value, ct).ConfigureAwait(false);
                await floodRoadService.CreateFromEligibilityCheckCreated(message, cancellationToken).ConfigureAwait(false);
                break;
            case FloodReportType.Location:
                // TODO new logic to be created
                // var withinBufferReports = await floodLocationService.GetFloodLocationsByPoint(message.Easting, message.Northing, _settings.LocationBufferMetres, ct).ConfigureAwait(false);
                await floodLocationService.CreateFromEligibilityCheckCreated(message, cancellationToken).ConfigureAwait(false);
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
}

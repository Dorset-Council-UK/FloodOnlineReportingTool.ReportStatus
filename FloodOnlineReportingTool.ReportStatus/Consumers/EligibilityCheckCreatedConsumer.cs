using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(
    ILogger<EligibilityCheckCreatedConsumer> logger,
    IOptions<ReportStatusSettings> settings,
    IFloodReportService floodReportService,
    IFloodReportSourceService floodReportSourceService,
    IFloodPropertyService floodPropertyService,
    IFloodRoadService floodRoadService,
    IFloodLocationService floodLocationService,
    IFloodPropertyFloodReportService floodPropertyFloodReportService,
    IFloodRoadFloodReportService floodRoadFloodReportService,
    IFloodLocationFloodReportService floodLocationFloodReportService,
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

        // Now record the flood report and flood source records (done for every message passed to the system)
        logger.LogInformation("Creating a new report");
        var floodReport = await floodReportService.CreateFloodReport(message.ToFloodReportDto(), cancellationToken).ConfigureAwait(false);
        var floodReportId = floodReport.Id;
        if (message.FloodSources.Count > 0)
        {
            await floodReportSourceService.CreateFloodReportSources(floodReportId, message.FloodSources, cancellationToken).ConfigureAwait(false);
        }

        // Determine if we have a property, road or location submission
        var reportType = message switch
        {
            { Uprn: not null } => FloodReportType.Property,
            { Usrn: not null } => FloodReportType.Road,
            _ => FloodReportType.Location,
        };

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

        // Determine if this is a possible duplicate
        var isPotentialDuplicate = await IsPotentialDuplicate(reportType, floodReportId, message, cancellationToken).ConfigureAwait(false);

        // If not then create a new record
        if (!isPotentialDuplicate)
        {
            logger.LogInformation("Creating a new report");

            switch (reportType)
            {
                case FloodReportType.Property:
                    var floodProperty = await floodPropertyService.CreateFloodProperty(message.ToFloodPropertyDto(locationDescription), cancellationToken).ConfigureAwait(false);
                    await floodPropertyFloodReportService.CreateFloodPropertyFloodReport(floodProperty.Id, floodReportId, cancellationToken).ConfigureAwait(false);
                    break;
                case FloodReportType.Road:
                    var floodRoad = await floodRoadService.CreateFloodRoad(message.ToFloodRoadDto(locationDescription), cancellationToken).ConfigureAwait(false);
                    await floodRoadFloodReportService.CreateFloodRoadFloodReport(floodRoad.Id, floodReportId, cancellationToken).ConfigureAwait(false);
                    break;
                case FloodReportType.Location:
                    var floodLocation = await floodLocationService.CreateFloodLocation(message.ToFloodLocationDto(), cancellationToken).ConfigureAwait(false);
                    await floodLocationFloodReportService.CreateFloodLocationFloodReport(floodLocation.Id, floodReportId, cancellationToken).ConfigureAwait(false);
                    break;
            }
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

    // <summary>
    // Check if the report is potentially a duplicate, this will then be checked by the team.
    // </summary>
    // <returns><see langword = "true" /> if a duplicate report is found, otherwise<see langword = "false" />.</ returns > "
    private async Task<bool> IsPotentialDuplicate(FloodReportType reportType, Guid floodReportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        // check by UPRN first
        if (await HasDuplicateCandidatesByUprn(reportType, floodReportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no UPRN, check by USRN
        if (await HasDuplicateCandidatesByUsrn(reportType, floodReportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no UPRN or USRN, check by buffer of location
        if (await HasDuplicateCandidatesByLocation(reportType, floodReportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no matches
        return false;
    }

    private async Task<bool> HasDuplicateCandidatesByUprn(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        if (reportType != FloodReportType.Property)
        {
            return false;
        }

        // find existing reports by UPRN
        var reports = await floodPropertyService.GetFloodPropertiesByUprn(message.Uprn!.Value, ct).ConfigureAwait(false);

        // filter reports that match the sources logic
        List<Guid> matchedReports = new List<Guid>();
        foreach (var matchedReport in reports)
        {
            if (matchedReport.FloodReports != null)
            {
                foreach (FloodPropertyFloodReport propertyReport in matchedReport.FloodReports)
                {
                    Guid CurrentID = propertyReport.FloodPropertyId;
                    var report = await floodReportService.GetFloodReportById(propertyReport.FloodReportId, ct).ConfigureAwait(false);
                    if (report != null)
                    {
                        if (HasMatchingSources(report, message))
                        {
                            matchedReports.Add(CurrentID);
                        }
                    }
                } 
            } 
        }

        if (matchedReports.Count == 0)
        {
            return false;
        }

        logger.LogInformation("Found {Count} matching reports by UPRN", matchedReports.Count);
        foreach( var linkedReport in matchedReports)
        {
            await floodPropertyFloodReportService.CreateFloodPropertyFloodReport(linkedReport, reportId, ct).ConfigureAwait(false);
        }
        return true;
    }

    private async Task<bool> HasDuplicateCandidatesByUsrn(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        if (reportType == FloodReportType.Road)
        {
            // find existing reports by USRN
            var reports = await floodRoadService.GetFloodRoadsByUsrn(message.Usrn!.Value, ct).ConfigureAwait(false);

            // filter reports that match the sources logic
            List<Guid> matchedReports = new List<Guid>();
            foreach (var matchedReport in reports)
            {
                if (matchedReport.FloodReports != null)
                {
                    foreach (FloodRoadFloodReport roadReport in matchedReport.FloodReports)
                    {
                        Guid CurrentID = roadReport.FloodRoadId;
                        var report = await floodReportService.GetFloodReportById(roadReport.FloodReportId, ct).ConfigureAwait(false);
                        if (report != null)
                        {
                            if (HasMatchingSources(report, message))
                            {
                                matchedReports.Add(CurrentID);
                            }
                        }
                    }
                }
            }

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports by USRN", matchedReports.Count);
                foreach (var linkedReport in matchedReports)
                {
                    await floodRoadFloodReportService.CreateFloodRoadFloodReport(linkedReport, reportId, ct).ConfigureAwait(false);
                }
                return true;
            }
        }

        return false;
    }

    private async Task<bool> HasDuplicateCandidatesByLocation(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        var withinBufferReports = await floodLocationService.GetFloodLocationsByPoint(message.Easting, message.Northing, _settings.LocationBufferMetres, ct).ConfigureAwait(false);
        if (withinBufferReports.Count > 0)
        {
            // filter reports that match the sources logic
            List<Guid> matchedReports = new List<Guid>();
            foreach (var matchedReport in withinBufferReports)
            {
                if (matchedReport.FloodReports != null)
                {
                    foreach (FloodLocationFloodReport roadReport in matchedReport.FloodReports)
                    {
                        Guid CurrentID = roadReport.FloodLocationId;
                        var report = await floodReportService.GetFloodReportById(roadReport.FloodReportId, ct).ConfigureAwait(false);
                        if (report != null)
                        {
                            if (HasMatchingSources(report, message))
                            {
                                matchedReports.Add(CurrentID);
                            }
                        }
                    }
                }
            }

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports within {Metres} metres", matchedReports.Count, _settings.LocationBufferMetres);
                foreach (var linkedReport in matchedReports)
                {
                    await floodLocationFloodReportService.CreateFloodLocationFloodReport(linkedReport, reportId, ct).ConfigureAwait(false);
                }
                return true;
            }
        }

        return false;
    }

    private bool HasMatchingSources(FloodReport floodReport, EligibilityCheckCreated message)
    {
        // do any of the message Ids match on any of the report Ids
        ImmutableHashSet<Guid> reportSourceIds = [.. floodReport.Sources.Select(s => s.SourceId)];
        ImmutableHashSet<Guid> messageSourceIds = [.. message.FloodSources.Select(s => s.Id)];

        if (!messageSourceIds.Overlaps(reportSourceIds))
        {
            return false;
        }

        var isGroundWaterSource = messageSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;

        return WasReportedWithinDays(floodReport, message, bufferDays);
    }

    private bool WasReportedWithinDays(FloodReport floodReport, EligibilityCheckCreated message, int bufferDays)
    {
        if (floodReport.ImpactStartUtc == null)
        {
            logger.LogError("Existing flood report does not have an ImpactStartUtc date, please investigate.");
            return false;
        }
        if (message.ImpactStartUTC == null)
        {
            logger.LogError("Incoming message does not have an ImpactStartUTC date, please investigate.");
            return false;
        }

        DateTimeOffset existingStartDate = floodReport.ImpactStartUtc.Value;
        DateTimeOffset existingEndDate = floodReport.ImpactStartUtc.Value.AddHours(floodReport.ImpactDurationHours);

        DateTimeOffset incomingStartDate = (message.ImpactStartUTC.Value).AddDays(-bufferDays);
        DateTimeOffset incomingEndDate = message.IsOnGoing ? DateTimeOffset.UtcNow : message.ImpactStartUTC.Value.AddHours(message.ImpactDurationHours);

        // check if the ranges overlap
        bool rangesOverlap = existingStartDate <= incomingEndDate && incomingStartDate <= existingEndDate;
        return rangesOverlap;
    }
}

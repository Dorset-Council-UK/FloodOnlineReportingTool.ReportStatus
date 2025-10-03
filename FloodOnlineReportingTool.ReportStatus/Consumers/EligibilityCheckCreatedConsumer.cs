using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using static FloodOnlineReportingTool.ReportStatus.Database.Models.FloodReport;

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
    IFloodLocationFloodReportService floodLocationFloodReportService
) : IConsumer<EligibilityCheckCreated>
{
    private readonly ReportStatusSettings _settings = settings.Value;

    public async Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(EligibilityCheckCreated));

        var message = context.Message;

        // Check if the report triggers a vulnerability warning and sent to subscribers if required
        // Note! This is not an incident management tool but alerts are still sent to help with prioritisation of responses. 
        CheckVulnerability(message);

        // Now record the flood report and flood source records (done for every message passed to the system)
        logger.LogInformation("Creating a new report");
        var newReport = await floodReportService.CreateFloodReport(message.ToFloodReportDto(), context.CancellationToken).ConfigureAwait(false);
        foreach(var floodSource in message.FloodSources)
        {
            await floodReportSourceService.CreateFloodReportSource(newReport.Id, floodSource.Id, floodSource.ToFloodReportSourceDto(), context.CancellationToken).ConfigureAwait(false);
        }

        // Determine if we have a property, road or location submission
        FloodReportType reportType = GetReportType(message);

        // Determine if this is a possible duplicate
        var isPotentialDuplicate = await IsPotentialDuplicate(reportType, newReport.Id, message, context.CancellationToken).ConfigureAwait(false);

        // If not then create a new record
        if (!isPotentialDuplicate)
        {
            logger.LogInformation("Creating a new report");

            switch (reportType)
            {
                case FloodReportType.Property:
                    var propertyDtoModel = message.ToFloodPropertyDto();
                    if (propertyDtoModel != null)
                    {
                        FloodProperty propertyReport = await floodPropertyService.CreateFloodProperty(propertyDtoModel, context.CancellationToken).ConfigureAwait(false);
                        await floodPropertyFloodReportService.CreateFloodPropertyFloodReport(propertyReport.Id, newReport.Id, context.CancellationToken).ConfigureAwait(false);
                    }
                    break;
                case FloodReportType.Road:
                    var roadDtoModel = message.ToFloodRoadDto();
                    if (roadDtoModel != null)
                    {
                        FloodRoad roadReport = await floodRoadService.CreateFloodRoad(roadDtoModel, context.CancellationToken).ConfigureAwait(false);
                        await floodRoadFloodReportService.CreateFloodRoadFloodReport(roadReport.Id, newReport.Id, context.CancellationToken).ConfigureAwait(false);
                    }
                    break;
                case FloodReportType.Location:
                    FloodLocation locationReport = await floodLocationService.CreateFloodLocation(message.ToFloodLocationDto(), context.CancellationToken).ConfigureAwait(false);
                    await floodLocationFloodReportService.CreateFloodLocationFloodReport(locationReport.Id, newReport.Id, context.CancellationToken).ConfigureAwait(false);
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

    private FloodReportType GetReportType(EligibilityCheckCreated message)
    {
        if (message.Uprn != null)
        {
            return FloodReportType.Property;
        }
        else if (message.Usrn != null)
        {
            return FloodReportType.Road;
        }
        else
        {
            return FloodReportType.Location;
        }
    }

    // <summary>
    // Check if the report is potentially a duplicate, this will then be checked by the team.
    // </summary>
    // <returns><see langword = "true" /> if a duplicate report is found, otherwise<see langword = "false" />.</ returns > "
    private async Task<bool> IsPotentialDuplicate(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        // check by UPRN first
        if (await HasDuplicateCandidatesByUprn(reportType, reportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no UPRN, check by USRN
        if (await HasDuplicateCandidatesByUsrn(reportType, reportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no UPRN or USRN, check by buffer of location
        if (await HasDuplicateCandidatesByLocation(reportType, reportId, message, ct).ConfigureAwait(false))
        {
            return true;
        }

        // no matches
        return false;
    }

    private async Task<bool> HasDuplicateCandidatesByUprn(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        if (reportType == FloodReportType.Property)
        {
            // find existing reports by UPRN
#pragma warning disable CS8629 // Nullable value type may be null.
            var reports = await floodPropertyService.GetFloodPropertiesByUprn(message.Uprn.Value, ct).ConfigureAwait(false);
#pragma warning restore CS8629 // Nullable value type may be null.

            // filter reports that match the sources logic
            List<Guid> matchedReports = new List<Guid>();
            foreach (var matchedReport in reports)
            {
                if (matchedReport.FloodReports != null)
                {
                    foreach (FloodPropertyFloodReport propreport in matchedReport.FloodReports)
                    {
                        Guid CurrentID = propreport.FloodPropertyId;
                        var report = await floodReportService.GetFloodReportById(propreport.FloodReportId, ct).ConfigureAwait(false);
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
                logger.LogInformation("Found {Count} matching reports by UPRN", matchedReports.Count);
                foreach( var linkedReport in matchedReports)
                {
                    await floodPropertyFloodReportService.CreateFloodPropertyFloodReport(linkedReport, reportId, ct).ConfigureAwait(false);
                }
                return true;
            }
        }

        return false;
    }

    private async Task<bool> HasDuplicateCandidatesByUsrn(FloodReportType reportType, Guid reportId, EligibilityCheckCreated message, CancellationToken ct)
    {
        if (reportType == FloodReportType.Road)
        {
            // find existing reports by USRN
#pragma warning disable CS8629 // Nullable value type may be null.
            var reports = await floodRoadService.GetFloodRoadsByUsrn(message.Usrn.Value, ct).ConfigureAwait(false);
#pragma warning restore CS8629 // Nullable value type may be null.

            // filter reports that match the sources logic
            var floodReports = reports.Where(p => p.FloodReports != null).SelectMany(p => p.FloodReports).ToList();
            List<FloodReport> matchedReports = new List<FloodReport>();
            foreach (var matchedReport in floodReports)
            {
                var report = matchedReport.FloodReport;
                if (HasMatchingSources(report, message))
                {
                    matchedReports.Add(report);
                }
            }

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports by USRN", matchedReports.Count);
                foreach (var linkedReport in matchedReports)
                {
                    await floodRoadFloodReportService.CreateFloodRoadFloodReport(linkedReport.Id, reportId, ct).ConfigureAwait(false);
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
            var floodReports = withinBufferReports.Where(p => p.FloodReports != null).SelectMany(p => p.FloodReports).ToList();
            List<FloodReport> matchedReports = new List<FloodReport>();
            foreach (var matchedReport in floodReports)
            {
                var report = matchedReport.FloodReport;
                if (HasMatchingSources(report, message))
                {
                    matchedReports.Add(report);
                }
            }

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports within {Metres} metres", matchedReports.Count, _settings.LocationBufferMetres);
                foreach (var linkedReport in matchedReports)
                {
                    await floodLocationFloodReportService.CreateFloodLocationFloodReport(linkedReport.Id, reportId, ct).ConfigureAwait(false);
                }
                return true;
            }
        }

        return false;
    }

    private bool HasMatchingSources(FloodReport report, EligibilityCheckCreated message)
    {
        // do any of the message Ids match on any of the report Ids
        ImmutableHashSet<Guid> reportSourceIds = [.. report.Sources.Select(s => s.SourceId)];
        ImmutableHashSet<Guid> messageSourceIds = [.. message.FloodSources.Select(s => s.Id)];

        if (!messageSourceIds.Overlaps(reportSourceIds))
        {
            return false;
        }

        var isGroundWaterSource = messageSourceIds.Contains(FloodSourceIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;

        return WasReportedWithinDays(report, message, bufferDays);
    }

    private bool WasReportedWithinDays(FloodReport report, EligibilityCheckCreated message, int bufferDays)
    {
        if (report.ImpactStartUtc == null)
        {
            logger.LogError("Existing report does not have an ImpactStartUtc date, please investigate.");
            return false;
        }
        if (message.ImpactStartUTC == null)
        {
            logger.LogError("Incoming message does not have an ImpactStartUTC date, please investigate.");
            return false;
        }

        DateTimeOffset existingStartDate = report.ImpactStartUtc.Value;
        DateTimeOffset existingEndDate = report.ImpactStartUtc.Value.AddHours(report.ImpactDurationHours);

        DateTimeOffset incomingStartDate = (message.ImpactStartUTC.Value).AddDays(-bufferDays);
        DateTimeOffset incomingEndDate = message.IsOnGoing ? DateTimeOffset.UtcNow : message.ImpactStartUTC.Value.AddHours(message.ImpactDurationHours);

        // check if the ranges overlap
        bool rangesOverlap = existingStartDate <= incomingEndDate && incomingStartDate <= existingEndDate;
        return rangesOverlap;
    }
}

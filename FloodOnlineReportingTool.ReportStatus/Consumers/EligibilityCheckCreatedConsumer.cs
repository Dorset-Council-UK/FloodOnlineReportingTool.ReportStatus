using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(
    ILogger<EligibilityCheckCreatedConsumer> logger,
    IOptions<ReportStatusSettings> settings,
    IReportService reportService,
    IReportDuplicateService reportDuplicateService
) : IConsumer<EligibilityCheckCreated>
{
    private readonly ReportStatusSettings _settings = settings.Value;

    public async Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(EligibilityCheckCreated));

        var message = context.Message;

        CheckVulnerability(message);
        
        var isPotentialDuplicate = await IsPotentialDuplicate(message, context.CancellationToken).ConfigureAwait(false);

        if (!isPotentialDuplicate)
        {
            logger.LogInformation("Creating a new report");
            await reportService.CreateReport(message.ToReportDto(), context.CancellationToken).ConfigureAwait(false);
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
    /// Check if the report is potentially a duplicate, this will then be checked by the team.
    /// </summary>
    /// <returns><see langword="true"/> if a duplicate report is found, otherwise <see langword="false"/>.</returns>"
    private async Task<bool> IsPotentialDuplicate(EligibilityCheckCreated message, CancellationToken ct)
    {
        // check by UPRN first
        if (message.Uprn != null)
        {
            // find existing reports by UPRN
            var reports = await reportService.GetReportsByUprn(message.Uprn.Value, ct).ConfigureAwait(false);

            // filter reports that match the sources logic
            IReadOnlyCollection<Report> matchedReports = [.. reports.Where(report => HasMatchingSources(report, message))];

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports by UPRN", matchedReports.Count);
                await reportDuplicateService.CreateDuplicates(matchedReports, ct).ConfigureAwait(false);
                return true;
            }

            return false;
        }

        // no UPRN, check by USRN
        if (message.Usrn != null)
        {
            // find existing reports by USRN
            var reports = await reportService.GetReportsByUsrn(message.Usrn.Value, ct).ConfigureAwait(false);

            // filter reports that match the sources logic
            IReadOnlyCollection<Report> matchedReports = [.. reports.Where(report => HasMatchingSources(report, message))];

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports by USRN", matchedReports.Count);
                await reportDuplicateService.CreateDuplicates(matchedReports, ct).ConfigureAwait(false);
                return true;
            }

            return false;
        }

        // no UPRN or USRN, check by buffer of location
        var withinBufferReports = await reportService.GetReportsWithinMetres(message.Easting, message.Northing, _settings.LocationBufferMetres, ct).ConfigureAwait(false);
        if (withinBufferReports.Count > 0)
        {
            // filter reports that match the sources logic
            IReadOnlyCollection<Report> matchedReports = [.. withinBufferReports.Where(report => HasMatchingSources(report, message))];

            if (matchedReports.Count > 0)
            {
                logger.LogInformation("Found {Count} matching reports within {Metres} metres", matchedReports.Count, _settings.LocationBufferMetres);
                await reportDuplicateService.CreateDuplicates(matchedReports, ct).ConfigureAwait(false);
                return true;
            }

            return false;
        }

        // no matches
        return false;
    }

    private bool HasMatchingSources(Report report, EligibilityCheckCreated message)
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

    private bool WasReportedWithinDays(Report report, EligibilityCheckCreated message, int bufferDays)
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

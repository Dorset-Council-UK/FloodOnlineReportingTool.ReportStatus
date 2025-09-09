using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using MassTransit;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Consumers;

internal class EligibilityCheckCreatedConsumer(
    ILogger<EligibilityCheckCreatedConsumer> logger,
    IReportService reportService,
    IReportDuplicateService reportDuplicateService
) : IConsumer<EligibilityCheckCreated>
{
    private readonly double _metres = 50;
    private readonly int _vulnerableThreshold = 9999;

    public async Task Consume(ConsumeContext<EligibilityCheckCreated> context)
    {
        logger.LogInformation("Consuming contract: {Contract}", nameof(EligibilityCheckCreated));

        var message = context.Message;

        CheckUninhabitable(message);
        CheckVulnerableCount(message);
        
        var isDuplicate = await CheckDuplicate(message, context.CancellationToken).ConfigureAwait(false);

        if (!isDuplicate)
        {
            logger.LogInformation("Creating a new report");
            await reportService.CreateReport(message.ToReportDto(), context.CancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Check if the area is uninhabitable. Sending a danger alert if it is.
    /// </summary>
    private void CheckUninhabitable(EligibilityCheckCreated eligibilityCheckCreated)
    {
        if (eligibilityCheckCreated.IsUninhabitable)
        {
            // TODO Create a danger alert notification
            logger.LogInformation("Area is uninhabitable");
        }
    }

    /// <summary>
    /// Check if the vulnerable count exceeds the threshold.
    /// </summary>
    private void CheckVulnerableCount(EligibilityCheckCreated eligibilityCheckCreated)
    {
        if (eligibilityCheckCreated.VulnerableCount > _vulnerableThreshold)
        {
            // TODO Create vulnerable alert logic
            logger.LogInformation("There are {VulnerableCount} vulnerable people in the area", eligibilityCheckCreated.VulnerableCount);
        }
    }

    /// <summary>
    /// Check if the report is a duplicate.
    /// </summary>
    /// <remarks>Existing reports are found by UPRN, by USRN, or within metres of a recent flood</remarks>
    /// <returns><see langword="true"/> if a duplicate report is found, otherwise <see langword="false"/>.</returns>"
    private async Task<bool> CheckDuplicate(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        var report = await CheckUprn(eligibilityCheckCreated.Uprn, ct).ConfigureAwait(false);
        if (report != null)
        {
            if (CheckSources(report, eligibilityCheckCreated))
            {
                var duplicate = await CreateDuplicateReport(report.Id, eligibilityCheckCreated, ct).ConfigureAwait(false);
                return (duplicate != null);
            }
        }

        report = await CheckUsrn(eligibilityCheckCreated.Usrn, ct).ConfigureAwait(false);
        if (report != null)
        {
            var duplicate = await CreateDuplicateReport(report.Id, eligibilityCheckCreated, ct).ConfigureAwait(false);
            return (duplicate != null);
        }

        report = await CheckNearbyFlood(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing, ct).ConfigureAwait(false);
        if (report != null)
        {
            var duplicate = await CreateDuplicateReport(report.Id, eligibilityCheckCreated, ct).ConfigureAwait(false);
            return (duplicate != null);
        }

        return false;
    }

    /// <summary>
    /// Check if there is an existing report with a matching UPRN.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    private async Task<Report?> CheckUprn(long? uprn, CancellationToken ct)
    {
        if (uprn == null)
        {
            logger.LogInformation("Message does not have a UPRN");
            return null;
        }

        var report = await reportService.GetReportByUprn(uprn.Value, ct).ConfigureAwait(false);

        if (report == null)
        {
            logger.LogInformation("Not able to find an existing report with matching UPRN: {Uprn}", uprn);
            return null;
        }

        logger.LogInformation("Found an existing report with matching UPRN: {Uprn}", uprn);
        return report;
    }

    /// <summary>
    /// Check if there is an existing report with a matching USRN.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    private async Task<Report?> CheckUsrn(long? usrn, CancellationToken ct)
    {
        if (usrn == null)
        {
            logger.LogInformation("Message does not have a USRN");
            return null;
        }

        var report = await reportService.GetReportByUsrn(usrn.Value, ct).ConfigureAwait(false);

        if (report == null)
        {
            logger.LogInformation("Not able to find an existing report with matching USRN: {Usrn}", usrn);
            return null;
        }

        logger.LogInformation("Found an existing report with matching USRN: {Usrn}", usrn);
        return report;
    }

    /// <summary>
    /// Check if any of the sources match, or if the report is recent enough to be considered a duplicate.
    /// </summary>
    private static bool CheckSources(Report report, EligibilityCheckCreated eligibilityCheckCreated)
    {
        // if any of the message Ids match on any of the report Ids, return true for a match
        ImmutableHashSet<Guid> reportSourceIds = [.. report.Sources.Select(s => s.Id)];
        ImmutableHashSet<Guid> messageSourceIds = [.. eligibilityCheckCreated.FloodSources.Select(s => s.Id)];

        if (messageSourceIds.Overlaps(reportSourceIds))
        {
            return true;
        }

        // is ground water source?
        var isGroundWaterSource = messageSourceIds.Contains(FloodSourceIds.WaterRisingOutOfTheGround);
        var days = isGroundWaterSource ? 14 : 7;

        return IsWithinDays(report, eligibilityCheckCreated, days);
    }

    /// <summary>
    /// Is the eligibility check within a specified distance of an existing report.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    private async Task<Report?> CheckNearbyFlood(double easting, double northing, CancellationToken ct)
    {
        var report = await reportService
            .GetReportWithinMetres(easting, northing, _metres, ct)
            .ConfigureAwait(false);

        if (report != null)
        {
            logger.LogInformation("Eligibility check is within {Metres} metres of an existing report.", _metres);
            return report;
        }

        logger.LogInformation("Not able to find an existing report within {Metres} metres.", _metres);
        return null;
    }

    /// <summary>
    /// When a nearby flood is found, check if it is recent enough to be considered a duplicate.
    /// </summary>
    /// <remarks>Use TimeSpan to check if the message is within the specified number of days from the report.</remarks>
    private static bool IsWithinDays(Report report, EligibilityCheckCreated eligibilityCheckCreated, int days)
    {
        if (eligibilityCheckCreated.IsOnGoing)
        {
            var todayDaysDifference = (DateTimeOffset.UtcNow - report.CreatedUtc).TotalDays;
            return eligibilityCheckCreated.IsOnGoing && todayDaysDifference <= days;
        }

        if (eligibilityCheckCreated.ImpactStartUTC == null || report.ImpactStartUtc == null)
        {
            // TODO Decide if no dates should be considered recent
            return true;
        }

        var daysDifference = (eligibilityCheckCreated.ImpactStartUTC.Value - report.ImpactStartUtc.Value).TotalDays;
        return daysDifference <= days;
    }

    private async Task<ReportDuplicate?> CreateDuplicateReport(Guid reportId, EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        logger.LogInformation("Creating duplicate report of report ID {ReportId}", reportId);
        return await reportDuplicateService.CreateDuplicate(reportId, eligibilityCheckCreated.ToReportDuplicateDto(), ct).ConfigureAwait(false);
    }
}

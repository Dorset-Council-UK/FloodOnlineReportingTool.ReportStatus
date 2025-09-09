using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using MassTransit;

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
    /// <remarks>Existing reports are found by UPRN or within metres of a recent flood</remarks>
    /// <returns><see langword="true"/> if a duplicate report is found, otherwise <see langword="false"/>.</returns>"
    private async Task<bool> CheckDuplicate(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        var report = await CheckUprn(eligibilityCheckCreated.Uprn, ct).ConfigureAwait(false);
        if (report != null)
        {
            var duplicate = await CreateDuplicateReport(report.Id, eligibilityCheckCreated, ct).ConfigureAwait(false);
            return (duplicate != null);
        }

        //report = await CheckRecentlyFlooded(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing, ct).ConfigureAwait(false);
        //if (report != null)
        //{
        //    var duplicate = await CreateDuplicateReport(report.Id, eligibilityCheckCreated, ct).ConfigureAwait(false);
        //    return (duplicate != null);
        //}

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
    /// Is the eligibility check within a specified distance of an existing report.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    private async Task<Report?> CheckRecentlyFlooded(DateTimeOffset? startDateUtc, CancellationToken ct)
    {
        var days = IsGroundWaterSource() ? 14 : 7;

        if (IsRecentFlood2(startDateUtc, days))
        {
            logger.LogInformation("Creating duplicate report of report ID {ReportId}", report.Id);
            await reportDuplicateService.CreateDuplicate(report.Id, message.ToReportDuplicateDto(), context.CancellationToken).ConfigureAwait(false);
            return;
        }

        if (report != null)
        {
            logger.LogInformation("Eligibility check is within {Metres} metres of an existing report.", _metres);
            return report;
        }

        logger.LogInformation("Not able to find an existing report within {Metres} metres.", _metres);

        return report;
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

    private bool IsGroundWaterSource()
    {
        // TODO Sources is missing at the moment.
        // Assuming eligibilityCheckCreated.Sources is an IReadOnlyCollection<EligibilityCheckSource>
        // And its the FloodProblemId I have to compare, which might mean FloodProblemIds needs moving to contracts?
        //return eligibilityCheckCreated.Sources.Any(source => source.FloodProblemId == FloodProblemIds.WaterRisingOutOfTheGround);
        return false;
    }

    /// <summary>
    /// When a nearby flood is found, check if it is recent enough to be considered a duplicate.
    /// </summary>
    private static bool IsRecentFlood(EligibilityCheckCreated eligibilityCheckCreated, Report report, int days)
    {
        DateTimeOffset? startDateUtc = eligibilityCheckCreated.ImpactStartUTC;
        DateTimeOffset? reportStartDateUtc = report.ImpactStartUtc;
        
        if (startDateUtc == null || reportStartDateUtc == null)
        {
            // TODO Decide if no dates should be considered recent (possibly an ongoing flood?)
            return false;
        }

        // Using a TimeSpan, check if the message is within the specified number of days from the report
        var daysDifference = (startDateUtc.Value - reportStartDateUtc.Value).TotalDays;
        return daysDifference <= days;
    }


    private async Task<ReportDuplicate?> CreateDuplicateReport(Guid reportId, EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        logger.LogInformation("Creating duplicate report of report ID {ReportId}", reportId);
        return await reportDuplicateService.CreateDuplicate(reportId, eligibilityCheckCreated.ToReportDuplicateDto(), ct).ConfigureAwait(false);
    }
}

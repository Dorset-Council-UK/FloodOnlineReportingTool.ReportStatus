namespace FloodOnlineReportingTool.ReportStatus.Services;

internal interface IGovNotifyEmailSender
{
    Task<string> SendVulnerableNotification(Guid floodEventId, double easting, double northing, int vulnerableCount, DateTimeOffset reportDate);
}

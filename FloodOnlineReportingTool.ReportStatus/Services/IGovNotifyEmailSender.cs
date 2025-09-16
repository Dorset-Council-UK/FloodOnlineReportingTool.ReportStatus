namespace FloodOnlineReportingTool.ReportStatus.Services;

internal interface IGovNotifyEmailSender
{
    Task<string> SendVulnerableNotification(Guid reportId, double easting, double northing);
}

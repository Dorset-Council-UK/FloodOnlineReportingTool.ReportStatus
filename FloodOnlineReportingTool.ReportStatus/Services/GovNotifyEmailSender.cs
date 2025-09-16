using FloodOnlineReportingTool.ReportStatus.Settings;
using Microsoft.Extensions.Options;
using Notify.Client;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Services;

internal class GovNotifyEmailSender(ILogger<GovNotifyEmailSender> logger, IOptions<GovNotifySettings> options, NotificationClient notificationClient) : IGovNotifyEmailSender
{
    private readonly GovNotifySettings _govNotifySettings = options.Value;

    /// <summary>
    ///     <para>Send an email to the specified email address via GovNotify.</para>
    ///     <para>If there is a problem pass the exception back up.</para>
    /// </summary>
    /// <remarks>
    /// Error codes are documented at <see href="https://docs.notifications.service.gov.uk/net.html#send-an-email-error-codes">Error Codes</see>
    /// </remarks>
    private async Task<string> SendEmail(string emailAddress, string templateId, Dictionary<string, dynamic>? personalisation, string? clientReference = null, string? emailReplyToId = null, string? oneClickUnsubscribeURL = null)
    {
        logger.LogDebug("Sending email to {EmailAddress}", emailAddress);

        var response = await notificationClient
            .SendEmailAsync(emailAddress, templateId, personalisation, clientReference, emailReplyToId, oneClickUnsubscribeURL)
            .ConfigureAwait(false);

        logger.LogInformation("Email sent to {EmailAddress} with GovNotify response ID {ResponseId}", emailAddress, response.id);

        return response.id;
    }

    public async Task<string> SendVulnerableNotification(Guid reportId, double easting, double northing)
    {
        var zoom = 17;
        var url = string.Create(CultureInfo.InvariantCulture, $"https://gi.dorsetcouncil.gov.uk/dorsetexplorer#map={zoom}/{easting}/{northing}/0/27700");

        var personalisation = new Dictionary<string, dynamic>(StringComparer.CurrentCulture)
        {
            { "report_id", reportId },
            { "url", url },
        };

        // TODO Replace with real email address when ready
        var emailAddress = "matthew.vincent@dorsetcouncil.gov.uk";

        return await SendEmail(emailAddress, _govNotifySettings.Templates.Vulnerable, personalisation).ConfigureAwait(false);
    }
}

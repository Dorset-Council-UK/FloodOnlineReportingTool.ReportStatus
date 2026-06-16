using FloodOnlineReportingTool.ReportStatus.Options;
using Microsoft.Extensions.Options;
using Notify.Client;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Services;

internal class GovNotifyEmailSender(
    ILogger<GovNotifyEmailSender> logger,
    IOptions<GovNotifyOptions> options,
    IWebHostEnvironment environment,
    ICurrentUserService currentUserService,
    NotificationClient notificationClient
) : IGovNotifyEmailSender
{
    private readonly GovNotifyOptions _govNotifyOptions = options.Value;

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

    private string RiskManagementUrl()
    {
        if (environment.IsDevelopment())
        {
            return "https://localhost:7112/floodriskmanagement";
        }

        if (environment.IsStaging())
        {
            return "https://gi-staging.dorsetcouncil.gov.uk/floodriskmanagement";
        }

        return "https://gi.dorsetcouncil.gov.uk/floodriskmanagement";
    }

    private string DorsetExplorerUrl(int zoom, double easting, double northing)
    {
        var host = environment.IsDevelopment() ? "https://gi-staging.dorsetcouncil.gov.uk/dorsetexplorer" : "https://gi.dorsetcouncil.gov.uk/dorsetexplorer";
        return string.Create(CultureInfo.InvariantCulture, $"{host}#map={zoom}/{easting}/{northing}/0/27700");
    }

    private string GetVulnerablemailAddress()
    {
        if (environment.IsDevelopment() && currentUserService.IsAuthenticated)
        {
            return currentUserService.Email;
        }

        // TODO Work out the logic of who to send this to in production
        return "";
    }

    public async Task<string> SendVulnerableNotification(Guid floodEventId, double easting, double northing, int vulnerableCount, DateTimeOffset reportDate)
    {
        var personalisation = new Dictionary<string, dynamic>(StringComparer.CurrentCulture)
        {
            { "from_development", environment.IsDevelopment() },
            { "admin_url", $"[Risk Management]({RiskManagementUrl()}/flood-event/{floodEventId})" },
            { "flood_location_url", $"[on Dorset Explorer]({DorsetExplorerUrl(17, easting, northing)})" },
            { "vulnerable_count", vulnerableCount },
            { "report_date", reportDate.GdsReadable() },
        };

        var emailAddress = GetVulnerablemailAddress();
        return await SendEmail(emailAddress, _govNotifyOptions.Templates.Vulnerable, personalisation).ConfigureAwait(false);
    }
}

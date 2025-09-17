using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class TestEndpoints
{
    internal static async Task<Results<Ok<string>, BadRequest>> TestVulnerableNotification(IGovNotifyEmailSender govNotifyEmailSender, CancellationToken ct)
    {
        var floodEventId = Guid.CreateVersion7();
        var easting = 123456.0;
        var northing = 654321.0;
        var vulnerableCount = 5;
        var reportDate = DateTimeOffset.UtcNow.AddDays(-2);

        try
        {
            var responseId = await govNotifyEmailSender
                .SendVulnerableNotification(floodEventId, easting, northing, vulnerableCount, reportDate)
                .ConfigureAwait(false);
            return TypedResults.Ok(responseId);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest();
        }
    }
}

using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class TestEndpoints
{
    internal static async Task<Results<Ok<string>, BadRequest>> TestVulnerableNotification(IGovNotifyEmailSender govNotifyEmailSender)
    {
        var floodEventId = Guid.CreateVersion7();
        double easting = 368911;
        double northing = 90272;
        var vulnerableCount = 5;
        var reportDate = DateTimeOffset.UtcNow.AddDays(-2);

        try
        {
            var responseId = await govNotifyEmailSender
                .SendVulnerableNotification(floodEventId, easting, northing, vulnerableCount, reportDate)
                .ConfigureAwait(false);
            return TypedResults.Ok(responseId);
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
    }

    internal static async Task<Results<Ok, BadRequest>> TestFloodReportMessage(IFloodReportService floodReportService, CancellationToken cancellationToken = default)
    {
        try
        {
            var now = DateTimeOffset.UtcNow;
            var ecc = new EligibilityCheckRecord(
                Guid.CreateVersion7(),
                null,
                null,
                368991.12,
                90881.94,
                now,
                1,
                false,
                false,
                1,
                "Test location description",
                [],
                [
                    new(PrimaryCauseIds.River, "River"),
                    new(PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
                ] 
            );
            var prsc = new FloodReportSourceCreated(
                Guid.CreateVersion7(),
                25,
                "Test1234",
                new Uri("https://example.com"),
                now,
                ecc,
                false,
                false,
                []
            );

            await floodReportService.CreateFromFloodReportCreated(prsc, cancellationToken).ConfigureAwait(false);
            return TypedResults.Ok();
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
    }
}

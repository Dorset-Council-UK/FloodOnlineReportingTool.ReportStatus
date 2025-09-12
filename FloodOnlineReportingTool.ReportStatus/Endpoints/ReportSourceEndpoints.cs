using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class ReportSourceEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<ReportSource>>, BadRequest>> GetReportSources(IReportSourceService reportSourceService, Guid reportId, CancellationToken ct)
    {
        var reportSources = await reportSourceService.GetReportSources(reportId, ct).ConfigureAwait(false);
        return TypedResults.Ok(reportSources);
    }

    internal static async Task<Results<Ok<ReportSource>, NotFound>> GetReportSource(IReportSourceService reportSourceService, Guid reportId, Guid sourceId, CancellationToken ct)
    {
        var reportSource = await reportSourceService.GetReportSource(reportId, sourceId, ct).ConfigureAwait(false);
        return reportSource == null ? TypedResults.NotFound() : TypedResults.Ok(reportSource);
    }

    internal static async Task<Results<Created<ReportSource>, BadRequest>> CreateReportSource(IReportSourceService reportSourceService, Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct)
    {
        var reportSource = await reportSourceService.CreateReportSource(reportId, sourceId, dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/reports/{reportSource.ReportId}/sources/{reportSource.SourceId}", reportSource);
    }

    internal static async Task<Results<Ok<ReportSource>, NotFound, BadRequest>> UpdateReportSource(IReportSourceService reportSourceService, Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct)
    {
        var reportSource = await reportSourceService.UpdateReportSource(reportId, sourceId, dto, ct).ConfigureAwait(false);
        return reportSource == null ? TypedResults.NotFound() : TypedResults.Ok(reportSource);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteReportSource(IReportSourceService reportSourceService, Guid reportId, Guid sourceId, CancellationToken ct)
    {
        var deleted = await reportSourceService.DeleteReportSource(reportId, sourceId, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class ReportEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<Report>>, BadRequest>> GetReports(IReportService reportService, CancellationToken ct)
    {
        var reports = await reportService.GetReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<Report>, NotFound>> GetReportById(IReportService reportService, Guid id, CancellationToken ct)
    {
        var report = await reportService.GetReportById(id, ct).ConfigureAwait(false);
        return report == null ? TypedResults.NotFound() : TypedResults.Ok(report);
    }

    internal static async Task<Results<Created<Report>, BadRequest>> CreateReport(IReportService reportService, ReportDto dto, CancellationToken ct)
    {
        var report = await reportService.CreateReport(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/reports/{report.Id}", report);
    }

    internal static async Task<Results<Ok<Report>, NotFound, BadRequest>> UpdateReport(IReportService reportService, Guid id, ReportDto dto, CancellationToken ct)
    {
        var report = await reportService.UpdateReport(id, dto, ct).ConfigureAwait(false);
        return report == null ? TypedResults.NotFound() : TypedResults.Ok(report);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteReport(IReportService reportService, Guid id, CancellationToken ct)
    {
        var deleted = await reportService.DeleteReport(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}

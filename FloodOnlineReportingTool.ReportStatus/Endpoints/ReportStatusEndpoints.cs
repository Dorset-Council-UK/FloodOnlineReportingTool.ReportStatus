using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class ReportStatusEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyList<Report>>, BadRequest>> GetReports(IReportService reportService, CancellationToken ct)
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
        var deleted = await reportService.DeleteReport(id, ct) .ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static Results<Ok, BadRequest> Test(HttpContext context, CancellationToken ct)
    {
        // get the users roles
        var user = context.User;
        var hasUser = user != null;
        var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
        var roles = user?.Claims.ToList() ?? [];

        return TypedResults.Ok();
    }

    internal static async Task<Results<Ok<IReadOnlyList<ReportDuplicate>>, BadRequest>> GetDuplicateReports(IReportDuplicateService reportDuplicateService, CancellationToken ct)
    {
        var duplicates = await reportDuplicateService.GetDuplicateReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(duplicates);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound>> GetDuplicateReportById(IReportDuplicateService reportDuplicateService, Guid id, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.GetDuplicateReportById(id, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<Created<ReportDuplicate>, BadRequest>> CreateDuplicateReport(IReportDuplicateService reportDuplicateService, ReportDuplicateDto dto, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.CreateDuplicateReport(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/reports/duplicate/{duplicate.Id}", duplicate);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound, BadRequest>> UpdateDuplicateReport(IReportDuplicateService reportDuplicateService, Guid id, ReportDuplicateDto dto, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.UpdateDuplicateReport(id, dto, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteDuplicateReport(IReportDuplicateService reportDuplicateService, Guid id, CancellationToken ct)
    {
        var deleted = await reportDuplicateService.DeleteDuplicateReport(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}

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

    internal static async Task<Results<Ok<IReadOnlyList<ReportDuplicate>>, BadRequest>> GetDuplicates(IReportDuplicateService reportDuplicateService, CancellationToken ct)
    {
        var duplicates = await reportDuplicateService.GetDuplicates(ct).ConfigureAwait(false);
        return TypedResults.Ok(duplicates);
    }

    internal static async Task<Results<Ok<IReadOnlyList<ReportDuplicate>>, BadRequest>> GetReportDuplicates(IReportDuplicateService reportDuplicateService, Guid reportId, CancellationToken ct)
    {
        var duplicates = await reportDuplicateService.GetDuplicates(reportId, ct).ConfigureAwait(false);
        return TypedResults.Ok(duplicates);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound>> GetDuplicate(IReportDuplicateService reportDuplicateService, Guid duplicateId, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.GetDuplicate(duplicateId, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound>> GetReportDuplicate(IReportDuplicateService reportDuplicateService, Guid reportId, Guid duplicateId, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.GetDuplicate(reportId, duplicateId, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<Created<ReportDuplicate>, NotFound, BadRequest>> CreateReportDuplicate(IReportDuplicateService reportDuplicateService, Guid reportId, ReportDuplicateDto dto, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.CreateDuplicate(reportId, dto, ct).ConfigureAwait(false);
        return duplicate == null
            ? TypedResults.NotFound()
            : TypedResults.Created($"/api/reports/{duplicate.ReportId}/duplicates/{duplicate.Id}", duplicate);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound, BadRequest>> UpdateDuplicate(IReportDuplicateService reportDuplicateService, Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.UpdateDuplicate(duplicateId, dto, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<Ok<ReportDuplicate>, NotFound, BadRequest>> UpdateReportDuplicate(IReportDuplicateService reportDuplicateService, Guid reportId, Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct)
    {
        var duplicate = await reportDuplicateService.UpdateDuplicate(reportId, duplicateId, dto, ct).ConfigureAwait(false);
        return duplicate == null ? TypedResults.NotFound() : TypedResults.Ok(duplicate);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteDuplicate(IReportDuplicateService reportDuplicateService, Guid duplicateId, CancellationToken ct)
    {
        var deleted = await reportDuplicateService.DeleteDuplicate(duplicateId, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteReportDuplicate(IReportDuplicateService reportDuplicateService, Guid reportId, Guid duplicateId, CancellationToken ct)
    {
        var deleted = await reportDuplicateService.DeleteDuplicate(reportId, duplicateId, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodPropertyFloodReportEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodPropertyFloodReport>>, BadRequest>> GetFloodReports(IFloodPropertyFloodReportService floodPropertyFloodReportService, Guid floodPropertyId, CancellationToken ct)
    {
        var floodPropertyFloodReports = await floodPropertyFloodReportService
            .GetFloodPropertyFloodReports(floodPropertyId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(floodPropertyFloodReports);
    }

    internal static async Task<Results<Ok<FloodPropertyFloodReport>, NotFound>> GetFloodReport(IFloodPropertyFloodReportService floodPropertyFloodReportService, Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        var floodPropertyFloodReport = await floodPropertyFloodReportService
            .GetFloodPropertyFloodReport(floodPropertyId, floodReportId, ct)
            .ConfigureAwait(false);
        return floodPropertyFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodPropertyFloodReport);
    }

    internal static async Task<Results<Created<FloodPropertyFloodReport>, BadRequest>> CreateFloodReport(IFloodPropertyFloodReportService floodPropertyFloodReportService, Guid floodPropertyId, Guid floodReportId, Guid floodReportSourceId, CancellationToken ct)
    {
        var floodPropertyFloodReport = await floodPropertyFloodReportService
            .CreateFloodPropertyFloodReport(floodPropertyId, floodReportId, floodReportSourceId, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/property/{floodPropertyFloodReport.FloodPropertyId}/reports/{floodPropertyFloodReport.FloodReportId}", floodPropertyFloodReport);
    }

    internal static async Task<Results<Created<FloodPropertyFloodReport>, BadRequest>> CreateFloodReportConfirmed(IFloodPropertyFloodReportService floodPropertyFloodReportService, ICurrentUserService currentUserService, Guid floodPropertyId, Guid floodReportId, Guid floodReportSourceId, CancellationToken ct)
    {
        var floodPropertyFloodReport = await floodPropertyFloodReportService
            .CreateFloodPropertyFloodReport(floodPropertyId, floodReportId, floodReportSourceId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/property/{floodPropertyFloodReport.FloodPropertyId}/reports/{floodPropertyFloodReport.FloodReportId}", floodPropertyFloodReport);
    }

    internal static async Task<Results<Ok<FloodPropertyFloodReport>, NotFound, BadRequest>> ConfirmFloodReport(IFloodPropertyFloodReportService floodPropertyFloodReportService, ICurrentUserService currentUserService, Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        var floodPropertyFloodReport = await floodPropertyFloodReportService
            .ConfirmFloodPropertyFloodReport(floodPropertyId, floodReportId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return floodPropertyFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodPropertyFloodReport);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodReport(IFloodPropertyFloodReportService floodPropertyFloodReportService, Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        var deleted = await floodPropertyFloodReportService
            .DeleteFloodPropertyFloodReport(floodPropertyId, floodReportId, ct)
            .ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportExists(IFloodPropertyFloodReportService floodPropertyFloodReportService, Guid floodPropertyId, Guid floodReportId, CancellationToken ct)
    {
        var exists = await floodPropertyFloodReportService
            .FloodPropertyFloodReportExists(floodPropertyId, floodReportId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

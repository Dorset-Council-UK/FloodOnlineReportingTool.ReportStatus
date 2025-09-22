using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodRoadFloodReportEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodRoadFloodReport>>, BadRequest>> GetFloodReports(IFloodRoadFloodReportService floodRoadFloodReportService, Guid floodRoadId, CancellationToken ct)
    {
        var floodRoadFloodReports = await floodRoadFloodReportService
            .GetFloodRoadFloodReports(floodRoadId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(floodRoadFloodReports);
    }

    internal static async Task<Results<Ok<FloodRoadFloodReport>, NotFound>> GetFloodReport(IFloodRoadFloodReportService floodRoadFloodReportService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var floodRoadFloodReport = await floodRoadFloodReportService
            .GetFloodRoadFloodReport(floodRoadId, floodReportId, ct)
            .ConfigureAwait(false);
        return floodRoadFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodRoadFloodReport);
    }

    internal static async Task<Results<Created<FloodRoadFloodReport>, BadRequest>> CreateFloodReport(IFloodRoadFloodReportService floodRoadFloodReportService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var floodRoadFloodReport = await floodRoadFloodReportService
            .CreateFloodRoadFloodReport(floodRoadId, floodReportId, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/road/{floodRoadFloodReport.FloodRoadId}/reports/{floodRoadFloodReport.FloodReportId}", floodRoadFloodReport);
    }

    internal static async Task<Results<Created<FloodRoadFloodReport>, BadRequest>> CreateFloodReportConfirmed(IFloodRoadFloodReportService floodRoadFloodReportService, ICurrentUserService currentUserService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var floodRoadFloodReport = await floodRoadFloodReportService
            .CreateFloodRoadFloodReport(floodRoadId, floodReportId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/road/{floodRoadFloodReport.FloodRoadId}/reports/{floodRoadFloodReport.FloodReportId}", floodRoadFloodReport);
    }

    internal static async Task<Results<Ok<FloodRoadFloodReport>, NotFound, BadRequest>> ConfirmFloodReport(IFloodRoadFloodReportService floodRoadFloodReportService, ICurrentUserService currentUserService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var floodRoadFloodReport = await floodRoadFloodReportService
            .ConfirmFloodRoadFloodReport(floodRoadId, floodReportId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return floodRoadFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodRoadFloodReport);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodReport(IFloodRoadFloodReportService floodRoadFloodReportService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var deleted = await floodRoadFloodReportService
            .DeleteFloodRoadFloodReport(floodRoadId, floodReportId, ct)
            .ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportExists(IFloodRoadFloodReportService floodRoadFloodReportService, Guid floodRoadId, Guid floodReportId, CancellationToken ct)
    {
        var exists = await floodRoadFloodReportService
            .FloodRoadFloodReportExists(floodRoadId, floodReportId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

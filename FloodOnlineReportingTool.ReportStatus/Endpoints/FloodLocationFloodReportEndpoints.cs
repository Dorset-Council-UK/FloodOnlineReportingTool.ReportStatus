using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodLocationFloodReportEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocationFloodReport>>, BadRequest>> GetFloodReports(IFloodLocationFloodReportService floodLocationFloodReportService, Guid floodLocationId, CancellationToken ct)
    {
        var floodLocationFloodReports = await floodLocationFloodReportService
            .GetFloodLocationFloodReports(floodLocationId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(floodLocationFloodReports);
    }

    internal static async Task<Results<Ok<FloodLocationFloodReport>, NotFound>> GetFloodReport(IFloodLocationFloodReportService floodLocationFloodReportService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var floodLocationFloodReport = await floodLocationFloodReportService
            .GetFloodLocationFloodReport(floodLocationId, floodReportId, ct)
            .ConfigureAwait(false);
        return floodLocationFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocationFloodReport);
    }

    internal static async Task<Results<Created<FloodLocationFloodReport>, BadRequest>> CreateFloodReport(IFloodLocationFloodReportService floodLocationFloodReportService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var floodLocationFloodReport = await floodLocationFloodReportService
            .CreateFloodLocationFloodReport(floodLocationId, floodReportId, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/location/{floodLocationFloodReport.FloodLocationId}/reports/{floodLocationFloodReport.FloodReportId}", floodLocationFloodReport);
    }

    internal static async Task<Results<Created<FloodLocationFloodReport>, BadRequest>> CreateFloodReportConfirmed(IFloodLocationFloodReportService floodLocationFloodReportService, ICurrentUserService currentUserService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var floodLocationFloodReport = await floodLocationFloodReportService
            .CreateFloodLocationFloodReport(floodLocationId, floodReportId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/location/{floodLocationFloodReport.FloodLocationId}/reports/{floodLocationFloodReport.FloodReportId}", floodLocationFloodReport);
    }

    internal static async Task<Results<Ok<FloodLocationFloodReport>, NotFound, BadRequest>> ConfirmFloodReport(IFloodLocationFloodReportService floodLocationFloodReportService, ICurrentUserService currentUserService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var floodLocationFloodReport = await floodLocationFloodReportService
            .ConfirmFloodLocationFloodReport(floodLocationId, floodReportId, currentUserService.UserId, currentUserService.Name, ct)
            .ConfigureAwait(false);
        return floodLocationFloodReport is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocationFloodReport);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodReport(IFloodLocationFloodReportService floodLocationFloodReportService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var deleted = await floodLocationFloodReportService
            .DeleteFloodLocationFloodReport(floodLocationId, floodReportId, ct)
            .ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportExists(IFloodLocationFloodReportService floodLocationFloodReportService, Guid floodLocationId, Guid floodReportId, CancellationToken ct)
    {
        var exists = await floodLocationFloodReportService
            .FloodLocationFloodReportExists(floodLocationId, floodReportId, ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

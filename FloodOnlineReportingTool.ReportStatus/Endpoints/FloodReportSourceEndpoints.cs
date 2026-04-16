using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodReportSourceEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReportSource>>, BadRequest>> GetFloodReportSources(IFloodReportSourceService floodReportSourceService, Guid floodReportId, CancellationToken ct)
    {
        var floodReportSources = await floodReportSourceService.GetFloodReportSources(floodReportId, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReportSources);
    }

    internal static async Task<Results<Ok<FloodReportSource>, NotFound>> GetFloodReportSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        var floodReportSource = await floodReportSourceService.GetFloodReportSource(floodReportId, sourceId, ct).ConfigureAwait(false);
        return floodReportSource == null ? TypedResults.NotFound() : TypedResults.Ok(floodReportSource);
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> CreateReportSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, FloodReportNameDto dto, CancellationToken ct)
    {
        var floodReportSource = await floodReportSourceService.AddFloodReportFloodSource(floodReportId, sourceId, dto, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReportSource);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteReportFloodSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        var deleted = await floodReportSourceService.DeleteFloodReportFloodSource(floodReportId, sourceId, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportFloodSourceExists(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        var exists = await floodReportSourceService.FloodReportFloodSourceExists(floodReportId, sourceId, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

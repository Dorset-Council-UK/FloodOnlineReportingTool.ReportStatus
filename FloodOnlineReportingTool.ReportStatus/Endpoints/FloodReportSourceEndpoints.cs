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

    internal static async Task<Results<Created<FloodReportSource>, BadRequest>> CreateReportSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct)
    {
        var floodReportSource = await floodReportSourceService.CreateFloodReportSource(floodReportId, sourceId, dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/reports/{floodReportSource.FloodReportId}/sources/{floodReportSource.SourceId}", floodReportSource);
    }

    internal static async Task<Results<Ok<FloodReportSource>, NotFound, BadRequest>> UpdateReportSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct)
    {
        var floodReportSource = await floodReportSourceService.UpdateFloodReportSource(floodReportId, sourceId, dto, ct).ConfigureAwait(false);
        return floodReportSource == null ? TypedResults.NotFound() : TypedResults.Ok(floodReportSource);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteReportSource(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        var deleted = await floodReportSourceService.DeleteFloodReportSource(floodReportId, sourceId, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportSourceExists(IFloodReportSourceService floodReportSourceService, Guid floodReportId, Guid sourceId, CancellationToken ct)
    {
        var exists = await floodReportSourceService.FloodReportSourceExists(floodReportId, sourceId, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

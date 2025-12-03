using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodRoadEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodRoad>>, BadRequest>> GetFloodRoads(IFloodRoadService floodRoadService, CancellationToken ct)
    {
        var floodRoads = await floodRoadService.GetFloodRoads(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodRoads);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodRoad>>, BadRequest>> GetFloodRoadsByUsrn(IFloodRoadService floodRoadService, long usrn, CancellationToken ct)
    {
        var floodRoads = await floodRoadService.GetFloodRoadsByUsrn(usrn, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodRoads);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodRoad>>, BadRequest>> GetFloodRoadsWithReports(IFloodRoadService floodRoadService, CancellationToken ct)
    {
        var floodRoads = await floodRoadService.GetFloodRoadsWithReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodRoads);
    }

    internal static async Task<Results<Ok<FloodRoad>, NotFound>> GetFloodRoad(IFloodRoadService floodRoadService, Guid id, CancellationToken ct)
    {
        var floodRoad = await floodRoadService.GetFloodRoad(id, ct).ConfigureAwait(false);
        return floodRoad is null ? TypedResults.NotFound() : TypedResults.Ok(floodRoad);
    }

    internal static async Task<Results<Ok<FloodRoad>, NotFound>> GetFloodRoadWithReports(IFloodRoadService floodRoadService, Guid id, CancellationToken ct)
    {
        var floodRoad = await floodRoadService.GetFloodRoadWithReports(id, ct).ConfigureAwait(false);
        return floodRoad is null ? TypedResults.NotFound() : TypedResults.Ok(floodRoad);
    }

    internal static async Task<Results<Created<FloodRoad>, BadRequest>> CreateFloodRoad(IFloodRoadService floodRoadService, FloodRoadDto dto, CancellationToken ct)
    {
        var floodRoad = await floodRoadService.CreateFloodRoad(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/road/{floodRoad.Id}", floodRoad);
    }

    internal static async Task<Results<Ok<FloodRoad>, NotFound, BadRequest>> UpdateFloodRoad(IFloodRoadService floodRoadService, Guid id, FloodRoadDto dto, CancellationToken ct)
    {
        var floodRoad = await floodRoadService.UpdateFloodRoad(id, dto, ct).ConfigureAwait(false);
        return floodRoad is null ? TypedResults.NotFound() : TypedResults.Ok(floodRoad);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodRoad(IFloodRoadService floodRoadService, Guid id, CancellationToken ct)
    {
        var deleted = await floodRoadService.DeleteFloodRoad(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodRoadExists(IFloodRoadService floodRoadService, Guid id, CancellationToken ct)
    {
        var exists = await floodRoadService.FloodRoadExists(id, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

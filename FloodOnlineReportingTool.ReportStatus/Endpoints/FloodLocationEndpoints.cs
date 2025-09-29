using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodLocationEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocation>>, BadRequest>> GetFloodLocations(IFloodLocationService floodLocationService, CancellationToken ct)
    {
        var floodLocations = await floodLocationService.GetFloodLocations(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodLocations);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocation>>, BadRequest>> GetFloodLocationsWithReports(IFloodLocationService floodLocationService, CancellationToken ct)
    {
        var floodLocations = await floodLocationService.GetFloodLocationsWithReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodLocations);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound>> GetFloodLocation(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.GetFloodLocation(id, ct).ConfigureAwait(false);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound>> GetFloodLocationWithReports(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.GetFloodLocationWithReports(id, ct).ConfigureAwait(false);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<Created<FloodLocation>, BadRequest>> CreateFloodLocation(IFloodLocationService floodLocationService, FloodLocationDto dto, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.CreateFloodLocation(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/location/{floodLocation.Id}", floodLocation);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound, BadRequest>> UpdateFloodLocation(IFloodLocationService floodLocationService, Guid id, FloodLocationDto dto, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.UpdateFloodLocation(id, dto, ct).ConfigureAwait(false);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodLocation(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var deleted = await floodLocationService.DeleteFloodLocation(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodLocationExists(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var exists = await floodLocationService.FloodLocationExists(id, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

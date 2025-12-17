using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodLocationEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocation>>, BadRequest>> GetFloodLocations(IFloodLocationService floodLocationService, CancellationToken ct)
    {
        var floodLocations = await floodLocationService.GetFloodLocations(ct);
        return TypedResults.Ok(floodLocations);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocation>>, ValidationProblem>> GetFloodLocationsByPoint(IFloodLocationService floodLocationService, double easting, double northing, double buffer, CancellationToken ct)
    {
        if (buffer is < 0 or > IFloodLocationService.MaxBuffer)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(buffer), [string.Create(CultureInfo.InvariantCulture, $"Buffer must be between 0 and {IFloodLocationService.MaxBuffer} metres.")] },
            });
        }

        var floodLocations = await floodLocationService.GetFloodLocationsByPoint(easting, northing, buffer, ct);
        return TypedResults.Ok(floodLocations);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodLocation>>, BadRequest>> GetFloodLocationsWithReports(IFloodLocationService floodLocationService, CancellationToken ct)
    {
        var floodLocations = await floodLocationService.GetFloodLocationsWithReports(ct);
        return TypedResults.Ok(floodLocations);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound>> GetFloodLocation(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.GetFloodLocation(id, ct);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound>> GetFloodLocationWithReports(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.GetFloodLocationWithReports(id, ct);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<Created<FloodLocation>, BadRequest>> CreateFloodLocation(IFloodLocationService floodLocationService, FloodLocationDto dto, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.CreateFloodLocation(dto, ct);
        return TypedResults.Created($"/api/flood/location/{floodLocation.Id}", floodLocation);
    }

    internal static async Task<Results<Ok<FloodLocation>, NotFound, BadRequest>> UpdateFloodLocation(IFloodLocationService floodLocationService, Guid id, FloodLocationDto dto, CancellationToken ct)
    {
        var floodLocation = await floodLocationService.UpdateFloodLocation(id, dto, ct);
        return floodLocation is null ? TypedResults.NotFound() : TypedResults.Ok(floodLocation);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodLocation(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var deleted = await floodLocationService.DeleteFloodLocation(id, ct);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodLocationExists(IFloodLocationService floodLocationService, Guid id, CancellationToken ct)
    {
        var exists = await floodLocationService.FloodLocationExists(id, ct);
        return TypedResults.Ok(exists);
    }
}

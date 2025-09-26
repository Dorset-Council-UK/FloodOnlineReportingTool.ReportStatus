using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodPropertyEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodProperty>>, BadRequest>> GetFloodProperties(IFloodPropertyService floodPropertyService, CancellationToken ct)
    {
        var floodProperties = await floodPropertyService.GetFloodProperties(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodProperties);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodProperty>>, BadRequest>> GetFloodPropertiesWithReports(IFloodPropertyService floodPropertyService, CancellationToken ct)
    {
        var floodProperties = await floodPropertyService.GetFloodPropertiesWithReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodProperties);
    }

    internal static async Task<Results<Ok<FloodProperty>, NotFound>> GetFloodProperty(IFloodPropertyService floodPropertyService, Guid id, CancellationToken ct)
    {
        var floodProperty = await floodPropertyService.GetFloodProperty(id, ct).ConfigureAwait(false);
        return floodProperty is null ? TypedResults.NotFound() : TypedResults.Ok(floodProperty);
    }

    internal static async Task<Results<Ok<FloodProperty>, NotFound>> GetFloodPropertyWithReports(IFloodPropertyService floodPropertyService, Guid id, CancellationToken ct)
    {
        var floodProperty = await floodPropertyService.GetFloodPropertyWithReports(id, ct).ConfigureAwait(false);
        return floodProperty is null ? TypedResults.NotFound() : TypedResults.Ok(floodProperty);
    }

    internal static async Task<Results<Created<FloodProperty>, BadRequest>> CreateFloodProperty(IFloodPropertyService floodPropertyService, FloodPropertyDto dto, CancellationToken ct)
    {
        var floodProperty = await floodPropertyService.CreateFloodProperty(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/property/{floodProperty.Id}", floodProperty);
    }

    internal static async Task<Results<Ok<FloodProperty>, NotFound, BadRequest>> UpdateFloodProperty(IFloodPropertyService floodPropertyService, Guid id, FloodPropertyDto dto, CancellationToken ct)
    {
        var floodProperty = await floodPropertyService.UpdateFloodProperty(id, dto, ct).ConfigureAwait(false);
        return floodProperty is null ? TypedResults.NotFound() : TypedResults.Ok(floodProperty);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodProperty(IFloodPropertyService floodPropertyService, Guid id, CancellationToken ct)
    {
        var deleted = await floodPropertyService.DeleteFloodProperty(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodPropertyExists(IFloodPropertyService floodPropertyService, Guid id, CancellationToken ct)
    {
        var exists = await floodPropertyService.FloodPropertyExists(id, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

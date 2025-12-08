using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodLocationEndpoints
{
    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, BadRequest>> GetFloodLocations(ISearchFloodLocationService searchFloodLocationService, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        var Locations = await searchFloodLocationService.GetFloodLocations(pageNumber, pageSize, ct);
        return TypedResults.Ok(Locations);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, BadRequest>> GetFloodLocationsWithReports(ISearchFloodLocationService searchFloodLocationService, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        var Locations = await searchFloodLocationService.GetFloodLocationsWithReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(Locations);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, BadRequest>> GetFloodLocationsByPoint(ISearchFloodLocationService searchFloodLocationService, double easting, double northing, double buffer, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        var Locations = await searchFloodLocationService.GetFloodLocationsByPoint(easting, northing, buffer, pageNumber, pageSize, ct);
        return TypedResults.Ok(Locations);
    }
}

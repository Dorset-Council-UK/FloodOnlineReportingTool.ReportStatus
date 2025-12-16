using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodLocationEndpoints
{
    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, BadRequest>> GetFloodLocations(ISearchFloodLocationService searchFloodLocationService, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        var locations = await searchFloodLocationService.GetFloodLocations(pageNumber, pageSize, ct);
        return TypedResults.Ok(locations);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, BadRequest>> GetFloodLocationsWithReports(ISearchFloodLocationService searchFloodLocationService, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        var locations = await searchFloodLocationService.GetFloodLocationsWithReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(locations);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodLocation>>, ValidationProblem>> GetFloodLocationsByPoint(ISearchFloodLocationService searchFloodLocationService, double easting, double northing, double buffer, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        if (buffer is < 0 or > ISearchFloodLocationService.MaxBuffer)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(buffer), [string.Create(CultureInfo.InvariantCulture, $"Buffer must be between 0 and {ISearchFloodLocationService.MaxBuffer} metres.")] },
            });
        }

        var locations = await searchFloodLocationService.GetFloodLocationsByPoint(easting, northing, buffer, pageNumber, pageSize, ct);
        return TypedResults.Ok(locations);
    }
}

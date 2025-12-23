using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodRoadEndpoints
{
    internal static async Task<Results<Ok<PagedResult<SearchResultFloodRoad>>, BadRequest>> GetFloodRoads(ISearchFloodRoadService searchFloodRoadService, int pageNumber = 1, int pageSize = ISearchFloodRoadService.DefaultPageSize, CancellationToken ct = default)
    {
        var roads = await searchFloodRoadService.GetFloodRoads(pageNumber, pageSize, ct);
        return TypedResults.Ok(roads);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodRoad>>, BadRequest>> GetFloodRoadsWithReports(ISearchFloodRoadService searchFloodRoadService, int pageNumber = 1, int pageSize = ISearchFloodRoadService.DefaultPageSize, CancellationToken ct = default)
    {
        var roadsWithReports = await searchFloodRoadService.GetFloodRoadsWithReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(roadsWithReports);
    }
}

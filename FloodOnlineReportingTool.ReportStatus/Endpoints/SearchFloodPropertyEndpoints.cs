using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodPropertyEndpoints
{
    internal static async Task<Results<Ok<PagedResult<SearchResultFloodProperty>>, BadRequest>> GetFloodProperties(ISearchFloodPropertyService searchFloodPropertyService, int pageNumber = 1, int pageSize = ISearchFloodPropertyService.DefaultPageSize, CancellationToken ct = default)
    {
        var properties = await searchFloodPropertyService.GetFloodProperties(pageNumber, pageSize, ct);
        return TypedResults.Ok(properties);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodProperty>>, BadRequest>> GetFloodPropertiesWithReports(ISearchFloodPropertyService searchFloodPropertyService, int pageNumber = 1, int pageSize = ISearchFloodPropertyService.DefaultPageSize, CancellationToken ct = default)
    {
        var propertiesWithReports = await searchFloodPropertyService.GetFloodPropertiesWithReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(propertiesWithReports);
    }
}

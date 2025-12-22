using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodRoadService(ILogger<SearchFloodRoadService> logger, IFloodRoadRepository floodRoadRepository) : ISearchFloodRoadService
{
    public async Task<PagedResult<SearchResultFloodRoad>> GetFloodRoads(int pageNumber = 1, int pageSize = ISearchFloodRoadService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood roads from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodRoadRepository
            .GetFloodRoads()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodRoadService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodRoad>> GetFloodRoadsWithReports(int pageNumber = 1, int pageSize = ISearchFloodRoadService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood roads with their flood reports from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodRoadRepository
            .GetFloodRoadsWithReports()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodRoadService.DefaultPageSize, ct);
    }
}

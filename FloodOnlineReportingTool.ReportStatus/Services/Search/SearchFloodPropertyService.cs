using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodPropertyService(ILogger<SearchFloodPropertyService> logger, IFloodPropertyRepository floodPropertyRepository) : ISearchFloodPropertyService
{
    public async Task<PagedResult<SearchResultFloodProperty>> GetFloodProperties(int pageNumber = 1, int pageSize = ISearchFloodPropertyService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood properties from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodPropertyRepository
            .GetFloodProperties()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodPropertyService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodProperty>> GetFloodPropertiesWithReports(int pageNumber = 1, int pageSize = ISearchFloodPropertyService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood properties with their flood reports from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodPropertyRepository
            .GetFloodPropertiesWithReports()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodPropertyService.DefaultPageSize, ct);
    }
}

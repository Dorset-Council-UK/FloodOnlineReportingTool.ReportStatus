using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodLocationService(ILogger<SearchFloodLocationService> logger, IFloodLocationRepository floodLocationRepository) : ISearchFloodLocationService
{
    public async Task<PagedResult<SearchResultFloodLocation>> GetFloodLocations(int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood locations from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodLocationRepository
            .GetFloodLocations()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodLocationService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodLocation>> GetFloodLocationsWithReports(int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood locations from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = floodLocationRepository
            .GetFloodLocationsWithReports()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodLocationService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodLocation>> GetFloodLocationsByPoint(double easting, double northing, double buffer, int pageNumber = 1, int pageSize = ISearchFloodLocationService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood locations within {buffer} distance of coordinates (Easting: {Easting}, Northing: {Northing}) from page {PageNumber} for {PageSize} items per page.", buffer, easting, northing, pageNumber, pageSize);

        var query = floodLocationRepository
            .GetFloodLocationsByPoint(easting, northing, buffer)
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodLocationService.DefaultPageSize, ct);
    }
}

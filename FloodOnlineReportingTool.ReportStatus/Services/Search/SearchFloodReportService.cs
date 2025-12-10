using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodReportService(ILogger<SearchFloodReportService> logger, IFloodReportRepository floodReportRepository) : ISearchFloodReportService
{
    public async Task<PagedResult<SearchResultFloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        // convert the FloodReport to SearchResultFloodReport and the FloodReportSource to SearchResultFloodReportSource
        var query = floodReportRepository
            .GetFloodReports()
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports within {Metres} metres of coordinates (Easting: {Easting}, Northing: {Northing}) from page {PageNumber} for {PageSize} items per page.", metres, easting, northing, pageNumber, pageSize);

        if (metres > ISearchFloodReportService.MaxMetres)
        {
            logger.LogWarning("Metres {Metres} exceeds maximum allowed metres of {MaxMetres}. Using maximum metres instead.", metres, ISearchFloodReportService.MaxMetres);
            metres = ISearchFloodReportService.MaxMetres;
        }

        var query = floodReportRepository
            .GetFloodReportsWithinMetres(easting, northing, metres)
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with UPRN {Uprn} from page {PageNumber} for {PageSize} items per page.", uprn, pageNumber, pageSize);

        var query = floodReportRepository
            .GetFloodReportsByUprn(uprn)
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize, ct);
    }

    public async Task<PagedResult<SearchResultFloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with USRN {Usrn} from page {PageNumber} for {PageSize} items per page.", usrn, pageNumber, pageSize);

        var query = floodReportRepository
            .GetFloodReportsByUsrn(usrn)
            .ToSearchResult();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize, ct);
    }
}

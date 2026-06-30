using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodReportService(
    ILogger<SearchFloodReportService> logger,
    IFloodReportRepository floodReportRepository
    ) : ISearchFloodReportService
{
    public async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var floodReportQuery = floodReportRepository.GetFloodReports();
        return await CreatePagedResultWithTotalFloodSourceCount(floodReportQuery, pageNumber, pageSize, ct);
    }

    public async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports within {Metres} metres of coordinates (Easting: {Easting}, Northing: {Northing}) from page {PageNumber} for {PageSize} items per page.", metres, easting, northing, pageNumber, pageSize);

        metres = Math.Clamp(metres, 0, ISearchFloodReportService.MaxMetres);
        var floodReportQuery = floodReportRepository.GetFloodReportsWithinMetres(easting, northing, metres);
        return await CreatePagedResultWithTotalFloodSourceCount(floodReportQuery, pageNumber, pageSize, ct);
    }

    public async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with UPRN {Uprn} from page {PageNumber} for {PageSize} items per page.", uprn, pageNumber, pageSize);

        var floodReportQuery = floodReportRepository.GetFloodReportsByUprn(uprn);
        return await CreatePagedResultWithTotalFloodSourceCount(floodReportQuery, pageNumber, pageSize, ct);
    }

    public async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with USRN {Usrn} from page {PageNumber} for {PageSize} items per page.", usrn, pageNumber, pageSize);

        var floodReportQuery = floodReportRepository.GetFloodReportsByUsrn(usrn);
        return await CreatePagedResultWithTotalFloodSourceCount(floodReportQuery, pageNumber, pageSize, ct);
    }

    public async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> SearchFloodReports(DateTimeOffset? resultsFrom, DateTimeOffset? resultsTo, ICollection<Guid> floodSources, long? uprn, long? usrn, string? postcode, MultiPolygon? geom, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with general search criteria from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var floodReportQuery = floodReportRepository.SearchFloodReports(resultsFrom, resultsTo, floodSources, uprn, usrn, postcode, geom);
        return await CreatePagedResultWithTotalFloodSourceCount(floodReportQuery, pageNumber, pageSize, ct);
    }

    private static async Task<SearchFloodReportPagedResult<SearchResultFloodReport>> CreatePagedResultWithTotalFloodSourceCount(IQueryable<FloodReport> floodReportQuery, int pageNumber, int pageSize, CancellationToken ct)
    {
        var totalFloodSourceCount = await floodReportQuery
            .SelectMany(fr => fr.SourceRecords)
            .CountAsync(ct);

        var pagedResult = await SearchUtility.CreatePagedResult(
            floodReportQuery.ToSearchResult(),
            pageNumber,
            pageSize,
            ISearchFloodReportService.DefaultPageSize,
            ct);

        return new SearchFloodReportPagedResult<SearchResultFloodReport>(
            pagedResult.Results,
            pagedResult.TotalCount,
            totalFloodSourceCount,
            pagedResult.PageSize,
            pagedResult.CurrentPage,
            pagedResult.TotalPages);
    }
}

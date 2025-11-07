using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories.Search;
using FloodOnlineReportingTool.ReportStatus.Models.Search;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal class SearchFloodReportService(ILogger<SearchFloodReportService> logger, ISearchFloodReportRepository searchFloodReportRepository) : ISearchFloodReportService
{
    public async Task<PagedResult<FloodReport>> GetFloodReports(int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports from page {PageNumber} for {PageSize} items per page.", pageNumber, pageSize);

        var query = searchFloodReportRepository.GetFloodReports();
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize);
    }

    public async Task<PagedResult<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports within {Metres} metres of coordinates (Easting: {Easting}, Northing: {Northing}) from page {PageNumber} for {PageSize} items per page.", metres, easting, northing, pageNumber, pageSize);

        var query = searchFloodReportRepository.GetFloodReportsWithinMetres(easting, northing, metres);
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize);
    }

    public async Task<PagedResult<FloodReport>> GetFloodReportsByUprn(long uprn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with UPRN {Uprn} from page {PageNumber} for {PageSize} items per page.", uprn, pageNumber, pageSize);

        var query = searchFloodReportRepository.GetFloodReportsByUprn(uprn);
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize);
    }

    public async Task<PagedResult<FloodReport>> GetFloodReportsByUsrn(long usrn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Searching flood reports with USRN {Usrn} from page {PageNumber} for {PageSize} items per page.", usrn, pageNumber, pageSize);

        var query = searchFloodReportRepository.GetFloodReportsByUsrn(usrn);
        return await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, ISearchFloodReportService.DefaultPageSize);
    }
}

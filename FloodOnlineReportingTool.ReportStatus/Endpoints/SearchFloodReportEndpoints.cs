using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodReportEndpoints
{
    internal static async Task<Results<Ok<PagedResult<SearchResultFloodReport>>, BadRequest>> GetFloodReports(ISearchFloodReportService searchFloodReportService, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodReport>>, ValidationProblem>> GetFloodReportsWithinMetres(ISearchFloodReportService searchFloodReportService, double easting, double northing, double metres = 50, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        if (metres < 0 || metres > ISearchFloodReportService.MaxMetres)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(metres), [string.Create(CultureInfo.InvariantCulture, $"Metres must be between 0 and {ISearchFloodReportService.MaxMetres} metres.")] },
            });
        }

        var reports = await searchFloodReportService.GetFloodReportsWithinMetres(easting, northing, metres, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodReport>>, BadRequest>> GetFloodReportsByUprn(ISearchFloodReportService searchFloodReportService, long uprn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReportsByUprn(uprn, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<PagedResult<SearchResultFloodReport>>, BadRequest>> GetFloodReportsByUsrn(ISearchFloodReportService searchFloodReportService, long usrn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReportsByUsrn(usrn, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }
}

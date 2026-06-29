using FloodOnlineReportingTool.ReportStatus.Models;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchFloodReportEndpoints
{
    internal static async Task<Results<Ok<SearchFloodReportPagedResult>, BadRequest>> GetFloodReports(ISearchFloodReportService searchFloodReportService, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReports(pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<SearchFloodReportPagedResult>, ValidationProblem>> GetFloodReportsWithinMetres(ISearchFloodReportService searchFloodReportService, double easting, double northing, double metres = 50, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        if (metres is < 0 or > ISearchFloodReportService.MaxMetres)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(metres), [string.Create(CultureInfo.InvariantCulture, $"Metres must be between 0 and {ISearchFloodReportService.MaxMetres} metres.")] },
            });
        }

        var reports = await searchFloodReportService.GetFloodReportsWithinMetres(easting, northing, metres, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<SearchFloodReportPagedResult>, BadRequest>> GetFloodReportsByUprn(ISearchFloodReportService searchFloodReportService, long uprn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReportsByUprn(uprn, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<SearchFloodReportPagedResult>, BadRequest>> GetFloodReportsByUsrn(ISearchFloodReportService searchFloodReportService, long usrn, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        var reports = await searchFloodReportService.GetFloodReportsByUsrn(usrn, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<SearchFloodReportPagedResult>, ValidationProblem>> SearchFloodReports(ISearchFloodReportService searchFloodReportService, [FromBody] SearchFloodReportRequest request, int pageNumber = 1, int pageSize = ISearchFloodReportService.DefaultPageSize, CancellationToken ct = default)
    {
        if (!request.HasSearchCriteria)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(request), ["At least one search criterion must be provided."] },
            });
        }

        var reports = await searchFloodReportService.SearchFloodReports(request.ResultsFrom, request.ResultsTo, request.FloodSources, request.Uprn, request.Usrn, request.Postcode, request.Geom, pageNumber, pageSize, ct);
        return TypedResults.Ok(reports);
    }
}

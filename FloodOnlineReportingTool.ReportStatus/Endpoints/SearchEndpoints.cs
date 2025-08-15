using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyList<Report>>, NotFound, BadRequest>> GetReportsByUPRN(ISearchService searchService, Double Uprn, CancellationToken ct)
    {
        var reports = await searchService.GetReportsByUPRN(Uprn, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<IReadOnlyList<Report>>, NotFound, BadRequest>> GetReportsByCoordinates(ISearchService searchService, Double Easting, Double Northing, CancellationToken ct)
    {
        var reports = await searchService.GetReportsByCoordinates(Easting, Northing, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<IReadOnlyList<Report>>, NotFound, BadRequest>> GetReportsByStartDate(ISearchService searchService, DateTimeOffset ImpactStartUtc, CancellationToken ct)
    {
        var reports = await searchService.GetReportsByStartDate(ImpactStartUtc, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }
}

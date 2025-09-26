using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class SearchEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetFloodReportsByUPRN(ISearchService searchService, long uprn, CancellationToken ct)
    {
        var reports = await searchService.GetFloodReportsByUPRN(uprn, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetReportsByCoordinates(ISearchService searchService, double easting, double northing, CancellationToken ct)
    {
        var reports = await searchService.GetFloodReportsByCoordinates(easting, northing, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetReportsByStartDate(ISearchService searchService, DateTimeOffset ImpactStartUtc, CancellationToken ct)
    {
        var reports = await searchService.GetFloodReportsByStartDate(ImpactStartUtc, ct).ConfigureAwait(false);
        return TypedResults.Ok(reports);
    }
}

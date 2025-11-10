using FloodOnlineReportingTool.ReportStatus.Models.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class FloodReportExtensions
{
    internal static IQueryable<SearchResultFloodReport> ToSearchResult(this IQueryable<FloodReport> floodReportQuery)
    {
        return floodReportQuery.Select(fr => new SearchResultFloodReport(
            fr.Id,
            fr.CreatedUtc,
            fr.UpdatedUtc,
            fr.Easting,
            fr.Northing,
            fr.ImpactStartUtc,
            fr.Sources.ToSearchResult()
        ));
    }
}

using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodRoadFloodReportExtensions
{
    public static SearchResultFloodRoadFloodReport ToSearchResult(this FloodRoadFloodReport floodRoadFloodReport)
    {
        return new(
            floodRoadFloodReport.FloodReportId,
            floodRoadFloodReport.CreatedUtc,
            floodRoadFloodReport.UpdatedUtc,
            floodRoadFloodReport.ConfirmedUtc,
            floodRoadFloodReport.ConfirmedUserName
        );
    }

    public static IQueryable<SearchResultFloodRoadFloodReport> ToSearchResult(this IQueryable<FloodRoadFloodReport> floodRoadFloodReportQuery)
    {
        return floodRoadFloodReportQuery.Select(fr => fr.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodRoadFloodReport> ToSearchResults(this ICollection<FloodRoadFloodReport> floodRoadFloodReports)
    {
        return [.. floodRoadFloodReports.Select(frfr => frfr.ToSearchResult())];
    }
}

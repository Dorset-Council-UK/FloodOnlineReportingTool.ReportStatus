using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match fFloodReportSourceer structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match fFloodReportSourceer structure

public static class FloodPropertyFloodReportExtensions
{
    public static SearchResultFloodPropertyFloodReport ToSearchResult(this FloodPropertyFloodReport floodPropertyFloodReport)
    {
        return new(
            floodPropertyFloodReport.FloodReportId,
            floodPropertyFloodReport.CreatedUtc,
            floodPropertyFloodReport.UpdatedUtc,
            floodPropertyFloodReport.ConfirmedUtc,
            floodPropertyFloodReport.ConfirmedUserName
        );
    }

    public static IQueryable<SearchResultFloodPropertyFloodReport> ToSearchResult(this IQueryable<FloodPropertyFloodReport> floodPropertyFloodReportQuery)
    {
        return floodPropertyFloodReportQuery.Select(fr => fr.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodPropertyFloodReport> ToSearchResults(this ICollection<FloodPropertyFloodReport> floodPropertyFloodReports)
    {
        return [.. floodPropertyFloodReports.Select(fpfr => fpfr.ToSearchResult())];
    }
}

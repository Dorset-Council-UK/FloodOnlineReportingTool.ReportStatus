using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodLocationFloodReportExtensions
{
    public static SearchResultFloodLocationFloodReport ToSearchResult(this FloodLocationFloodReport floodLocationFloodReport)
    {
        return new(
            floodLocationFloodReport.FloodReportId,
            floodLocationFloodReport.CreatedUtc,
            floodLocationFloodReport.UpdatedUtc,
            floodLocationFloodReport.ConfirmedUtc,
            floodLocationFloodReport.ConfirmedUserName
        );
    }

    public static IReadOnlyCollection<SearchResultFloodLocationFloodReport> ToSearchResults(this ICollection<FloodLocationFloodReport> floodLocationFloodReports)
    {
        return [.. floodLocationFloodReports.Select(flfr => flfr.ToSearchResult())];
    }
}

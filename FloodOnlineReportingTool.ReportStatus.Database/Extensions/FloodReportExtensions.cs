using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodReportExtensions
{
    public static FloodReportDto ToDto(this FloodReport floodReport)
    {
        return new()
        {
            EligibilityCheckId = floodReport.EligibilityCheckId,
            Uprn = floodReport.Uprn,
            Usrn = floodReport.Usrn,
            Easting = floodReport.Easting,
            Northing = floodReport.Northing,
            ImpactStartUtc = floodReport.ImpactStartUtc,
            ImpactDurationHours = floodReport.ImpactDurationHours,
        };
    }

    public static SearchResultFloodReport ToSearchResult(this FloodReport floodReport)
    {
        return new(
            floodReport.Id,
            floodReport.CreatedUtc,
            floodReport.UpdatedUtc,
            floodReport.Easting,
            floodReport.Northing,
            floodReport.ImpactStartUtc,
            floodReport.Sources.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodReport> ToSearchResult(this IQueryable<FloodReport> floodReportQuery)
    {
        return floodReportQuery.Select(fr => fr.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodReport> ToSearchResults(this ICollection<FloodReport> floodReports)
    {
        return [.. floodReports.Select(fr => fr.ToSearchResult())];
    }
}

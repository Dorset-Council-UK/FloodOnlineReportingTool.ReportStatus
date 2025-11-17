using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class FloodReportSourceExtensions
{
    internal static IReadOnlyCollection<SearchResultFloodReportSource> ToSearchResult(this ICollection<FloodReportSource> floodReportSources)
    {
        return [.. floodReportSources.Select(frs => new SearchResultFloodReportSource(frs.SourceId, frs.Name))];
    }
}

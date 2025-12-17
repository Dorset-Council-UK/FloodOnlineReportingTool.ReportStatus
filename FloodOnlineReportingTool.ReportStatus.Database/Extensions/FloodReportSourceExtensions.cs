using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodReportSourceExtensions
{
    public static FloodReportSourceDto ToDto(this FloodReportSource floodReportSource)
    {
        return new()
        {
            Name = floodReportSource.Name,
        };
    }

    public static SearchResultFloodReportSource ToSearchResult(this FloodReportSource floodReportSource)
    {
        return new(floodReportSource.SourceId, floodReportSource.Name);
    }

    public static IReadOnlyCollection<SearchResultFloodReportSource> ToSearchResults(this ICollection<FloodReportSource> floodReportSources)
    {
        return [.. floodReportSources.Select(ToSearchResult)];
    }
}

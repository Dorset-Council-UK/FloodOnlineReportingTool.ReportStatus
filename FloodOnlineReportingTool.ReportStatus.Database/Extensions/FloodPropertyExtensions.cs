#pragma warning disable IDE0130 // Namespace does not match fFloodReportSourceer structure
using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match fFloodReportSourceer structure

public static class FloodPropertyExtensions
{
    public static FloodPropertyDto ToDto(this FloodProperty floodProperty)
    {
        return new()
        {
            Title = floodProperty.Title,
            Uprn = floodProperty.Uprn,
            Notes = floodProperty.Notes,
            Geom = floodProperty.Geom,
        };
    }

    public static SearchResultFloodProperty ToSearchResult(this FloodProperty floodProperty)
    {
        return new(
            floodProperty.Id,
            floodProperty.CreatedUtc,
            floodProperty.UpdatedUtc,
            floodProperty.Title,
            floodProperty.Uprn,
            floodProperty.Geom,
            floodProperty.Latitude,
            floodProperty.Longitude,
            floodProperty.FloodReports.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodProperty> ToSearchResult(this IQueryable<FloodProperty> floodPropertyQuery)
    {
        return floodPropertyQuery.Select(fp => fp.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodProperty> ToSearchResults(this ICollection<FloodProperty> floodProperties)
    {
        return [.. floodProperties.Select(fp => fp.ToSearchResult())];
    }
}

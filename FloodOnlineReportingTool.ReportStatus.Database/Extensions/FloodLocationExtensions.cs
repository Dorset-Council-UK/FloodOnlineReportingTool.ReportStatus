using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodLocationExtensions
{
    public static FloodLocationDto ToDto(this FloodLocation floodLocation)
    {
        return new()
        {
            Title = floodLocation.Title,
            Notes = floodLocation.Notes,
            Geom = floodLocation.Geom,
        };
    }

    public static SearchResultFloodLocation ToSearchResult(this FloodLocation floodLocation)
    {
        return new(
            floodLocation.Id,
            floodLocation.CreatedUtc,
            floodLocation.UpdatedUtc,
            floodLocation.Title,
            floodLocation.Geom,
            floodLocation.Latitude,
            floodLocation.Longitude,
            floodLocation.FloodReports.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodLocation> ToSearchResult(this IQueryable<FloodLocation> floodLocationQuery)
    {
        return floodLocationQuery.Select(fr => fr.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodLocation> ToSearchResults(this ICollection<FloodLocation> floodLocations)
    {
        return [.. floodLocations.Select(fr => fr.ToSearchResult())];
    }
}

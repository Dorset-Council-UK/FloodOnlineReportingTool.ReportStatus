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
            Location = floodLocation.Location,
        };
    }

    public static SearchResultFloodLocation ToSearchResult(this FloodLocation floodLocation)
    {
        return new(
            floodLocation.Id,
            floodLocation.CreatedUtc,
            floodLocation.UpdatedUtc,
            floodLocation.Title,
            floodLocation.Location,
            floodLocation.FloodReports.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodLocation> ToSearchResult(this IQueryable<FloodLocation> floodLocationQuery)
    {
        return floodLocationQuery.Select(fr => fr.ToSearchResult());
    }
}

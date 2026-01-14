#pragma warning disable IDE0130 // Namespace does not match folder structure
using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodRoadExtensions
{
    public static FloodRoadDto ToDto(this FloodRoad floodRoad)
    {
        return new()
        {
            Title = floodRoad.Title,
            Usrn = floodRoad.Usrn,
            Notes = floodRoad.Notes,
            Geom = floodRoad.Geom,
        };
    }

    public static SearchResultFloodRoad ToSearchResult(this FloodRoad floodRoad)
    {
        return new(
            floodRoad.Id,
            floodRoad.CreatedUtc,
            floodRoad.UpdatedUtc,
            floodRoad.Title,
            floodRoad.Usrn,
            floodRoad.Geom,
            floodRoad.Latitude,
            floodRoad.Longitude,
            floodRoad.FloodReports.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodRoad> ToSearchResult(this IQueryable<FloodRoad> floodRoadQuery)
    {
        return floodRoadQuery.Select(fr => fr.ToSearchResult());
    }

    public static IReadOnlyCollection<SearchResultFloodRoad> ToSearchResults(this ICollection<FloodRoad> floodRoads)
    {
        return [.. floodRoads.Select(fr => fr.ToSearchResult())];
    }
}

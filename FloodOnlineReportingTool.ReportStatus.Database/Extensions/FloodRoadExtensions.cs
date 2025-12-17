#pragma warning disable IDE0130 // Namespace does not match folder structure
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
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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
}

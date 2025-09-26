#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ModelExtensions
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

    public static FloodPropertyDto ToDto(this FloodProperty floodProperty)
    {
        return new()
        {
            Title = floodProperty.Title,
            Notes = floodProperty.Notes,
            Location = floodProperty.Location,
        };
    }

    public static FloodRoadDto ToDto(this FloodRoad floodRoad)
    {
        return new()
        {
            Title = floodRoad.Title,
            Notes = floodRoad.Notes,
            Location = floodRoad.Location,
        };
    }

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

    public static FloodReportSourceDto ToDto(this FloodReportSource floodReportSource)
    {
        return new()
        {
            Name = floodReportSource.Name,
        };
    }
}

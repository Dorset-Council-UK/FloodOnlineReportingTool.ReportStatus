using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.Contracts;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class ContractsExtensions
{
    internal static FloodReportDto ToFloodReportDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return new()
        {
            EligibilityCheckId = eligibilityCheckCreated.Id,
            Uprn = eligibilityCheckCreated.Uprn,
            Usrn = eligibilityCheckCreated.Usrn,
            Easting = eligibilityCheckCreated.Easting,
            Northing = eligibilityCheckCreated.Northing,
            ImpactStartUtc = eligibilityCheckCreated.ImpactStartUTC,
            ImpactDurationHours = eligibilityCheckCreated.ImpactDurationHours,
        };
    }

    internal static FloodReportSourceDto ToFloodReportSourceDto(this EligibilityCheckFloodSource eligibilityCheckFloodSource)
    {
        return new()
        {
            Name = eligibilityCheckFloodSource.Name,
        };
    }

    internal static FloodPropertyDto? ToFloodPropertyDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        if (eligibilityCheckCreated.Uprn != null)
        {
            return new()
            {
                Title = string.IsNullOrEmpty(locationDescription) ? "No address provided" : locationDescription,
                Uprn = (long)eligibilityCheckCreated.Uprn,
                Notes = "",
                Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
            };
        }

        return null;
    }

    internal static FloodRoadDto? ToFloodRoadDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        if (eligibilityCheckCreated.Usrn != null)
        {
            return new()
            {
                Title = string.IsNullOrEmpty(locationDescription) ? "No road name provided" : locationDescription,
                Usrn = (long)eligibilityCheckCreated.Usrn,
                Notes = "",
                Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
            };
        }
        
        return null;
    }

    internal static FloodLocationDto ToFloodLocationDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        return new()
        {
            Title = string.IsNullOrEmpty(locationDescription) ? "No description provided" : locationDescription,
            Notes = "",
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
        };
    }
}

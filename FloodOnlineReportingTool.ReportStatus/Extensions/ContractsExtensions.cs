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

    // TODO - fix the title bits in here, we need to get them using services potentially
    internal static FloodPropertyDto? ToFloodPropertyDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        if (eligibilityCheckCreated.Uprn != null)
        {
            return new()
            {
                Title = "Get the address string from UPRN",
                Uprn = (long)eligibilityCheckCreated.Uprn,
                Notes = "",
                Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
            };
        }

        return null;
    }

    internal static FloodRoadDto? ToFloodRoadDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        if (eligibilityCheckCreated.Usrn != null)
        {
            return new()
            {
                Title = "Get the road name from USRN",
                Usrn = (long)eligibilityCheckCreated.Usrn,
                Notes = "",
                Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
            };
        }
        
        return null;
    }

    internal static FloodLocationDto ToFloodLocationDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return new()
        {
            Title = "Use the location description passed by the message",
            Notes = "",
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
        };
    }
}

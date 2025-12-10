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
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing) { SRID = 27700 },
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

    internal static FloodPropertyDto ToFloodPropertyDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return eligibilityCheckCreated.ToFloodPropertyDto(eligibilityCheckCreated.LocationDescription ?? "No address provided");
    }
    internal static FloodPropertyDto ToFloodPropertyDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        if (eligibilityCheckCreated.Uprn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Property DTO without a UPRN.");
        }

        return new()
        {
            Title = locationDescription,
            Uprn = eligibilityCheckCreated.Uprn.Value,
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
        };
    }

    internal static FloodRoadDto ToFloodRoadDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return eligibilityCheckCreated.ToFloodRoadDto(eligibilityCheckCreated.LocationDescription ?? "No road name provided");
    }
    internal static FloodRoadDto ToFloodRoadDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        if (eligibilityCheckCreated.Usrn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Road DTO without a USRN.");
        }

        return new()
        {
            Title = locationDescription,
            Usrn = eligibilityCheckCreated.Usrn.Value,
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
        };
    }

    internal static FloodLocationDto ToFloodLocationDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return eligibilityCheckCreated.ToFloodLocationDto(eligibilityCheckCreated.LocationDescription ?? "No description provided");
    }
    internal static FloodLocationDto ToFloodLocationDto(this EligibilityCheckCreated eligibilityCheckCreated, string locationDescription)
    {
        return new()
        {
            Title = locationDescription,
            Location = new Point(eligibilityCheckCreated.Easting, eligibilityCheckCreated.Northing),
        };
    }
}

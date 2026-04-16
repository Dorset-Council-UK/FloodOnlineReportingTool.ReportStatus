using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

#pragma warning disable IDE0130 // Namespace does not match fFloodReportSourceer structure
namespace FloodOnlineReportingTool.Contracts;
#pragma warning restore IDE0130 // Namespace does not match fFloodReportSourceer structure

internal static class ContractsExtensions
{
    //internal static FloodReportDto ToFloodReportDto(this FloodReportSourceCreated floodReportSourceCreated)
    //{
    //    return new()
    //    {
    //        EligibilityCheckId = floodReportSourceCreated.Id,
    //        Uprn = floodReportSourceCreated.Uprn,
    //        Usrn = floodReportSourceCreated.Usrn,
    //        Geom = new Point(floodReportSourceCreated.Easting, floodReportSourceCreated.Northing) { SRID = 27700 },
    //        ImpactStartUtc = floodReportSourceCreated.ImpactStartUTC,
    //        ImpactDurationHours = floodReportSourceCreated.ImpactDurationHours,
    //    };
    //}

    internal static FloodReportNameDto ToFloodReportSourceDto(this EligibilityCheckFloodSource eligibilityCheckFloodSource)
    {
        return new()
        {
            Name = eligibilityCheckFloodSource.Name,
        };
    }

    //internal static FloodPropertyDto ToFloodPropertyDto(this FloodReportSourceCreated floodReportSourceCreated)
    //{
    //    return floodReportSourceCreated.EligibilityCheckRecord.ToFloodPropertyDto(floodReportSourceCreated.EligibilityCheckRecord.LocationDescription ?? "No address provided");
    //}
    internal static FloodPropertyDto ToFloodPropertyDto(this FloodReportSourceCreated floodReportSourceCreated, string locationDescription)
    {
        if (floodReportSourceCreated.EligibilityCheckRecord.Uprn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Property DTO without a UPRN.");
        }

        return new()
        {
            Title = locationDescription,
            Uprn = floodReportSourceCreated.EligibilityCheckRecord.Uprn.Value,
            Geom = new Point(floodReportSourceCreated.EligibilityCheckRecord.Easting, floodReportSourceCreated.EligibilityCheckRecord.Northing),
        };
    }

    internal static FloodRoadDto ToFloodRoadDto(this FloodReportSourceCreated floodReportSourceCreated)
    {
        return floodReportSourceCreated.ToFloodRoadDto(floodReportSourceCreated.EligibilityCheckRecord.LocationDescription ?? "No road name provided");
    }
    internal static FloodRoadDto ToFloodRoadDto(this FloodReportSourceCreated floodReportSourceCreated, string locationDescription)
    {
        if (floodReportSourceCreated.EligibilityCheckRecord.Usrn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Road DTO without a USRN.");
        }

        return new()
        {
            Title = locationDescription,
            Usrn = floodReportSourceCreated.EligibilityCheckRecord.Usrn.Value,
            Geom = new Point(floodReportSourceCreated.EligibilityCheckRecord.Easting, floodReportSourceCreated.EligibilityCheckRecord.Northing),
        };
    }

    internal static FloodLocationDto ToFloodLocationDto(this FloodReportSourceCreated floodReportSourceCreated)
    {
        return floodReportSourceCreated.ToFloodLocationDto(floodReportSourceCreated.EligibilityCheckRecord.LocationDescription ?? "No description provided");
    }
    internal static FloodLocationDto ToFloodLocationDto(this FloodReportSourceCreated floodReportSourceCreated, string locationDescription)
    {
        return new()
        {
            Title = locationDescription,
            Geom = new Point(floodReportSourceCreated.EligibilityCheckRecord.Easting, floodReportSourceCreated.EligibilityCheckRecord.Northing),
        };
    }
}

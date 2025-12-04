using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodLocationRepository(ApplicationDbContext context) : IFloodLocationRepository
{
    public IQueryable<FloodLocation> GetFloodLocations()
    {
        return context.FloodLocations
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fl => fl.Id);
    }

    public IQueryable<FloodLocation> GetFloodLocationsWithReports()
    {
        return context.FloodLocations
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .OrderBy(fl => fl.Id)
            .Include(fl => fl.FloodReports.OrderBy(fr => fr.FloodReportId));
    }

    public IQueryable<FloodLocation> GetFloodLocationsByPoint(double easting, double northing, double buffer)
    {
        return context.FloodLocations
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(fl => fl.Location.Intersects(new Point(easting, northing).Buffer(buffer)))
            .OrderBy(fl => fl.Id)
            .Include(fl => fl.FloodReports.OrderBy(flfr => flfr.FloodReportId));
    }
}

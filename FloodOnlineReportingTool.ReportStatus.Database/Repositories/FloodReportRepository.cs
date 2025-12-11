using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public class FloodReportRepository(ApplicationDbContext context) : IFloodReportRepository
{
    public IQueryable<FloodReport> GetFloodReports()
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport?> GetFloodReportById(Guid id)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId))
            .Where(fr => fr.Id.Equals(id));
    }

    public IQueryable<FloodReport> GetFloodReportsWithinMetres(double easting, double northing, double metres)
    {
        var searchPoint = new Point(easting, northing) { SRID = 27700 };

        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId))
            .Where(fr => fr.Geom.IsWithinDistance(searchPoint, metres))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport> GetFloodReportsByUprn(long uprn)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId))
            .Where(fr => fr.Uprn.Equals(uprn))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport> GetFloodReportsByUsrn(long usrn)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId))
            .Where(fr => fr.Usrn.Equals(usrn))
            .OrderBy(fr => fr.Id);
    }
}

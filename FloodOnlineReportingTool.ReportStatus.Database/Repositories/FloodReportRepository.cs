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

    public IQueryable<FloodReport> SearchFloodReports(DateTimeOffset? resultsFrom, DateTimeOffset? resultsTo, ICollection<Guid> floodSources, long? uprn, long? usrn, MultiPolygon? geom)
    {
        IQueryable<FloodReport> query = context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.Sources.OrderBy(frs => frs.SourceId));

        if (resultsFrom.HasValue)
        {
            query = query.Where(fr => fr.ImpactStartUtc.HasValue &&
                fr.ImpactStartUtc.Value.AddHours(fr.ImpactDurationHours) >= resultsFrom.Value);
        }

        if (resultsTo.HasValue)
        {
            query = query.Where(fr => fr.ImpactStartUtc.HasValue &&
                fr.ImpactStartUtc.Value <= resultsTo.Value);
        }

        if (floodSources.Count > 0)
        {
            query = query.Where(fr => fr.Sources.Any(s => floodSources.Contains(s.SourceId)));
        }

        if (uprn.HasValue)
        {
            query = query.Where(fr => fr.Uprn.Equals(uprn.Value));
        }

        if (usrn.HasValue)
        {
            query = query.Where(fr => fr.Usrn.Equals(usrn.Value));
        }

        if (geom is not null)
        {
            geom.SRID = 27700;
            query = query.Where(fr => fr.Geom.Within(geom));
        }

        return query.OrderBy(fr => fr.Id);
    }
}

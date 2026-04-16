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
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport?> GetFloodReportByReference(string reference)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(fr => fr.SourceRecords.Any( sr => sr.Reference.Equals(reference)))
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id));
    }

    public IQueryable<FloodReport?> GetFloodReportById(Guid id)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id))
            .Where(fr => fr.Id.Equals(id));
    }

    public IQueryable<FloodReport> GetFloodReportsWithinMetres(double easting, double northing, double metres)
    {
        var searchPoint = new Point(easting, northing) { SRID = 27700 };

        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(fr => fr.SourceRecords.Any(sr => sr.Geom.IsWithinDistance(searchPoint, metres)))
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport> GetFloodReportsByUprn(long uprn)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(fr => fr.SourceRecords.Any(sr => sr.Uprn.Equals(uprn)))
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport> GetFloodReportsByUsrn(long usrn)
    {
        return context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Where(fr => fr.SourceRecords.Any(sr => sr.Usrn.Equals(usrn)))
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id))
            .OrderBy(fr => fr.Id);
    }

    public IQueryable<FloodReport> SearchFloodReports(DateTimeOffset? resultsFrom,
                                                      DateTimeOffset? resultsTo,
                                                      ICollection<Guid> floodSources,
                                                      long? uprn,
                                                      long? usrn,
                                                      string? postcode,
                                                      MultiPolygon? geom)
    {
        IQueryable<FloodReport> query = context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Include(fr => fr.SourceRecords.OrderBy(frs => frs.Id));

        if (resultsFrom.HasValue)
        {
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.ImpactStartUtc.HasValue &&
                sr.ImpactStartUtc.Value.AddHours(sr.ImpactDurationHours ?? 1) >= resultsFrom.Value));
        }

        if (resultsTo.HasValue)
        {
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.ImpactStartUtc.HasValue &&
                sr.ImpactStartUtc.Value <= resultsTo.Value));
        }

        if (floodSources.Count > 0)
        {
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.FloodCauses.Any(s => floodSources.Contains(s.CauseId))));
        }

        if (uprn.HasValue)
        {
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.Uprn.Equals(uprn.Value)));
        }

        if (usrn.HasValue)
        {
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.Usrn.Equals(usrn.Value)));
        }

        if (!string.IsNullOrWhiteSpace(postcode))
        {
            //TODO implement this (for now, use a placehFloodReportSourceer that returns all results)
            query = query.Where(fr => true);
        }

        if (geom is not null)
        {
            geom.SRID = 27700;
            query = query.Where(fr => fr.SourceRecords.Any(sr => sr.Geom.Within(geom)));
        }

        return query.OrderBy(fr => fr.Id);
    }
}

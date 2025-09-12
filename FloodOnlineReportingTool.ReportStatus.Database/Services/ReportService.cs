using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class ReportService(ILogger<ReportService> logger, ApplicationDbContext context) : IReportService
{
    public async Task<IReadOnlyCollection<Report>> GetReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all reports from the database.");

        return await context.Reports
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<Report?> GetReportById(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving report with ID {ReportId} from the database.", id);

        var report = await context.Reports
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        return report;
    }

    public async Task<IReadOnlyCollection<Report>> GetReportsByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Finding reports with UPRN {Uprn} from the database.", uprn);

        return await context.Reports
            .AsNoTracking()
            .Where(o => o.Uprn.Equals(uprn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<Report>> GetReportsByUsrn(long usrn, CancellationToken ct)
    {
        logger.LogDebug("Finding reports with USRN {Usrn} from the database.", usrn);

        return await context.Reports
            .AsNoTracking()
            .Where(o => o.Usrn.Equals(usrn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<Report>> GetReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct)
    {
        logger.LogDebug("Finding report within {Metres} metres of Easting {Easting} and Northing {Northing}.", metres, easting, northing);

        var metresSquared = metres * metres;
        return await context.Reports
            .AsNoTracking()
            .Where(o =>
                Math.Pow(o.Easting - easting, 2) + Math.Pow(o.Northing - northing, 2) <= metresSquared
            )
            .ToListAsync(ct)
            .ConfigureAwait(false);

        //return await context.Reports
        //    .AsNoTracking()
        //    .Where(o =>
        //        Math.Sqrt(
        //            Math.Pow(o.Easting - easting, 2) +
        //            Math.Pow(o.Northing - northing, 2)
        //        ) <= metres
        //    )
        //    .ToListAsync(ct)
        //    .ConfigureAwait(false);
    }

    public async Task<Report> CreateReport(ReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new report with EligibilityCheckId {EligibilityCheckId}.", dto.EligibilityCheckId);

        var newReport = new Report()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            EligibilityCheckId = dto.EligibilityCheckId,
            Uprn = dto.Uprn,
            Usrn = dto.Usrn,
            Easting = dto.Easting,
            Northing = dto.Northing,
            ImpactStartUtc = dto.ImpactStartUtc,
            ImpactDurationHours = dto.ImpactDurationHours,
        };

        context.Reports.Add(newReport);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newReport;
    }

    public async Task<Report?> UpdateReport(Guid id, ReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating report with ID {ReportId} in the database.", id);

        var existing = await context.Reports
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            EligibilityCheckId = dto.EligibilityCheckId,
            Uprn = dto.Uprn,
            Usrn = dto.Usrn,
            Easting = dto.Easting,
            Northing = dto.Northing,
            ImpactStartUtc = dto.ImpactStartUtc,
            ImpactDurationHours = dto.ImpactDurationHours,
        };

        context.Reports.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete report with ID {ReportId} from the database.", id);

        var existing = await context.Reports
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.Reports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodReportService(ILogger<FloodReportService> logger, ApplicationDbContext context) : IFloodReportService
{
    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood reports from the database.");

        return await context.FloodReports
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodReport?> GetFloodReportById(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report with ID {FloodReportId} from the database.", id);

        return await context.FloodReports
            .FindAsync([id], ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with UPRN {Uprn} from the database.", uprn);

        return await context.FloodReports
            .AsNoTracking()
            .Where(o => o.Uprn.Equals(uprn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUsrn(long usrn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with USRN {Usrn} from the database.", usrn);

        return await context.FloodReports
            .AsNoTracking()
            .Where(o => o.Usrn.Equals(usrn))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports within {Metres} metres of Easting {Easting} and Northing {Northing}.", metres, easting, northing);

        var metresSquared = metres * metres;
        return await context.FloodReports
            .AsNoTracking()
            .Where(o =>
                Math.Pow(o.Easting - easting, 2) + Math.Pow(o.Northing - northing, 2) <= metresSquared
            )
            .ToListAsync(ct)
            .ConfigureAwait(false);

        // TODO Test the performance of the above against the below, I believe the above is more efficient as it avoids a square root calculation
        //return await context.FloodReports
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

    public async Task<FloodReport> CreateFloodReport(FloodReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood report with EligibilityCheckId {EligibilityCheckId}.", dto.EligibilityCheckId);

        var creating = new FloodReport()
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

        context.FloodReports.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodReport?> UpdateFloodReport(Guid id, FloodReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood report with ID {FloodReportId} in the database.", id);

        var existing = await context.FloodReports
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id.Equals(id), ct)
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

        context.FloodReports.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report with ID {FloodReportId} from the database.", id);

        var existing = await context.FloodReports
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodReportExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report with ID {FloodReportId} exists in the database.", id);

        return await context.FloodReports
            .AsNoTracking()
            .AnyAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }
}

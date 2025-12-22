using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodReportService(ILogger<FloodReportService> logger, IFloodReportRepository floodReportRepository, ApplicationDbContext context) : IFloodReportService
{
    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood reports from the database.");

        var query = floodReportRepository.GetFloodReports();
        return await query.ToListAsync(ct);
    }

    public async Task<FloodReport?> GetFloodReportById(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood report with ID {FloodReportId} from the database.", id);

        var query = floodReportRepository.GetFloodReportById(id);
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with UPRN {Uprn} from the database.", uprn);

        var query = floodReportRepository.GetFloodReportsByUprn(uprn);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUsrn(long usrn, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports with USRN {Usrn} from the database.", usrn);

        var query = floodReportRepository.GetFloodReportsByUsrn(usrn);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct)
    {
        logger.LogDebug("Finding flood reports within {Metres} metres of Easting {Easting} and Northing {Northing}.", metres, easting, northing);

        metres = Math.Clamp(metres, 0, IFloodReportService.MaxMetres);

        return await floodReportRepository
            .GetFloodReportsWithinMetres(easting, northing, metres)
            .ToListAsync(ct);
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
            Geom = dto.Geom,
            ImpactStartUtc = dto.ImpactStartUtc,
            ImpactDurationHours = dto.ImpactDurationHours,
        };

        context.FloodReports.Add(creating);
        await context.SaveChangesAsync(ct);

        return creating;
    }

    public async Task<FloodReport?> UpdateFloodReport(Guid id, FloodReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood report with ID {FloodReportId} in the database.", id);

        var existing = await floodReportRepository
            .GetFloodReports()
            .FirstOrDefaultAsync(fr => fr.Id.Equals(id), ct);

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
            Geom = dto.Geom,
            ImpactStartUtc = dto.ImpactStartUtc,
            ImpactDurationHours = dto.ImpactDurationHours,
        };

        context.FloodReports.Update(updated);
        await context.SaveChangesAsync(ct);

        return updated;
    }

    public async Task<bool> DeleteFloodReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood report with ID {FloodReportId} from the database.", id);

        var existing = await context.FloodReports.FindAsync([id], ct);

        if (existing == null)
        {
            return false;
        }

        context.FloodReports.Remove(existing);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0;
    }

    public async Task<bool> FloodReportExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood report with ID {FloodReportId} exists in the database.", id);

        return await context.FloodReports
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.Id.Equals(id), ct);
    }
}

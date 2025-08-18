using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using MassTransit;
using MassTransit.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class ReportService(
    ILogger<ReportService> logger,
    ApplicationDbContext context,
    IPublishEndpoint publishEndpoint
) : IReportService
{
    public async Task<IReadOnlyList<Report>> GetReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all reports from the database.");

        return await context.Reports
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<Report?> GetReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving report with ID {ReportId} from the database.", id);

        var report = await context.Reports
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        return report;
    }

    public async Task<Guid?> GetReport(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        //logger.LogDebug("Retrieving report with ID {ReportId} from the database.", id);

        // Matching UPRN
        var reportId = await context.Reports
            .Where(o => o.Uprn == eligibilityCheckCreated.Uprn)
            .Select(o => (Guid?)o.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (reportId != null)
        {
            return reportId;
        }

        // Within 50 metre buffer
        reportId = await context.Reports
            .Where(o =>
                Math.Sqrt(
                    Math.Pow(o.Easting - eligibilityCheckCreated.Easting, 2) +
                    Math.Pow(o.Northing - eligibilityCheckCreated.Northing, 2)
                ) <= 50.0
            )
            .Select(o => (Guid?)o.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (reportId != null)
        {
            return reportId;
        }

        // Flood source is groundwater
        // Flood date is within 14 days
        // return null;

        // Flood source is other
        // Flood date is within 7 days
        // return null;

        return reportId;
    }

    public async Task<Report> CreateReport(ReportDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new report with EligibilityCheckId {EligibilityCheckId} and Uprn {Uprn}.", dto.EligibilityCheckId, dto.Uprn);

        var newReport = new Report()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            EligibilityCheckId = dto.EligibilityCheckId,
            Uprn = dto.Uprn,
            Easting = dto.Easting,
            Northing = dto.Northing,
            ImpactStartUtc = dto.ImpactStartUtc,
            ImpactDurationHours = dto.ImpactDurationHours,
        };

        context.Reports.Add(newReport);
        await publishEndpoint.Publish(newReport.ToMessageCreated(), ct).ConfigureAwait(false);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newReport;
    }

    public async Task<Report> CreateReport(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct)
    {
        return await CreateReport(eligibilityCheckCreated.ToReportDto(), ct);
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

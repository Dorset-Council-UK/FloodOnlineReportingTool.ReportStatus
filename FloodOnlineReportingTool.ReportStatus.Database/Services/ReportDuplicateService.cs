using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class ReportDuplicateService(ILogger<ReportDuplicateService> logger, ApplicationDbContext context) : IReportDuplicateService
{
    public async Task<IReadOnlyList<ReportDuplicate>> GetDuplicateReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all duplicate reports from the database.");

        return await context.ReportDuplicates
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<ReportDuplicate?> GetDuplicateReportById(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving duplicate report with ID {ReportDuplicateId} from the database.", id);

        var duplicate = await context.ReportDuplicates
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        return duplicate;
    }

    public async Task<ReportDuplicate> CreateDuplicateReport(ReportDuplicateDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new duplicate report of report with ID {ReportId} in the database.", dto.ReportId);

        var newDuplicate = new ReportDuplicate()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            ReportId = dto.ReportId,
            EligibilityCheckId = dto.EligibilityCheckId,
            Easting = dto.Easting,
            Northing = dto.Northing,
        };

        context.ReportDuplicates.Add(newDuplicate);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newDuplicate;
    }

    public async Task<ReportDuplicate?> UpdateDuplicateReport(Guid id, ReportDuplicateDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating duplicate report with ID {ReportDuplicateId} in the database.", id);

        var existing = await context.ReportDuplicates
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            ReportId = dto.ReportId,
            EligibilityCheckId = dto.EligibilityCheckId,
            Easting = dto.Easting,
            Northing = dto.Northing,
        };

        context.ReportDuplicates.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteDuplicateReport(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete duplicate report with ID {ReportDuplicateId} from the database.", id);

        var existing = await context.ReportDuplicates
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.ReportDuplicates.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }
}

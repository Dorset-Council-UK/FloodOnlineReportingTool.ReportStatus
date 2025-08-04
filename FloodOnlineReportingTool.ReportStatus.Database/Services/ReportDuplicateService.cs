using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class ReportDuplicateService(ILogger<ReportDuplicateService> logger, ApplicationDbContext context) : IReportDuplicateService
{
    public async Task<IReadOnlyList<ReportDuplicate>> GetDuplicates(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all duplicate reports from the database.");

        return await context.ReportDuplicates
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ReportDuplicate>> GetDuplicates(Guid reportId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all duplicates for a report from the database.");

        return await context.ReportDuplicates
            .AsNoTracking()
            .Where(d => d.ReportId == reportId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<ReportDuplicate?> GetDuplicate(Guid duplicateId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving duplicate report with ID {ReportDuplicateId} from the database.", duplicateId);

        var duplicate = await context.ReportDuplicates
            .FindAsync([duplicateId], ct)
            .ConfigureAwait(false);

        return duplicate;
    }

    public async Task<ReportDuplicate?> GetDuplicate(Guid reportId, Guid duplicateId, CancellationToken ct)
    {
        logger.LogDebug("Retrieving duplicate report with ID {ReportDuplicateId} from the database.", duplicateId);

        var duplicate = await context.ReportDuplicates
            .FirstOrDefaultAsync(o => o.ReportId == reportId && o.Id == duplicateId, ct)
            .ConfigureAwait(false);

        return duplicate;
    }

    public async Task<ReportDuplicate?> CreateDuplicate(Guid reportId, ReportDuplicateDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new duplicate report of report with ID {ReportId} in the database.", reportId);

        var reportExists = await context.Reports
            .AnyAsync(o => o.Id == reportId, ct)
            .ConfigureAwait(false);

        if (!reportExists)
        {
            return null;
        }

        var newDuplicate = new ReportDuplicate()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            ReportId = reportId,
            EligibilityCheckId = dto.EligibilityCheckId,
            Easting = dto.Easting,
            Northing = dto.Northing,
        };

        context.ReportDuplicates.Add(newDuplicate);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return newDuplicate;
    }

    public async Task<ReportDuplicate?> UpdateDuplicate(Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating duplicate report with ID {ReportDuplicateId} in the database.", duplicateId);

        var existing = await context.ReportDuplicates
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == duplicateId, ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            EligibilityCheckId = dto.EligibilityCheckId,
            Easting = dto.Easting,
            Northing = dto.Northing,
        };

        context.ReportDuplicates.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<ReportDuplicate?> UpdateDuplicate(Guid reportId, Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating duplicate report with ID {ReportDuplicateId} in the database.", duplicateId);

        var existing = await context.ReportDuplicates
            .FirstOrDefaultAsync(o => o.ReportId == reportId && o.Id == duplicateId, ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return null;
        }

        var updated = existing with
        {
            UpdatedUtc = DateTimeOffset.UtcNow,

            EligibilityCheckId = dto.EligibilityCheckId,
            Easting = dto.Easting,
            Northing = dto.Northing,
        };

        context.ReportDuplicates.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteDuplicate(Guid duplicateId, CancellationToken ct)
    {
        logger.LogDebug("Delete duplicate report with ID {ReportDuplicateId} from the database.", duplicateId);

        var existing = await context.ReportDuplicates
            .FindAsync([duplicateId], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.ReportDuplicates.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> DeleteDuplicate(Guid reportId, Guid duplicateId, CancellationToken ct)
    {
        logger.LogDebug("Delete duplicate report with ID {ReportDuplicateId} from the database.", duplicateId);

        var existing = await context.ReportDuplicates
            .FirstOrDefaultAsync(o => o.ReportId == reportId && o.Id == duplicateId, ct)
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

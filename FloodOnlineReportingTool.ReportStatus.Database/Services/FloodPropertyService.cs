using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodPropertyService(ILogger<FloodPropertyService> logger, ApplicationDbContext context) : IFloodPropertyService
{
    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodProperties(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties from the database.");

        return await context.FloodProperties
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesByUprn(long Uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties for UPRN {Uprn} from the database.", Uprn);

        return await context.FloodProperties
            .AsNoTracking()
            .Where(o => o.Uprn.Equals(Uprn))
            .Include(o => o.FloodReports)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties and their flood reports from the database.");

        return await context.FloodProperties
            .AsNoTracking()
            .Include(o => o.FloodReports)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodProperty?> GetFloodProperty(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property with ID {FloodPropertyId} from the database.", id);

        return await context.FloodProperties
            .FindAsync([id], ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodProperty?> GetFloodPropertyWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property and its flood reports with ID {FloodPropertyId} from the database.", id);

        return await context.FloodProperties
            .AsNoTracking()
            .Include(o => o.FloodReports)
            .FirstOrDefaultAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }

    public async Task<FloodProperty> CreateFloodProperty(FloodPropertyDto dto, CancellationToken ct)
    {
        logger.LogDebug("Creating a new flood property in the database");

        var creating = new FloodProperty()
        {
            Id = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,

            Title = dto.Title,
            Uprn = dto.Uprn,
            Notes = dto.Notes,
            Location = dto.Location,
        };

        context.FloodProperties.Add(creating);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return creating;
    }

    public async Task<FloodProperty?> UpdateFloodProperty(Guid id, FloodPropertyDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood property with ID {FloodPropertyId} in the database.", id);

        var existing = await context.FloodProperties
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

            Title = dto.Title,
            Notes = dto.Notes,
            Location = dto.Location,
        };

        context.FloodProperties.Update(updated);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return updated;
    }

    public async Task<bool> DeleteFloodProperty(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Delete flood property with ID {FloodPropertyId} from the database.", id);

        var existing = await context.FloodProperties
            .FindAsync([id], ct)
            .ConfigureAwait(false);

        if (existing == null)
        {
            return false;
        }

        context.FloodProperties.Remove(existing);
        var changes = await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return changes > 0;
    }

    public async Task<bool> FloodPropertyExists(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Checking if flood property with ID {FloodPropertyId} exists in the database.", id);

        return await context.FloodProperties
            .AsNoTracking()
            .AnyAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }
}

using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodPropertyService(ILogger<FloodPropertyService> logger, ApplicationDbContext context) : IFloodPropertyService
{
    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodProperties(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties from the database.");

        return await context.FloodProperties
            .AsNoTracking()
            .OrderBy(fp => fp.Id)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesByUprn(long Uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties for UPRN {Uprn} from the database.", Uprn);

        return await context.FloodProperties
            .AsNoTracking()
            .Where(fp => fp.Uprn.Equals(Uprn))
            .OrderBy(fp => fp.Id)
            .Include(fp => fp.FloodReports.OrderBy(fr => fr.FloodReportId))
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties and their flood reports from the database.");

        return await context.FloodProperties
            .AsNoTracking()
            .OrderBy(fp => fp.Id)
            .Include(fp => fp.FloodReports.OrderBy(fr => fr.FloodReportId))
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
            .OrderBy(fp => fp.Id)
            .Include(fp => fp.FloodReports.OrderBy(fr => fr.FloodReportId))
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct)
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

    public async Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, CancellationToken ct)
    {
        if (ecc.Uprn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Property without a UPRN.");
        }

        var createdUtc = DateTimeOffset.UtcNow;
        var floodReportId = Guid.CreateVersion7();
        var uprn = ecc.Uprn.Value;

        // Create the flood report sources
        var newSources = ecc.FloodSources.Select(fs => new FloodReportSource()
        {
            FloodReportId = floodReportId,
            SourceId = fs.Id,
            CreatedUtc = createdUtc,
            Name = fs.Name,
        });
        ImmutableHashSet<Guid> floodReportSourceIds = [.. ecc.FloodSources.Select(s => s.Id)];

        // Create flood report and sources
        var floodReport = new FloodReport()
        {
            Id = floodReportId,
            CreatedUtc = createdUtc,

            EligibilityCheckId = ecc.Id,
            Uprn = uprn,
            Usrn = ecc.Usrn,
            Easting = ecc.Easting,
            Northing = ecc.Northing,
            ImpactStartUtc = ecc.ImpactStartUTC,
            ImpactDurationHours = ecc.ImpactDurationHours,

            Sources = [.. newSources],
        };

        // Similar to FloodPropertyService.GetFloodPropertiesByUprn but with specific changes for this use case. Get less data etc.

        // Flood property with matching UPRN query, only selecting what we need
        var floodPropertiesQuery = context.FloodProperties
            .AsNoTracking()
            .Where(fp => fp.Uprn.Equals(uprn))
            .OrderBy(fp => fp.Id)
            .Select(fp => new
            {
                fp.Id,
                fp.FloodReports,
            });

        // Flood property > Flood report query, only selecting what we need
        var floodReportsQuery = floodPropertiesQuery
            .SelectMany(fp => fp.FloodReports)
            .OrderBy(fpfr => fpfr.FloodReportId)
            .Select(fpfr => new
            {
                fpfr.FloodReportId,
                fpfr.FloodReport.ImpactStartUtc,
                fpfr.FloodReport.ImpactDurationHours,
                fpfr.FloodReport.Sources,
            });

        // Flood report > Flood report source query, only selecting what we need
        var floodReportSourcesQuery = floodReportsQuery
            .SelectMany(fr => fr.Sources)
            .OrderBy(frs => frs.SourceId)
            .Select(frs => new
            {
                frs.SourceId,
                frs.Name,
            });

        var floodProperties = await floodPropertiesQuery
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var floodReports = await floodReportsQuery
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var floodReportSources = await floodReportSourcesQuery
            .ToListAsync(ct)
            .ConfigureAwait(false);


        //var existingSources = await context.FloodProperties
        //  .AsNoTracking()
        //  .Where(fp => fp.Uprn.Equals(uprn))
        //  .OrderBy(fp => fp.Id)
        //  .Include(fp => fp.FloodReports) // FloodPropertyFloodReport
        //    .ThenInclude(fpr => fpr.FloodReport) // FloodReport
        //        .ThenInclude(fr => fr.Sources) // FloodReportSource
        //  .ToListAsync(ct)
        //  .ConfigureAwait(false);
        //var existingSources = await context.FloodProperties
        //  .AsNoTracking()
        //  .Where(fp => fp.Uprn.Equals(uprn))
        //  .OrderBy(fp => fp.Id)
        //  .Select(fp => new
        //  {
        //      FloodPropertyId = fp.Id,
        //      FloodReports = fp.FloodReports.Select(fpfr => new
        //      {
        //          fpfr.FloodReportId,
        //          fpfr.FloodReport.ImpactStartUtc,
        //          fpfr.FloodReport.ImpactDurationHours,
        //          Sources = fpfr.FloodReport.Sources.Select(src => new
        //          {
        //              src.SourceId,
        //              src.Name,
        //          }).ToList(),
        //      }).ToList(),
        //  })
        //  .ToListAsync(ct)
        //  .ConfigureAwait(false);

        // HasMatchingSources Sources FloodReportSource.SourceId's, FloodSources EligibilityCheckFloodSource.Id's
        //ImmutableHashSet<Guid> floodPropertyFloodReportsSourceIds = [.. floodPropertyFloodReports.];
        // WasReportedWithinDays FloodReport and EligibilityCheckCreated ImpactStartUtc, and ImpactDurationHours

        //using var transaction = await context.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
        //try
        //{
        //    context.FloodReports.Add(floodReport);
        //    await context.SaveChangesAsync(ct).ConfigureAwait(false);
        //    await transaction.CommitAsync(ct).ConfigureAwait(false);
        //}
        //catch
        //{
        //    await transaction.RollbackAsync(ct).ConfigureAwait(false);
        //    throw;
        //}
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
            .IgnoreAutoIncludes()
            .AnyAsync(o => o.Id.Equals(id), ct)
            .ConfigureAwait(false);
    }
}

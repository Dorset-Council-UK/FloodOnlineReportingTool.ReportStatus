using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Collections.Immutable;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public class FloodPropertyService(ILogger<FloodPropertyService> logger, ApplicationDbContext context, IFloodPropertyRepository floodPropertyRepositiry) : IFloodPropertyService
{
    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodProperties(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties from the database.");

        return await floodPropertyRepositiry
            .GetFloodProperties()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesByUprn(long uprn, CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties for UPRN {Uprn} from the database.", uprn);

        return await floodPropertyRepositiry
            .GetFloodProperties()
            .Where(fp => fp.Uprn.Equals(uprn))
            .OrderBy(fp => fp.Id)
            .Include(fp => fp.FloodReports.OrderBy(fr => fr.FloodReportId))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesWithReports(CancellationToken ct)
    {
        logger.LogDebug("Retrieving all flood properties and their flood reports from the database.");

        return await floodPropertyRepositiry
            .GetFloodPropertiesWithReports()
            .ToListAsync(ct);
    }

    public async Task<FloodProperty?> GetFloodProperty(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property with ID {FloodPropertyId} from the database.", id);

        return await floodPropertyRepositiry
            .GetFloodProperties()
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct);
    }

    public async Task<FloodProperty?> GetFloodPropertyWithReports(Guid id, CancellationToken ct)
    {
        logger.LogDebug("Retrieving flood property and its flood reports with ID {FloodPropertyId} from the database.", id);

        var property = await floodPropertyRepositiry
            .GetFloodPropertiesWithReports()
            .FirstOrDefaultAsync(fp => fp.Id.Equals(id), ct);

        return property;
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

    public async Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, string locationDescription, CancellationToken ct)
    {
        if (ecc.Uprn is null)
        {
            throw new InvalidOperationException("Cannot create Flood Property without a UPRN.");
        }

        var createdUtc = DateTimeOffset.UtcNow;
        var floodReportId = Guid.CreateVersion7();
        var uprn = ecc.Uprn.Value;

        // Build the flood report and sources
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

            Sources = [.. ecc.FloodSources.Select(ecfs =>
                new FloodReportSource()
                {
                    FloodReportId = floodReportId,
                    SourceId = ecfs.Id,
                    CreatedUtc = createdUtc,
                    Name = ecfs.Name,
                }),
            ],
        };

        var potentialDuplicates = await FindPotentialDuplicates(uprn, ecc, ct).ConfigureAwait(false);

        FloodProperty? floodProperty = null;
        ICollection<FloodPropertyFloodReport> floodPropertyFloodReports = [];
        if (potentialDuplicates.Count > 0)
        {
            foreach (var duplicateFloodPropertyId in potentialDuplicates)
            {
                floodPropertyFloodReports.Add(new FloodPropertyFloodReport()
                {
                    FloodPropertyId = duplicateFloodPropertyId,
                    FloodReportId = floodReportId,
                    CreatedUtc = createdUtc,
                });
            }
        }
        else
        {
            var floodPropertyId = Guid.CreateVersion7();
            floodProperty = new FloodProperty()
            {
                Id = floodPropertyId,
                CreatedUtc = createdUtc,

                Title = locationDescription,
                Uprn = uprn,
                Notes = null,
                Location = new Point(ecc.Easting, ecc.Northing),

                FloodReports = [
                    new()
                    {
                        FloodPropertyId = floodPropertyId,
                        FloodReportId = floodReportId,
                        CreatedUtc = createdUtc,
                    },
                ],
            };
        }

        context.FloodReports.Add(floodReport);
        if (floodProperty is not null) context.FloodProperties.Add(floodProperty);
        if (floodPropertyFloodReports.Count > 0) context.FloodPropertyReports.AddRange(floodPropertyFloodReports);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task<FloodProperty?> UpdateFloodProperty(Guid id, FloodPropertyDto dto, CancellationToken ct)
    {
        logger.LogDebug("Updating flood property with ID {FloodPropertyId} in the database.", id);

        var existing = await floodPropertyRepositiry
            .GetFloodProperties()
            .FirstOrDefaultAsync(o => o.Id.Equals(id), ct);

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

    /// <summary>
    /// Searches for existing flood reports associated with a property (by UPRN) that are considered potential duplicates of a new flood report.
    /// A flood report is considered a potential duplicate if it shares at least one flood source (by <c>SourceId</c>) with the new flood report
    /// and its impact period overlaps with the new flood report's impact period (with a buffer).
    /// </summary>
    /// <param name="uprn">The UPRN of the property to search for existing flood reports.</param>
    /// <param name="ecc">The <see cref="EligibilityCheckCreated"/> message containing details of the new flood report.</param>
    /// <returns>
    /// A read-only collection of Flood property <see cref="Guid"/>'s that are potential duplicates based on source and date overlap criteria.
    /// </returns>
    private async Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(long uprn, EligibilityCheckCreated ecc, CancellationToken ct)
    {
        ImmutableHashSet<Guid> newSourceIds = [.. ecc.FloodSources.Select(s => s.Id)];
        var isGroundWaterSource = newSourceIds.Contains(PrimaryCauseIds.WaterRisingOutOfTheGround);
        var bufferDays = isGroundWaterSource ? 14 : 7;
        var incomingStart = ecc.ImpactStartUTC?.AddDays(-bufferDays);
        var incomingEnd = ecc.IsOnGoing ? DateTimeOffset.UtcNow : ecc.ImpactStartUTC?.AddHours(ecc.ImpactDurationHours);

        return await context.FloodProperties
            .AsNoTracking()
            .Where(fp => fp.Uprn.Equals(uprn))
            .SelectMany(fp => fp.FloodReports
                .Where(fpfr =>
                    // At least one matching source
                    fpfr.FloodReport.Sources.Any(src => newSourceIds.Contains(src.SourceId))
                    // Date overlap
                    && fpfr.FloodReport.ImpactStartUtc != null
                    && incomingStart != null
                    && incomingEnd != null
                    && fpfr.FloodReport.ImpactStartUtc <= incomingEnd
                    && fpfr.FloodReport.ImpactStartUtc.Value.AddHours(fpfr.FloodReport.ImpactDurationHours) >= incomingStart
                )
                .OrderBy(fpfr => fpfr.FloodPropertyId)
                    .ThenBy(fpfr => fpfr.FloodReportId)
            )
            .Select(fpfr => fpfr.FloodPropertyId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}

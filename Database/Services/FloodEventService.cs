using FloodOnlineReportingTool.Database.DbContexts;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.Database.Models;
using FloodOnlineReportingTool.Database.Models.Messaging;
using FloodOnlineReportingTool.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FloodOnlineReportingTool.Database.Services;

public class FloodEventService(
    ILogger<FloodEventService> logger,
    IFloodEventRepository floodEventRepository,
    IFloodReportService floodReportService,
    ApplicationDbContext context
    ) : IFloodEventService
{
    private readonly JsonSerializerOptions _jsonOptions = JsonSerializerOptions.Web;

    public async Task CreateFromFloodEventCreated(FloodEventCreated message, CancellationToken ct)
    {
        if (message is null)
        {
            logger.LogWarning("Skipping flood event creation because the incoming message is null.");
            return;
        }

        logger.LogDebug("Creating flood event from {MessageType}.", nameof(FloodEventCreated));

        // Design choice: keep this resilient to contract evolution by mapping via JSON shape.
        var dto = JsonSerializer.Deserialize<FloodEventCreateDto>(
            JsonSerializer.Serialize(message, _jsonOptions),
            _jsonOptions);

        if (dto is null)
        {
            logger.LogWarning("Unable to map {MessageType} to {DtoType}.", nameof(FloodEventCreated), nameof(FloodEventCreateDto));
            return;
        }

        await CreateFloodEvent(dto, ct);
    }

    public async Task<ICollection<FloodEvent>?> CreateFloodEvent(FloodEventCreateDto dto, CancellationToken ct)
    {
        if (dto is null)
        {
            logger.LogWarning("Cannot create flood event because DTO is null.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(dto.Reference) || string.IsNullOrWhiteSpace(dto.Title))
        {
            logger.LogWarning("Cannot create flood event because reference or title is missing.");
            return null;
        }

        if (dto.FloodEndDate < dto.FloodStartDate)
        {
            logger.LogWarning(
                "Cannot create flood event with reference {Reference} because end date is before start date.",
                dto.Reference);
            return null;
        }

        var existing = await floodEventRepository
            .GetFloodEventByReference(dto.Reference)
            .FirstOrDefaultAsync(ct);

        if (existing is not null)
        {
            logger.LogInformation("Flood event with reference {Reference} already exists. Returning existing record.", dto.Reference);
            return [existing];
        }

        var floodEvent = new FloodEvent
        {
            Reference = dto.Reference.Trim(),
            Title = dto.Title.Trim(),
            RecordStatus = dto.RecordStatus,
            FloodStatus = dto.FloodStatus,
            InvestigationStatus = dto.InvestigationStatus,
            FloodStartDate = dto.FloodStartDate,
            FloodEndDate = dto.FloodEndDate,
            FloodCauses = dto.FloodCauses.Distinct().ToList(),
            Organisations = dto.Organisations.Distinct().ToList(),
            StatusFilter = dto.StatusFilter.Distinct().ToList(),
            LinkedFloodReports = dto.LinkedFloodReports ?? [],
            Geom = null,
        };

        context.FloodEvents.Add(floodEvent);
        var changes = await context.SaveChangesAsync(ct);

        if (changes < 1)
        {
            logger.LogWarning("No database changes were saved when creating flood event {Reference}.", dto.Reference);
            return null;
        }

        return [floodEvent];
    }

    public async Task UpdateFromFloodEventUpdated(FloodEventUpdated message, CancellationToken ct)
    {
        if (message is null)
        {
            logger.LogWarning("Skipping flood event update because the incoming message is null.");
            return;
        }

        logger.LogDebug("Updating flood event from {MessageType}.", nameof(FloodEventUpdated));

        // Design choice: keep this resilient to contract evolution by mapping via JSON shape.
        var dto = JsonSerializer.Deserialize<FloodEventUpdateDto>(
            JsonSerializer.Serialize(message, _jsonOptions),
            _jsonOptions);

        if (dto is null)
        {
            logger.LogWarning("Unable to map {MessageType} to {DtoType}.", nameof(FloodEventUpdated), nameof(FloodEventUpdateDto));
            return;
        }

        await UpdateFloodEvent(dto, ct);
    }

    public async Task UpdateFloodEvent(FloodEventUpdateDto dto, CancellationToken ct)
    {
        if (dto is null)
        {
            logger.LogWarning("Cannot update flood event because DTO is null.");
            return;
        }

        if (string.IsNullOrWhiteSpace(dto.Reference) || string.IsNullOrWhiteSpace(dto.Title))
        {
            logger.LogWarning("Cannot update flood event because reference or title is missing.");
            return;
        }

        if (dto.FloodEndDate < dto.FloodStartDate)
        {
            logger.LogWarning(
                "Cannot update flood event with reference {Reference} because end date is before start date.",
                dto.Reference);
            return;
        }

        var reference = dto.Reference.Trim();

        // Prefer business key matching since external systems may not know internal Id.
        var existing = await floodEventRepository
            .GetFloodEventByReference(reference)
            .FirstOrDefaultAsync(ct);

        // Optional fallback if reference did not match and Id was provided.
        if (existing is null && dto.Id is Guid id && id != Guid.Empty)
        {
            existing = await floodEventRepository
                .GetFloodEventById(id)
                .FirstOrDefaultAsync(ct);
        }

        if (existing is null)
        {
            logger.LogInformation( "Flood event was not found for reference {Reference}.", reference);
            return;
        }

        var updated = existing with
        {
            Title = dto.Title.Trim(),
            RecordStatus = dto.RecordStatus,
            FloodStatus = dto.FloodStatus,
            InvestigationStatus = dto.InvestigationStatus,
            LinkedFloodReports = dto.LinkedFloodReports is null ? existing.LinkedFloodReports : dto.LinkedFloodReports ?? [],
            // If geometry is missing from the message, keep existing geometry.
            Geom = dto.Geom ?? existing.Geom,
            FloodStartDate = dto.FloodStartDate,
            FloodEndDate = dto.FloodEndDate,
            FloodCauses = dto.FloodCauses?.Distinct().ToList() ?? existing.FloodCauses,
            Organisations = dto.Organisations?.Distinct().ToList() ?? existing.Organisations,
            StatusFilter = dto.StatusFilter?.Distinct().ToList() ?? existing.StatusFilter,
            UpdatedUtc = DateTimeOffset.UtcNow,
        };

        context.FloodEvents.Update(updated);
        var changes = await context.SaveChangesAsync(ct);

        if (changes < 1)
        {
            logger.LogWarning("No database changes were saved when updating flood event {Reference}.", reference);
        }
    }

    public async Task TriggerFromFloodEventS19Triggered(FloodEventS19Triggered message, CancellationToken ct)
    {
        if (message is null)
        {
            logger.LogWarning("Skipping because the incoming message is null.");
            return;
        }

        logger.LogDebug("Trigger from {MessageType}.", nameof(FloodEventS19Triggered));

        var floodEvent = await context
            .FloodEvents
            .Where(e => e.Reference == message.Reference.Trim())
            .FirstOrDefaultAsync(ct);

        if (floodEvent is null)
        {
            logger.LogWarning("Skipping because flood event is not found.");
            return;
        }

        foreach (var reportReference in floodEvent.LinkedFloodReports)
        {
            if (string.IsNullOrWhiteSpace(reportReference))
            {
                logger.LogWarning("Skipping status update because linked flood report reference is missing.");
                continue;
            }

            var trimmedReference = reportReference.Trim();

            var report = await context.FloodReports
               .Include(r => r.SourceRecords)
               .FirstOrDefaultAsync(r => r.Reference == trimmedReference, ct);

            if (report is null)
            {
                logger.LogWarning("Skipping status update because flood report {ReportReference} was not found.", trimmedReference);
                continue;
            }

            foreach (var sourceReport in report.SourceRecords)
            {
                FloodReportExtraInfoRequest messageBody = new FloodReportExtraInfoRequest(
                sourceReport.Reference,
                DateTimeOffset.UtcNow);

                OutboxMessage outboxMessage = new()
                {
                    MessageType = nameof(FloodReportExtraInfoRequest),
                    Message = JsonSerializer.Serialize(messageBody, _jsonOptions),
                    Status = MessageStatus.Pending,
                };

                context.OutboxMessages.Add(outboxMessage);
            }
            await context.SaveChangesAsync(ct);

            // Start from current values, then override only what should change.
            var dto = report.ToDto() with
            {
                RecordStatus = RecordStatusIds.ActionNeeded
            };

            var updatedReport = await floodReportService.UpdateFloodReportByReference(trimmedReference, dto, ct);

            if (updatedReport is null)
            {
                logger.LogWarning("Failed to update status for flood report {ReportReference}.", trimmedReference);
            }
        }
        await context.SaveChangesAsync(ct);

    } 
}

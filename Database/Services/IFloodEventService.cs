
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Database.Models;

namespace FloodOnlineReportingTool.Database.Services;

public interface IFloodEventService
{
    Task CreateFromFloodEventCreated(FloodEventCreated message, CancellationToken ct);
    Task UpdateFromFloodEventUpdated(FloodEventUpdated message, CancellationToken ct);
    Task<ICollection<FloodEvent>?> CreateFloodEvent(FloodEventCreateDto dto, CancellationToken ct);
    Task UpdateFloodEvent(FloodEventUpdateDto dto, CancellationToken ct);
    Task TriggerFromFloodEventS19Triggered(FloodEventS19Triggered message, CancellationToken ct);
}

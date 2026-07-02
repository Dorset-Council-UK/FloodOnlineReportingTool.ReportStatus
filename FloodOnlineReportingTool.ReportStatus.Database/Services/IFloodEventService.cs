
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodEventService
{
    Task CreateFromFloodEventCreated(FloodEventCreated message, CancellationToken ct);
    Task UpdateFromFloodEventUpdated(FloodEventUpdated message, CancellationToken ct);
    Task<ICollection<FloodEvent>?> CreateFloodEvent(FloodEventCreateDto dto, CancellationToken ct);
    Task UpdateFloodEvent(FloodEventUpdateDto dto, CancellationToken ct);
}

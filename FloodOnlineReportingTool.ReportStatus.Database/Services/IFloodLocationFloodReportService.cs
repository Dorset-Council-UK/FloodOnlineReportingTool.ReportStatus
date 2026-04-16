using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodLocationFloodReportService
{
    /// <summary>
    /// Gets all flood report relationships for the flood location.
    /// </summary>
    Task<IReadOnlyCollection<FloodLocationFloodReport>> GetFloodLocationFloodReports(Guid floodLocationId, CancellationToken ct);

    /// <summary>
    /// Gets a flood report relationship for a flood location.
    /// </summary>
    Task<FloodLocationFloodReport?> GetFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new unconfirmed flood report relationship for a flood location.
    /// </summary>
    Task<FloodLocationFloodReport> CreateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, Guid floodReportSourceId, CancellationToken ct);

    /// <summary>
    /// Creates a new confirmed flood report relationship for a flood location.
    /// </summary>
    Task<FloodLocationFloodReport> CreateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, Guid floodReportSourceId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Confirm the flood report is correctly related to the flood location.
    /// </summary>
    Task<FloodLocationFloodReport?> ConfirmFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood report relationship for a flood location.
    /// </summary>
    Task<bool> DeleteFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a relationship exists between a flood location and flood report.
    /// </summary>
    Task<bool> FloodLocationFloodReportExists(Guid floodLocationId, Guid floodReportId, CancellationToken ct);
}

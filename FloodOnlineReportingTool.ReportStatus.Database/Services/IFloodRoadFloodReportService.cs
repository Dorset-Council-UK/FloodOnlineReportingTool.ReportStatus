using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodRoadFloodReportService
{
    /// <summary>
    /// Gets all flood report relationships for the flood road.
    /// </summary>
    Task<IReadOnlyCollection<FloodRoadFloodReport>> GetFloodRoadFloodReports(Guid floodRoadId, CancellationToken ct);

    /// <summary>
    /// Gets a flood report relationship for a flood road.
    /// </summary>
    Task<FloodRoadFloodReport?> GetFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new unconfirmed flood report relationship for a flood road.
    /// </summary>
    Task<FloodRoadFloodReport> CreateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, Guid floodReportSourceId, CancellationToken ct);

    /// <summary>
    /// Creates a new confirmed flood report relationship for a flood road.
    /// </summary>
    Task<FloodRoadFloodReport> CreateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, Guid floodReportSourceId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Confirm the flood report is correctly related to the flood road.
    /// </summary>
    Task<FloodRoadFloodReport?> ConfirmFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood report relationship for a flood road.
    /// </summary>
    Task<bool> DeleteFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a relationship exists between a flood road and flood report.
    /// </summary>
    Task<bool> FloodRoadFloodReportExists(Guid floodRoadId, Guid floodReportId, CancellationToken ct);
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodRoadFloodReportService
{
    /// <summary>
    /// Gets all flood road flood reports from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodRoadFloodReport>> GetFloodRoadFloodReports(Guid floodRoadId, CancellationToken ct);

    /// <summary>
    /// Gets a flood road flood report from the database.
    /// </summary>
    Task<FloodRoadFloodReport?> GetFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new flood road flood report in the database.
    /// </summary>
    Task<FloodRoadFloodReport> CreateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, FloodRoadFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood road flood report in the database.
    /// </summary>
    Task<FloodRoadFloodReport?> UpdateFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, FloodRoadFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood road flood report from the database.
    /// </summary>
    Task<bool> DeleteFloodRoadFloodReport(Guid floodRoadId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a flood road flood report exists
    /// </summary>
    Task<bool> FloodRoadFloodReportExists(Guid floodRoadId, Guid floodReportId, CancellationToken ct);
}

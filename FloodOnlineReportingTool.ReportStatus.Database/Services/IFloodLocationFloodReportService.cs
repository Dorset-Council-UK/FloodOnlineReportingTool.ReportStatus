using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodLocationFloodReportService
{
    /// <summary>
    /// Gets all flood location flood reports from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodLocationFloodReport>> GetFloodLocationFloodReports(Guid floodLocationId, CancellationToken ct);

    /// <summary>
    /// Gets a flood location flood report from the database.
    /// </summary>
    Task<FloodLocationFloodReport?> GetFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new flood location flood report in the database.
    /// </summary>
    Task<FloodLocationFloodReport> CreateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, FloodLocationFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood location flood report in the database.
    /// </summary>
    Task<FloodLocationFloodReport?> UpdateFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, FloodLocationFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood location flood report from the database.
    /// </summary>
    Task<bool> DeleteFloodLocationFloodReport(Guid floodLocationId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a flood location flood report exists
    /// </summary>
    Task<bool> FloodLocationFloodReportExists(Guid floodLocationId, Guid floodReportId, CancellationToken ct);
}

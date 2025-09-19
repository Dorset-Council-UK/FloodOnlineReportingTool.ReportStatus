using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodPropertyFloodReportService
{
    /// <summary>
    /// Gets all flood property flood reports from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodPropertyFloodReport>> GetFloodPropertyFloodReports(Guid floodPropertyId, CancellationToken ct);

    /// <summary>
    /// Gets a flood property flood report from the database.
    /// </summary>
    Task<FloodPropertyFloodReport?> GetFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new flood property flood report in the database.
    /// </summary>
    Task<FloodPropertyFloodReport> CreateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, FloodPropertyFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood property flood report in the database.
    /// </summary>
    Task<FloodPropertyFloodReport?> UpdateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, FloodPropertyFloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood property flood report from the database.
    /// </summary>
    Task<bool> DeleteFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a flood property flood report exists
    /// </summary>
    Task<bool> FloodPropertyFloodReportExists(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);
}

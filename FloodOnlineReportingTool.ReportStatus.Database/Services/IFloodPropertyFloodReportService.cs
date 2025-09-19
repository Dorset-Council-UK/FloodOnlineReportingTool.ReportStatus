using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodPropertyFloodReportService
{
    /// <summary>
    /// Gets all flood report relationships for the flood property.
    /// </summary>
    Task<IReadOnlyCollection<FloodPropertyFloodReport>> GetFloodPropertyFloodReports(Guid floodPropertyId, CancellationToken ct);

    /// <summary>
    /// Gets a flood report relationship for a flood property.
    /// </summary>
    Task<FloodPropertyFloodReport?> GetFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new unconfirmed flood report relationship for a flood property.
    /// </summary>
    Task<FloodPropertyFloodReport> CreateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Creates a new confirmed flood report relationship for a flood property.
    /// </summary>
    Task<FloodPropertyFloodReport> CreateFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Confirm the flood report is correctly related to the flood property.
    /// </summary>
    Task<FloodPropertyFloodReport?> ConfirmFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, string userId, string userName, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood report relationship for a flood property.
    /// </summary>
    Task<bool> DeleteFloodPropertyFloodReport(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Checks if a relationship exists between a flood property and flood report.
    /// </summary>
    Task<bool> FloodPropertyFloodReportExists(Guid floodPropertyId, Guid floodReportId, CancellationToken ct);
}

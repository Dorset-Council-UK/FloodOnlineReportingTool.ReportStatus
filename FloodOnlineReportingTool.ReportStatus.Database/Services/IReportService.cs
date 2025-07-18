using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IReportService
{
    /// <summary>
    /// Gets all reports from the database.
    /// </summary>
    Task<IReadOnlyList<Report>> GetReports(CancellationToken ct);

    /// <summary>
    /// Gets a report by its ID from the database.
    /// </summary>
    Task<Report?> GetReportById(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new report in the database.
    /// </summary>
    Task<Report> CreateReport(ReportDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing report in the database.
    /// </summary>
    Task<Report?> UpdateReport(Guid id, ReportDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing report in the database.
    /// </summary>
    Task<bool> DeleteReport(Guid id, CancellationToken ct);
}

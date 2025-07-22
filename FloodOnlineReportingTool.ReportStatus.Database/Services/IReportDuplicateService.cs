using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IReportDuplicateService
{
    /// <summary>
    /// Gets all duplicate reports from the database.
    /// </summary>
    Task<IReadOnlyList<ReportDuplicate>> GetDuplicateReports(CancellationToken ct);

    /// <summary>
    /// Gets all duplicates for a report from the database.
    /// </summary>
    Task<IReadOnlyList<ReportDuplicate>> GetReportDuplicates(Guid reportId, CancellationToken ct);

    /// <summary>
    /// Gets a duplicate report by its ID from the database.
    /// </summary>
    Task<ReportDuplicate?> GetDuplicateReportById(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new duplicate report in the database.
    /// </summary>
    Task<ReportDuplicate> CreateDuplicateReport(ReportDuplicateDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing duplicate report in the database.
    /// </summary>
    Task<ReportDuplicate?> UpdateDuplicateReport(Guid id, ReportDuplicateDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing duplicate report in the database.
    /// </summary>
    Task<bool> DeleteDuplicateReport(Guid id, CancellationToken ct);
}

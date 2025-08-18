using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IReportDuplicateService
{
    /// <summary>
    /// Gets all duplicate reports from the database.
    /// </summary>
    Task<IReadOnlyList<ReportDuplicate>> GetDuplicates(CancellationToken ct);

    /// <summary>
    /// Gets all duplicates associated with a report from the database.
    /// </summary>
    Task<IReadOnlyList<ReportDuplicate>> GetDuplicates(Guid reportId, CancellationToken ct);

    /// <summary>
    /// Gets a duplicate report by its ID from the database.
    /// </summary>
    Task<ReportDuplicate?> GetDuplicate(Guid duplicateId, CancellationToken ct);

    /// <summary>
    /// Gets a duplicate associated with a report by its ID from the database.
    /// </summary>
    Task<ReportDuplicate?> GetDuplicate(Guid reportId, Guid duplicateId, CancellationToken ct);

    /// <summary>
    /// Creates a new duplicate associated with a report in the database.
    /// </summary>
    Task<ReportDuplicate?> CreateDuplicate(Guid reportId, ReportDuplicateDto dto, CancellationToken ct);

    /// <summary>
    /// Creates a new duplicate associated with a report in the database.
    /// </summary>
    Task<ReportDuplicate?> CreateDuplicate(Guid reportId, EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct);

    /// <summary>
    /// Updates an existing duplicate report in the database.
    /// </summary>
    Task<ReportDuplicate?> UpdateDuplicate(Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing duplicate associated with a report in the database.
    /// </summary>
    Task<ReportDuplicate?> UpdateDuplicate(Guid reportId, Guid duplicateId, ReportDuplicateDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing duplicate report in the database.
    /// </summary>
    Task<bool> DeleteDuplicate(Guid duplicateId, CancellationToken ct);

    /// <summary>
    /// Deletes an existing duplicate associated with a report in the database.
    /// </summary>
    Task<bool> DeleteDuplicate(Guid reportId, Guid duplicateId, CancellationToken ct);
}

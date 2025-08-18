using FloodOnlineReportingTool.Contracts;
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
    Task<Report?> GetReport(Guid id, CancellationToken ct);

    /// <summary>
    /// Try to find a report using the information reported in an eligibility check created message.
    /// </summary>
    /// <remarks>Returning the ID not the full report</remarks>
    Task<Guid?> GetReport(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct);

    /// <summary>
    /// Creates a new report in the database.
    /// </summary>
    Task<Report> CreateReport(ReportDto dto, CancellationToken ct);

    /// <summary>
    /// Creates a new report in the database.
    /// </summary>
    Task<Report> CreateReport(EligibilityCheckCreated eligibilityCheckCreated, CancellationToken ct);

    /// <summary>
    /// Updates an existing report in the database.
    /// </summary>
    Task<Report?> UpdateReport(Guid id, ReportDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing report in the database.
    /// </summary>
    Task<bool> DeleteReport(Guid id, CancellationToken ct);
}

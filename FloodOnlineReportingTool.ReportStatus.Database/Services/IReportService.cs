using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IReportService
{
    /// <summary>
    /// Gets all reports from the database.
    /// </summary>
    Task<IReadOnlyCollection<Report>> GetReports(CancellationToken ct);

    /// <summary>
    /// Gets a report by its ID from the database.
    /// </summary>
    Task<Report?> GetReportById(Guid id, CancellationToken ct);

    /// <summary>
    /// Get reports by there matching UPRN's from the database.
    /// </summary>
    Task<IReadOnlyCollection<Report>> GetReportsByUprn(long uprn, CancellationToken ct);

    /// <summary>
    /// Get reports by there matching USRN's from the database.
    /// </summary>
    Task<IReadOnlyCollection<Report>> GetReportsByUsrn(long usrn, CancellationToken ct);

    /// <summary>
    /// Get reports that are within a specified buffer distance.
    /// </summary>
    Task<IReadOnlyCollection<Report>> GetReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct);

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

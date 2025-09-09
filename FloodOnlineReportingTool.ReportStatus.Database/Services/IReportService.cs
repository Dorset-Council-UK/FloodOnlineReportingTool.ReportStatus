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
    /// Gets a report by it's UPRN from the database.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    Task<Report?> GetReportByUprn(long uprn, CancellationToken ct);

    /// <summary>
    /// Gets a report that is within a specified buffer distance.
    /// </summary>
    /// <returns>The <see cref="Report"/> if found, otherwise <see langword="null"/>.</returns>
    Task<Report?> GetReportWithinMetres(double easting, double northing, double metres, CancellationToken ct);

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

using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IReportSourceService
{
    /// <summary>
    /// Gets all reports sources from the database.
    /// </summary>
    Task<IReadOnlyCollection<ReportSource>> GetReportSources(Guid reportId, CancellationToken ct);

    /// <summary>
    /// Gets a report source by its ID from the database.
    /// </summary>
    Task<ReportSource?> GetReportSource(Guid reportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Creates a new report source in the database.
    /// </summary>
    Task<ReportSource> CreateReportSource(Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing report source in the database.
    /// </summary>
    Task<ReportSource?> UpdateReportSource(Guid reportId, Guid sourceId, ReportSourceDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing report source in the database.
    /// </summary>
    Task<bool> DeleteReportSource(Guid reportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Checks if a report source exists
    /// </summary>
    Task<bool> ReportSourceExists(Guid reportId, Guid sourceId, CancellationToken ct);
}

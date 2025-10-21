using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodReportSourceService
{
    /// <summary>
    /// Gets all flood reports sources from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodReportSource>> GetFloodReportSources(Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Gets a flood report source by its ID from the database.
    /// </summary>
    Task<FloodReportSource?> GetFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Creates a new flood report source in the database.
    /// </summary>
    Task<FloodReportSource> CreateFloodReportSource(Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct);

    /// <summary>
    /// Creates multiple flood report sources at once using the message data.
    /// </summary>
    Task CreateFloodReportSources(Guid floodReportId, IReadOnlyCollection<EligibilityCheckFloodSource> floodSources, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood report source in the database.
    /// </summary>
    Task<FloodReportSource?> UpdateFloodReportSource(Guid floodReportId, Guid sourceId, FloodReportSourceDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood report source in the database.
    /// </summary>
    Task<bool> DeleteFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Checks if a flood report source exists
    /// </summary>
    Task<bool> FloodReportSourceExists(Guid floodReportId, Guid sourceId, CancellationToken ct);
}

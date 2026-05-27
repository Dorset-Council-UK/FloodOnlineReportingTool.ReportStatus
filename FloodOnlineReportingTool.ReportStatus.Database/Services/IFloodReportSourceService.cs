using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodReportSourceService
{
    /// <summary>
    /// Gets all flood reports sources from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodReportSource>> GetFloodReportSources(Guid floodReportId, CancellationToken ct);

    /// <summary>
    /// Gets a flood report source by its reference from the database
    /// </summary>
    Task<FloodReportSource?> GetFloodReportSourceByReference(string sourceReference, CancellationToken ct);

    /// <summary>
    /// Gets a flood report source by its ID from the database.
    /// </summary>
    Task<FloodReportSource?> GetFloodReportSource(Guid floodReportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Adds a new flood cause to a flood report source in the database.
    /// </summary>
    Task<bool> AddFloodReportFloodSource(Guid floodReportId, Guid sourceId, string name, CancellationToken ct);

    /// <summary>
    /// Adds multiple flood causes to a flood report source at once using the message data.
    /// </summary>
    Task AddManyFloodReportFloodSources(Guid floodReportId, IReadOnlyCollection<EligibilityCheckFloodSource> floodSources, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood cause from a flood report source in the database.
    /// </summary>
    Task<bool> DeleteFloodReportFloodSource(Guid floodReportId, Guid sourceId, CancellationToken ct);

    /// <summary>
    /// Checks if a flood cause exists within a flood report source.
    /// </summary>
    Task<bool> FloodReportFloodSourceExists(Guid floodReportId, Guid sourceId, CancellationToken ct);
}

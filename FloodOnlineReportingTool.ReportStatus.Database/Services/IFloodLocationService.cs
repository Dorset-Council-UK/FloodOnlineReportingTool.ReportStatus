using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodLocationService
{
    /// <summary>
    /// Gets all flood locations from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodLocation>> GetFloodLocations(CancellationToken ct);

    /// <summary>
    /// Gets all flood locations by matching within the buffer provided, including related flood reports.
    /// </summary>
    /// <param name="easting"></param>
    /// <param name="northing"></param>
    /// <param name="buffer"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<FloodLocation>> GetFloodLocationsByPoint(double easting, double northing, double buffer, CancellationToken ct);

    /// <summary>
    /// Gets all flood locations, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<IReadOnlyCollection<FloodLocation>> GetFloodLocationsWithReports(CancellationToken ct);

    /// <summary>
    /// Gets a flood location by its ID from the database.
    /// </summary>
    Task<FloodLocation?> GetFloodLocation(Guid id, CancellationToken ct);

    /// <summary>
    /// Gets a flood location by its ID, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<FloodLocation?> GetFloodLocationWithReports(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new flood location in the database.
    /// </summary>
    Task<FloodLocation> CreateFloodLocation(FloodLocationDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood location in the database.
    /// </summary>
    Task<FloodLocation?> UpdateFloodLocation(Guid id, FloodLocationDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood location from the database.
    /// </summary>
    Task<bool> DeleteFloodLocation(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if a flood location exists
    /// </summary>
    Task<bool> FloodLocationExists(Guid id, CancellationToken ct);
}

using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodLocationService
{
    const double MaxBuffer = 100;

    /// <summary>
    /// Gets all flood locations from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodLocation>> GetFloodLocations(CancellationToken ct);

    /// <summary>
    /// Gets all flood locations by matching within the buffer provided, including related flood reports.
    /// </summary>
    /// <param name="easting">The Easting coordinate of the location to search for existing flood reports.</param>
    /// <param name="northing">The Northing coordinate of the location to search for existing flood reports.</param>
    /// <param name="buffer">The distance buffer (in metres) around the Easting and Northing coordinates to include in the search.</param>
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
    /// <para>Always add the flood location + sources</para>
    /// <para>Sometimes add the location + linked flood report</para>
    /// <para>Sometimes just link the location flood report(s)</para>
    /// </summary>
    Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, string locationDescription, double buffer, CancellationToken ct);

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

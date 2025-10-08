using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodRoadService
{
    /// <summary>
    /// Gets all flood roads from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodRoad>> GetFloodRoads(CancellationToken ct);

    /// <summary>
    /// Gets all flood road by its USRN from the database, including related flood reports.
    /// </summary>
    /// <param name="Usrn"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsByUsrn(long Usrn, CancellationToken ct);

    /// <summary>
    /// Gets all flood roads, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<IReadOnlyCollection<FloodRoad>> GetFloodRoadsWithReports(CancellationToken ct);

    /// <summary>
    /// Gets a flood road by its ID from the database.
    /// </summary>
    Task<FloodRoad?> GetFloodRoad(Guid id, CancellationToken ct);

    /// <summary>
    /// Gets a flood road by its ID, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<FloodRoad?> GetFloodRoadWithReports(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new flood road in the database.
    /// </summary>
    Task<FloodRoad> CreateFloodRoad(FloodRoadDto dto, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood road in the database.
    /// </summary>
    Task<FloodRoad?> UpdateFloodRoad(Guid id, FloodRoadDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood road from the database.
    /// </summary>
    Task<bool> DeleteFloodRoad(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if a flood road exists
    /// </summary>
    Task<bool> FloodRoadExists(Guid id, CancellationToken ct);
}

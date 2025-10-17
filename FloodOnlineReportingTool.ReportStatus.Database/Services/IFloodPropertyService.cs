using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodPropertyService
{
    /// <summary>
    /// Gets all flood properties from the database.
    /// </summary>
    Task<IReadOnlyCollection<FloodProperty>> GetFloodProperties(CancellationToken ct);

    /// <summary>
    /// Gets all flood property by its UPRN from the database, including related flood reports.
    /// </summary>
    /// <param name="Uprn"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesByUprn(long uprn, CancellationToken ct);

    /// <summary>
    /// Gets all flood properties, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<IReadOnlyCollection<FloodProperty>> GetFloodPropertiesWithReports(CancellationToken ct);

    /// <summary>
    /// Gets a flood property by its ID from the database.
    /// </summary>
    Task<FloodProperty?> GetFloodProperty(Guid id, CancellationToken ct);

    /// <summary>
    /// Gets a flood property by its ID, including related flood reports.
    /// </summary>
    /// <remarks>Related Flood Reports will be included.</remarks>
    Task<FloodProperty?> GetFloodPropertyWithReports(Guid id, CancellationToken ct);

    /// <summary>
    /// Creates a new flood property in the database.
    /// </summary>
    Task<FloodProperty> CreateFloodProperty(FloodPropertyDto dto, CancellationToken ct);

    /// <summary>
    /// <para>Always add the flood report + sources</para>
    /// <para>Sometimes add the property + linked flood report</para>
    /// <para>Sometimes just link the property flood report(s)</para>
    /// </summary>
    Task CreateFromEligibilityCheckCreated(EligibilityCheckCreated ecc, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood property in the database.
    /// </summary>
    Task<FloodProperty?> UpdateFloodProperty(Guid id, FloodPropertyDto dto, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood property from the database.
    /// </summary>
    Task<bool> DeleteFloodProperty(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if a flood property exists
    /// </summary>
    Task<bool> FloodPropertyExists(Guid id, CancellationToken ct);
}

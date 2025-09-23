using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface ISearchService
{
    /// <summary>
    /// Gets search results by UPRN.
    /// </summary>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUPRN(long uprn, CancellationToken ct);

    /// <summary>
    /// Gets search results by coordinates (Easting and Northing).
    /// </summary>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByCoordinates(double easting, double northing, CancellationToken ct);

    /// <summary>
    /// Gets search results by date the flood started.
    /// </summary>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByStartDate(DateTimeOffset startDateUtc, CancellationToken ct);
}

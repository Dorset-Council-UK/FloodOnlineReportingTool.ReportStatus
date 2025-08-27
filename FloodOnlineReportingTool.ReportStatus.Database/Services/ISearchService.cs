using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface ISearchService
{
    /// <summary>
    /// Gets search results by UPRN.
    /// </summary>
    Task<IReadOnlyList<Report>> GetReportsByUPRN(double uprn, CancellationToken ct);

    /// <summary>
    /// Gets search results by coordinates (Easting and Northing).
    /// </summary>
    Task<IReadOnlyList<Report>> GetReportsByCoordinates(double easting, double northing, CancellationToken ct);

    /// <summary>
    /// Gets search results by date the flood started.
    /// </summary>
    Task<IReadOnlyList<Report>> GetReportsByStartDate(DateTimeOffset startDateUtc, CancellationToken ct);
}

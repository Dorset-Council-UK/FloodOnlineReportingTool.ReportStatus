namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

/// <summary>
/// Junction entity linking a FloodReportSource to its organisations.
/// Composite key ensures uniqueness per source, not globally.
/// </summary>
public record FloodReportSourceOrganisation
{
    // Composite FK to FloodReportSource
    public required Guid FloodReportId { get; init; }
    public required Guid FloodReportSourceId { get; init; }

    // The organisation - unique per FloodReportSource, not globally
    public required Guid OrganisationId { get; init; }
    public required string OrganisationName { get; init; }
    public required Guid FloodAuthorityId { get; init; }
    public required string FloodAuthorityName { get; init; }
}
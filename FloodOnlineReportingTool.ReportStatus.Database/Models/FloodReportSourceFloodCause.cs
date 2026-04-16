namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

/// <summary>
/// Junction entity linking a FloodReportSource to its flood causes.
/// Composite key ensures uniqueness per source, not globally.
/// </summary>
public record FloodReportSourceFloodCause
{
    // Composite FK to FloodReportSource
    public required Guid FloodReportId { get; init; }
    public required Guid FloodReportSourceId { get; init; }

    // The cause - unique per FloodReportSource, not globally
    public required Guid CauseId { get; init; }
    public required string Name { get; init; }
}
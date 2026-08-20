using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.Database.Models;

/// <summary>
/// Junction entity linking a FloodReportSource to its flood causes.
/// Composite key ensures uniqueness per source, not globally.
/// </summary>
public record FloodReportSourceFloodCause
{
    public Guid Id { get; init; }
    // The cause - unique per FloodReportSource, not globally
    public required Guid CauseId { get; init; }
    public required string Name { get; init; }

    public string? FloodReportSourceRef { get; init; }
    [JsonIgnore]
    public FloodReportSource? FloodReportSource { get; init; }

    public Guid? FloodReportSourceDuplicateId { get; init; }
    [JsonIgnore]
    public FloodReportSourceDuplicate? FloodReportSourceDuplicate { get; init; }
}
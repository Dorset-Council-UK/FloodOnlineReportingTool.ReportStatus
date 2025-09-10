namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record ReportSource
{
    public required Guid ReportId { get; init; }
    public required Guid SourceId { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }
    public required string Name { get; init; }

    public virtual Report Report { get; init; } = default!;
}
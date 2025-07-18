namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record ReportDuplicate
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required Guid ReportId { get; init; }
    public required Guid EligibilityCheckId { get; init; }
    public DateTimeOffset CreatedUtc { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedUtc { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }
}
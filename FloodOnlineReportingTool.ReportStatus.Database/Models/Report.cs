namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

internal record Report
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required Guid EligibilityCheckId { get; init; }
    public DateTimeOffset CreatedUtc { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedUtc { get; init; }
    public required long Uprn { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int ImpactDurationHours { get; init; }
}
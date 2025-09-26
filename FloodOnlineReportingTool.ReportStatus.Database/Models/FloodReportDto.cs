namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportDto
{
    public required Guid EligibilityCheckId { get; init; }
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int ImpactDurationHours { get; init; }
}
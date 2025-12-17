using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportDto
{
    public required Guid EligibilityCheckId { get; init; }
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public required Point Geom { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int ImpactDurationHours { get; init; }
}
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReport : ICreatedUpdated
{
    public required Guid Id { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    public required Guid EligibilityCheckId { get; init; }
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public required Point Location { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int ImpactDurationHours { get; init; }

    public virtual ICollection<FloodReportSource> Sources { get; init; } = [];
}
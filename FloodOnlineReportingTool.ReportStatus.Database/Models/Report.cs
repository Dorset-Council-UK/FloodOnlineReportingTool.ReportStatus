using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record Report
{
    public required Guid Id { get; init; }
    public required Guid EligibilityCheckId { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int ImpactDurationHours { get; init; }

    [JsonIgnore]
    public virtual IReadOnlyCollection<ReportDuplicate> Duplicates { get; init; } = [];
    [JsonIgnore]
    public virtual IReadOnlyCollection<ReportSource> Sources { get; init; } = [];
}
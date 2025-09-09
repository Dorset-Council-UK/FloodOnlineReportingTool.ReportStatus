using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record ReportDuplicate
{
    public required Guid ReportId { get; init; }
    public required Guid Id { get; init; }
    public required Guid EligibilityCheckId { get; init; }
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }

    //[JsonIgnore]
    public virtual Report Report { get; init; } = default!;
}
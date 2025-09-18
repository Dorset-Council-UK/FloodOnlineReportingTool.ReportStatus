using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportSource : ICreatedUpdated
{
    public required Guid FloodReportId { get; init; }
    public required Guid SourceId { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    public required string Name { get; init; }

    [JsonIgnore]
    public virtual FloodReport FloodReport { get; init; } = default!;
}
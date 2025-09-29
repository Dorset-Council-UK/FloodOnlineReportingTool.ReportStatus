using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodLocationFloodReport : ICreatedUpdated, IConfirmed
{
    public required Guid FloodLocationId { get; init; }
    public required Guid FloodReportId { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    // IConfirmed implementation
    public DateTimeOffset? ConfirmedUtc { get; init; }
    public string? ConfirmedByUserId { get; init; }
    public string? ConfirmedUserName { get; init; }

    [JsonIgnore]
    public virtual FloodLocation FloodLocation { get; init; } = default!;
    [JsonIgnore]
    public virtual FloodReport FloodReport { get; init; } = default!;
}
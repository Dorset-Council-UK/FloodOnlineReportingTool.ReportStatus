using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodRoadFloodReport : ICreatedUpdated, IConfirmed
{
    public required Guid FloodRoadId { get; init; }
    public required Guid FloodReportId { get; init; }
    public required Guid FloodReportSourceId { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    // IConfirmed implementation
    public DateTimeOffset? ConfirmedUtc { get; init; }
    public string? ConfirmedByUserId { get; init; }
    public string? ConfirmedUserName { get; init; }

    [JsonIgnore]
    public virtual FloodRoad FloodRoad { get; init; } = default!;
    [JsonIgnore]
    public virtual FloodReportSource FloodReport { get; init; } = default!;
}
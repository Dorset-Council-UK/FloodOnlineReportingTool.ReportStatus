using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodPropertyFloodReport : ICreatedUpdated, IConfirmed
{
    public required Guid FloodPropertyId { get; init; }
    public required Guid FloodReportId { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    // IConfirmed implementation
    public DateTimeOffset? ConfirmedUtc { get; init; }
    public string? ConfirmedByUserId { get; init; }
    public string? ConfirmedUserName { get; init; }

    [JsonIgnore]
    public virtual FloodProperty FloodProperty { get; init; } = default!;
    [JsonIgnore]
    public virtual FloodReport FloodReport { get; init; } = default!;
}
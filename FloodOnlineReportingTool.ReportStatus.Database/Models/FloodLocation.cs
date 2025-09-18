using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodLocation : ICreatedUpdated
{
    public required Guid Id { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    public required string Title { get; init; }
    public string? Notes { get; init; }
    public Point? Location { get; init; }

    [JsonIgnore]
    public virtual IReadOnlyCollection<FloodLocationFloodReport> FloodReports { get; init; } = [];
}
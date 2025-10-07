using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodProperty : ICreatedUpdated
{
    public required Guid Id { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    public required string Title { get; init; }
    public required long Uprn { get; init; }
    public string? Notes { get; init; }
    public required Point Location { get; init; }

    public virtual ICollection<FloodPropertyFloodReport> FloodReports { get; init; } = [];
}
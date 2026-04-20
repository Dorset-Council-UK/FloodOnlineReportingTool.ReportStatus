using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodPropertyDto
{
    public required string Title { get; init; }
    public required long Uprn { get; init; }
    public string? Notes { get; init; }
    public required Point Geom { get; init; }
}

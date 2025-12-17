using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodLocationDto
{
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public required Point Geom { get; init; }
}
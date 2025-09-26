using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodPropertyDto
{
    public required string Title { get; init; }
    public string? Notes { get; init; }
    public Point? Location { get; init; }
}
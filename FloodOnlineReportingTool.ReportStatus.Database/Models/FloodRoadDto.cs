using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodRoadDto
{
    public required string Title { get; init; }
    public required long Usrn { get; init; }
    public string? Notes { get; init; }
    public Point? Location { get; init; }
}
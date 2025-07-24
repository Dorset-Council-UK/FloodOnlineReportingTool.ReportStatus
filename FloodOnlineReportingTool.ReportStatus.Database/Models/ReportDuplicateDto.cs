namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record ReportDuplicateDto
{
    public required Guid EligibilityCheckId { get; init; }
    public required double Easting { get; init; }
    public required double Northing { get; init; }
}
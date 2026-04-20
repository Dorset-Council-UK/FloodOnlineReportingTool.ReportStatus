namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportDto
{
    public required Guid Id { get; init; }
    public required Guid RecordStatus { get; set; }

    // Other properties
    public required ICollection<FloodReportSource> SourceRecords { get; set; }
}
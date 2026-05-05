namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportDuplicateReference
{
    public required Guid Id { get; init; } // This is the primary key for this table
    public required Guid FloodReportId { get; init; }
    public required Guid PreviousRecordStatus { get; init; }
}

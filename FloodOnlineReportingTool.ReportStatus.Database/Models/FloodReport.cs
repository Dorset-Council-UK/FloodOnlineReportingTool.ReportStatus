using FloodOnlineReportingTool.Contracts.Shared;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
public record FloodReport : ICreatedUpdated
{
    public required Guid Id { get; init; }
    public required Guid RecordStatus { get; init; }
    public required FloodReportType RecordType { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    // Other properties
    public ICollection<FloodReportSource> SourceRecords { get; init; }
}

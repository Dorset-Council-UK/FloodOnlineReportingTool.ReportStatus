namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodPropertyFloodReportDto : IConfirmed
{
    // IConfirmed implementation
    public DateTimeOffset? ConfirmedUtc { get; init; }
    public string? ConfirmedByUserId { get; init; }
    public string? ConfirmedUserName { get; init; }
}
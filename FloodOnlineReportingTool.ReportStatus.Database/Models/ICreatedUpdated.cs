namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

internal interface ICreatedUpdated
{
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }
}

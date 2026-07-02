namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public interface ICreatedUpdated
{
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; set; }
}

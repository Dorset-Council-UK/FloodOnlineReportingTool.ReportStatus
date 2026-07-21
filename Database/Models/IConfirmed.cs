namespace FloodOnlineReportingTool.Database.Models;

public interface IConfirmed
{
    public DateTimeOffset? ConfirmedUtc { get; init; }
    public string? ConfirmedByUserId { get; init; }
    public string? ConfirmedUserName { get; init; }
}

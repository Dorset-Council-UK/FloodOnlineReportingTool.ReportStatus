namespace FloodOnlineReportingTool.ReportStatus.Settings;

public class ReportStatusSettings
{
    public const string SectionName = "ReportStatus";

    public required double LocationBufferMetres { get; init; }
    public required string ApiKey { get; init; }
    public required string PathBase { get; init; }
    public required Uri AddressNameUrl { get; init; }
    public required Uri StreetNameUrl { get; init; }
    public required int AccessTokenIssueDurationMonths { get; init; } = 6;
    public required string OSApiKey { get; init; }
}

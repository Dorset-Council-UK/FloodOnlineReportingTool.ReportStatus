namespace FloodOnlineReportingTool.ReportStatus.Database.Settings;

public record GISSettings
{
    public const string SectionName = "GIS";

    public required string ApiKey { get; init; }
    public required string PathBase { get; init; }
    public required Uri AddressNameUrl { get; init; }
    public required Uri StreetNameUrl { get; init; }
    public required int AccessTokenIssueDurationMonths { get; init; } = 6;
    public required string OSApiKey { get; init; }
}

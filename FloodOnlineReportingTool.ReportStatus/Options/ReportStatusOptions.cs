namespace FloodOnlineReportingTool.ReportStatus.Options;

public class ReportStatusOptions
{
    public const string SectionName = "ReportStatus";

    public required double LocationBufferMetres { get; init; }
    public required string ApiKey { get; init; }
    public required Uri AddressNameUrl { get; init; }
    public required Uri StreetNameUrl { get; init; }
}

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

internal class Versions
{
    internal static VersionInfo V1 = new(new(1, 0));
    internal static VersionInfo V2 = new(new(2, 0));
    internal static IReadOnlyCollection<VersionInfo> All = [V1, V2];
}

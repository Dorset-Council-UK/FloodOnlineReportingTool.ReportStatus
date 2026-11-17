namespace FloodOnlineReportingTool.ReportStatus.Options;

internal class KeyVaultOptions
{
    public const string SectionName = "KeyVault";

    public required string SharedVaultName { get; init; }
    public required string VaultName { get; init; }
}

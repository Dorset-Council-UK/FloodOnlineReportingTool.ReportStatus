namespace FloodOnlineReportingTool.ReportStatus.Services;

public interface IGazetteerSearchService
{
    /// <summary>
    /// Gets the address string by UPRN
    /// </summary>
    Task<string> GetAddressNameByUPRN(long uprn, Uri? referer, CancellationToken ct);

    /// <summary>
    /// Gets the street name by USRN
    /// </summary>
    Task<string> GetStreetNameByUSRN(long usrn, Uri? referer, CancellationToken ct);
}

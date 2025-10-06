namespace FloodOnlineReportingTool.ReportStatus.Database.Repositories;

public interface ISearchRepository
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

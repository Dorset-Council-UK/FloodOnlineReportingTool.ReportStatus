using FloodOnlineReportingTool.ReportStatus.Database.Exceptions;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace FloodOnlineReportingTool.ReportStatus.Services;

public class GazetteerSearchService : IGazetteerSearchService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ReportStatusSettings _settings;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<GazetteerSearchService> _logger;

    public GazetteerSearchService(IHttpClientFactory httpClientFactory, IOptions<ReportStatusSettings> settings, ILogger<GazetteerSearchService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    /// <summary>
    /// Create a search URI using the UPRN, URL and the API key from configuration.
    /// </summary>
    /// <exception cref="ConfigurationMissingException">If the search URI or API key are not set in the settings</exception>
    private Uri CreateAddressSearchUri(string uprn)
    {
        // Throw an error if the search URI or API key are not set
        var searchUri = _settings.AddressNameUrl
            ?? throw new ConfigurationMissingException("Missing configuration setting: The search URL is not set in the settings. Under GIS > AddressNameUrl");

        if (_settings.ApiKey is null)
        {
            throw new ConfigurationMissingException("Missing configuration setting: The GIS API key not set in the settings. Under GIS > ApiKey");
        }

        // Encode the search term
        var encodedSearchTerm = Uri.EscapeDataString(uprn);

        // Construct the search query using the Uri Builder, the search Uri and the API key are in the settings
        var builder = new UriBuilder(searchUri)
        {
            Query = $"query={encodedSearchTerm}&searchArea=uk",
        };
        return builder.Uri;
    }

    /// <summary>
    /// Create a search URI using the USRN, URL and the API key from configuration.
    /// </summary>
    /// <exception cref="ConfigurationMissingException">If the search URI or API key are not set in the settings</exception>
    private Uri CreateStreetSearchUri(string usrn)
    {
        // Throw an error if the search URI or API key are not set
        var searchUri = _settings.StreetNameUrl
            ?? throw new ConfigurationMissingException("Missing configuration setting: The search URL is not set in the settings. Under GIS > AddressNameUrl");

        if (_settings.ApiKey is null)
        {
            throw new ConfigurationMissingException("Missing configuration setting: The GIS API key not set in the settings. Under GIS > ApiKey");
        }

        // Encode the search term
        var encodedSearchTerm = Uri.EscapeDataString(usrn);

        // Construct the search query using the Uri Builder, the search Uri and the API key are in the settings
        var builder = new UriBuilder(searchUri)
        {
            Query = $"usrn={encodedSearchTerm}&searchArea=uk",
        };
        return builder.Uri;
    }

    private async Task<HttpResponseMessage> GetResponse(Uri uri, Uri? referer, CancellationToken ct)
    {
        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-FortApplication");
        if (referer is not null)
        {
            client.DefaultRequestHeaders.Add("Referer", referer.ToString());
        }
        client.DefaultRequestHeaders.Add("X-API-Key", _settings.ApiKey);

        var response = await client.GetAsync(uri, ct)
            .ConfigureAwait(false);

        return response;
    }

    public async Task<string> GetAddressNameByUPRN(long uprn, Uri? referer, CancellationToken ct)
    {
        _logger.LogDebug("Retrieving address string by UPRN");
        string resultString = "Address not found";

        var response = await GetResponse(CreateAddressSearchUri(uprn.ToString()), referer, ct)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var searchResult = await response.Content
                .ReadFromJsonAsync<List<ApiAddress>>(_jsonOptions, ct)
                .ConfigureAwait(false);

            if (searchResult is not null)
            {
                if (searchResult.Count > 0) {
                    resultString = searchResult[0].ConcatenatedAddress;
                }
            }
        }

        return resultString;
    }


    public async Task<string> GetStreetNameByUSRN(long usrn, Uri? referer, CancellationToken ct)
    {
        _logger.LogDebug("Retrieving street name by USRN");
        string resultString = "Street not found";

        var response = await GetResponse(CreateStreetSearchUri(usrn.ToString()), referer, ct)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var searchResult = await response.Content
                .ReadFromJsonAsync<List<ApiAddress>>(_jsonOptions, ct)
                .ConfigureAwait(false);

            if (searchResult is not null)
            {
                if (searchResult.Count > 0)
                {
                    resultString = searchResult[0].Street;
                }
            }
        }

        return resultString;
    }
}

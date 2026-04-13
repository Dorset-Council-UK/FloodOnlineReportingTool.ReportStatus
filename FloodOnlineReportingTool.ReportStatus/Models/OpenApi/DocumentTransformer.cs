using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Identity.Web;
using Microsoft.OpenApi;

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

internal sealed class DocumentTransformer(ApiVersion version, MicrosoftIdentityOptions identityOptions) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var scopes = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { $"api://{identityOptions.ClientId}/access_as_user", "Access the API as the signed-in user" },
        };

        AddInformation(document);
        AddSecuritySchemes(document, scopes);
        AddSecurityRequirements(document, scopes);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds information to the OpenAPI document.
    /// </summary>
    private void AddInformation(OpenApiDocument document)
    {
        // Document information
        var versionString = version.ToString("VVVV", ApiVersionFormatProvider.CurrentCulture);
        document.Info = new(document.Info)
        {
            Version = versionString,
            Title = $"Flood Online Reporting Tool - Report Status | v{versionString}",
            Description = "Administration and status reporting for the flood online reporting tool. Built for professional flood risk colleagues, tracking reports from both the public and lead authorities.",
            TermsOfService = new("https://github.com/Dorset-Council-UK/FloodOnlineReportingTool.ReportStatus"),
            Contact = new()
            {
                Name = "Contact",
                Url = new("https://github.com/Dorset-Council-UK/FloodOnlineReportingTool.ReportStatus"),
            },
            License = new()
            {
                Name = "MIT",
                Url = new("https://github.com/Dorset-Council-UK/FloodOnlineReportingTool.ReportStatus/blob/main/LICENSE"),
            },
        };
    }

    /// <summary>
    /// Adds security schemes to the OpenAPI document.
    /// </summary>
    private void AddSecuritySchemes(OpenApiDocument document, Dictionary<string, string> scopes)
    {
        // Note: OpenIdConnectUrl may not be needed unless Type is set to SecuritySchemeType.OpenIdConnect (OIDC) which might mean that the flow would need to be changed to Implicit.

        var baseEndpoint = new Uri(new Uri(identityOptions.Instance), $"{identityOptions.TenantId}/");
        var openIdConnectUrl = new Uri(baseEndpoint, "v2.0/.well-known/openid-configuration");
        var authorizationUrl = new Uri(baseEndpoint, "oauth2/v2.0/authorize");
        var tokenUrl = new Uri(baseEndpoint, "oauth2/v2.0/token");

        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>(StringComparer.OrdinalIgnoreCase);

        document.Components.SecuritySchemes.Add(Constants.Bearer, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Scheme = Constants.Bearer,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            OpenIdConnectUrl = openIdConnectUrl,
            Flows = new()
            {
                AuthorizationCode = new()
                {
                    AuthorizationUrl = authorizationUrl,
                    TokenUrl = tokenUrl,
                    Scopes = scopes,
                },
            },
        });
    }

    /// <summary>
    /// Adds security requirements to the OpenAPI document.
    /// </summary>
    private static void AddSecurityRequirements(OpenApiDocument document, Dictionary<string, string> scopes)
    {
        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(Constants.Bearer, document)] = [.. scopes.Keys]
        });
    }
}
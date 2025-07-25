using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

internal sealed class DocumentTransformer(ApiVersion version, MicrosoftIdentityOptions identityOptions) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
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

        // Security requirements
        var baseEndpoint = new Uri(new Uri(identityOptions.Instance), $"{identityOptions.TenantId}/oauth2/v2.0");

        document.Components ??= new();
        document.Components.SecuritySchemes.Add(Constants.Bearer, new()
        {
            Name = "Authorization",
            Scheme = Constants.Bearer,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Flows = new()
            {
                AuthorizationCode = new()
                {
                    AuthorizationUrl = new Uri(baseEndpoint, "authorize"),
                    TokenUrl = new Uri(baseEndpoint, "token"),
                },
            },
        });

        document.SecurityRequirements.Add(new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new()
                    {
                        Id = Constants.Bearer,
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                Array.Empty<string>()
            },
        });

        return Task.CompletedTask;
    }
}
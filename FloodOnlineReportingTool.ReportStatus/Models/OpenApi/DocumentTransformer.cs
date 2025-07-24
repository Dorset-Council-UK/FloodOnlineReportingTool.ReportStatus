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
        document.Info = new(document.Info)
        {
            Version = version.ToString("VVVV", ApiVersionFormatProvider.CurrentCulture),
            Title = $"Flood Online Reporting Tool - Report Status | v{version.ToString("VV", ApiVersionFormatProvider.CurrentCulture)}",
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
        document.Components.SecuritySchemes.Add(OpenApiExtensions.SecuritySchemesName, new()
        {
            Type = SecuritySchemeType.OAuth2,
            BearerFormat = "JWT",
            Scheme = Constants.Bearer,
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
                        Type= ReferenceType.SecurityScheme,
                        Id = OpenApiExtensions.SecuritySchemesName,
                    },
                },
                Array.Empty<string>()
            },
        });

        return Task.CompletedTask;
    }
}
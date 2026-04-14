using Asp.Versioning;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Identity.Web;
using Scalar.AspNetCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class OpenApiExtensions
{
    /// <summary>
    /// Add OpenApi using the versions
    /// </summary>
    /// <remarks>
    ///     <para>See: <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-9.0#use-document-transformers">Use document transformers</see></para>
    /// </remarks>
    internal static IServiceCollection AddReportStatusOpenApi(this IServiceCollection services, MicrosoftIdentityOptions identityOptions)
    {
        foreach (var versionInfo in Versions.All)
        {
            services.AddOpenApi(versionInfo.DocumentName, options =>
            {
                // Set the OpenAPI document info, security schemes, and security requirements
                options.AddDocumentTransformer(new DocumentTransformer(versionInfo.Version, identityOptions));

                // Add default responses for 401 not authorised
                options.AddOperationTransformer(new AuthOperationTransformer());

                options.ShouldInclude = description => ShouldIncludeVersion(description, versionInfo);

                // Add schema transformer to handle specific schema transformations
                options.AddSchemaTransformer<GeometryPointSchemaTransformer>();
            });
        }

        return services;
    }

    /// <summary>
    /// Add API methods to the correct version documentation
    /// </summary>
    private static bool ShouldIncludeVersion(ApiDescription description, VersionInfo versionInfo)
    {
        if (description.GroupName != null && description.GroupName.Equals(versionInfo.DocumentName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var versionMetadata = description.ActionDescriptor.EndpointMetadata
            .OfType<ApiVersionMetadata>()
            .FirstOrDefault();
        if (versionMetadata != null && versionMetadata.IsMappedTo(versionInfo.Version))
        {
            return true;
        }

        return true;
    }

    /// <summary>
    /// Add the Scalar UI <see href="https://localhost:7065/scalar" />
    /// </summary>
    internal static WebApplication UseScalarUI(this WebApplication app, MicrosoftIdentityOptions identityOptions)
    {
        var documentNames = Versions.All.Select(o => o.DocumentName);

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Report Status API")
                .AddDocuments(documentNames)
                .AddPreferredSecuritySchemes(Constants.Bearer)
                .AddOAuth2Flows(Constants.Bearer, flows =>
                {
                    flows.WithAuthorizationCode(options =>
                    {
                        options.ClientId = identityOptions.ClientId;
                        options.Pkce = Pkce.Sha256;
                        options.SelectedScopes = [ $"api://{identityOptions.ClientId}/access_as_user" ];
                    });
                });
        });

        return app;
    }
}

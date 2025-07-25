using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;
using Microsoft.Identity.Web;
using Scalar.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

internal static class OpenApiExtensions
{
    internal const string SecuritySchemesName = "Entra";

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
                // Set the OpenAPI document info
                options.AddDocumentTransformer(new DocumentTransformer(versionInfo.Version, identityOptions));

                // Add API methods to the correct versions by using the group name
                options.ShouldInclude = (description) =>
                    description.GroupName == null ||
                    description.GroupName.Equals(options.DocumentName, StringComparison.OrdinalIgnoreCase);
            });
        }

        return services;
    }

    /// <summary>
    /// Add the Scalar UI <see href="https://localhost:7065/scalar" />
    /// </summary>
    internal static WebApplication UseScalarUI(this WebApplication app, MicrosoftIdentityOptions identityOptions, bool isDevelopment = false)
    {
        var documentNames = Versions.All.Select(o => o.DocumentName);

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Report Status API")
                .WithOperationSorter(OperationSorter.Alpha)
                .AddDocuments(documentNames)
                .AddPreferredSecuritySchemes(SecuritySchemesName)
                .AddOAuth2Flows(SecuritySchemesName, flows =>
                {
                    flows.WithAuthorizationCode(options =>
                    {
                        if (isDevelopment)
                        {
                            options.ClientId = identityOptions.ClientId;
                        }
                        options.Pkce = Pkce.Sha256;
                    });
                });
        });

        return app;
    }

    /// <summary>
    /// Add the Swagger UI <see href="https://localhost:7065/swagger" />
    /// </summary>
    internal static WebApplication UseSwagger(this WebApplication app, MicrosoftIdentityOptions identityOptions, bool isDevelopment = false)
    {
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Report Status API";
            options.EnableTryItOutByDefault();
            foreach (var versionInfo in Versions.All)
            {
                options.SwaggerEndpoint($"/openapi/{versionInfo.DocumentName}.json", $"Report Status API | {versionInfo.DocumentName}");
            }
            if (isDevelopment)
            {
                options.OAuthClientId(identityOptions.ClientId);
            }
            options.OAuthUsePkce();
        });

        return app;
    }
}

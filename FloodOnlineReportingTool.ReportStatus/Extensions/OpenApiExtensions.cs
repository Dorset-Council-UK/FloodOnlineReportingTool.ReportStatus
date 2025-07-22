using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;
using Microsoft.Identity.Web;
using Scalar.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

internal static class OpenApiExtensions
{
    /// <summary>
    /// Add OpenApi using the versions
    /// </summary>
    /// <remarks>
    ///     <para>See: <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-9.0#use-document-transformers">Use document transformers</see></para>
    /// </remarks>
    internal static IServiceCollection AddReportStatusOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        var identityOptions = configuration.GetSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>();
        
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
    internal static WebApplication UseScalarUI(this WebApplication app)
    {
        var documentNames = Versions.All.Select(o => o.DocumentName);

        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Report Status API")
                .WithOperationSorter(OperationSorter.Method)
                .AddDocuments(documentNames)
                .AddPreferredSecuritySchemes(SecuritySchemesName)
                .WithPersistentAuthentication();
        });

        return app;
    }

    internal const string SecuritySchemesName = "Entra ID";
}

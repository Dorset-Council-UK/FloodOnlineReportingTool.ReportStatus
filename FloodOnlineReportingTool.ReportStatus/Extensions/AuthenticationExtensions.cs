using FloodOnlineReportingTool.ReportStatus.Authorization;
using Microsoft.Identity.Web;

namespace Microsoft.AspNetCore.Builder;

internal static class AuthenticationExtensions
{
    /// <summary>
    ///     <para>Configures authentication, authorization, and policies for report status.</para>
    ///     <para>Sets up Identity / Entra / Bearer authentication for role based access, using C# policies.</para>
    /// </summary>
    internal static IServiceCollection AddReportStatusAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Resilient HTTP client for authentication purposes. For example: retrying when .well-known fails to read.
        const string clientName = "OAuthResilient";
        services
            .AddHttpClient(clientName)
            .AddStandardResilienceHandler();

        // Add authentication
        services
            .AddAuthentication(Constants.Bearer)
            .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
            {
                configuration.Bind(Constants.AzureAd, jwtBearerOptions);
                var httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
                jwtBearerOptions.Backchannel = httpClientFactory.CreateClient(clientName);
            },
            microsoftIdentityOptions =>
            {
                configuration.Bind(Constants.AzureAd, microsoftIdentityOptions);
            });

        // Add authorization
        services
            .AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.Reader, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                    context.User.IsInRole(RoleNames.Reader) ||
                    context.User.IsInRole(RoleNames.Writer) ||
                    context.User.IsInRole(RoleNames.Admin)))
            .AddPolicy(PolicyNames.Writer, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                    context.User.IsInRole(RoleNames.Writer) ||
                    context.User.IsInRole(RoleNames.Admin)))
            .AddPolicy(PolicyNames.Admin, policy => policy
                .RequireAuthenticatedUser()
                .RequireRole(RoleNames.Admin));

        return services;
    }
}

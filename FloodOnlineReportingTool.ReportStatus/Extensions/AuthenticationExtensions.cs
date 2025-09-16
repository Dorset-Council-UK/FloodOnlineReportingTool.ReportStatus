using FloodOnlineReportingTool.ReportStatus.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class AuthenticationExtensions
{
    /// <summary>
    ///     <para>Configures authentication, authorization, and policies for report status.</para>
    ///     <para>Sets up Identity / Entra / Bearer authentication for role based access, using C# policies.</para>
    /// </summary>
    internal static IServiceCollection AddReportStatusAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Add authentication
        services
            .AddAuthentication(Constants.Bearer)
            .AddMicrosoftIdentityWebApi(configuration);

        ConfigureResilientJwtBearerOptions(services);

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

    /// <summary>
    /// Configure a resilient JWT Bearer Options authentication handler.
    /// </summary>
    /// <remarks>For example, retrying when .well-known fails to read.</remarks>
    private static void ConfigureResilientJwtBearerOptions(IServiceCollection services)
    {
        const string clientName = "OAuthResilient";

        services
            .AddHttpClient(clientName)
            .AddStandardResilienceHandler();

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IHttpClientFactory>((options, httpClientFactory) =>
            {
                options.Backchannel = httpClientFactory.CreateClient(clientName);
            });
    }
}

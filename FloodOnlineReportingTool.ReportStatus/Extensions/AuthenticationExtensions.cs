using FloodOnlineReportingTool.ReportStatus.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace Microsoft.AspNetCore.Builder;

internal static class AuthenticationExtensions
{
    /// <summary>
    ///     <para>Configures authentication, authorization, and policies for report status.</para>
    ///     <para>Sets up Identity / Entra / Bearer authentication for role based access, using C# policies for nice integration with minimal APIs.</para>
    /// </summary>
    internal static IServiceCollection AddReportStatusAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Build the admin policy
        var adminPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(RoleNames.Admin)
            .Build();

        // Add authentication
        services
            .AddAuthentication(Constants.Bearer)
            .AddMicrosoftIdentityWebApi(configuration);

        // Add authorization
        services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy(PolicyNames.Admin, adminPolicy);

        return services;
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Repositories;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class RepositoryExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ISearchRepository, SearchRepository>();

        return services;
    }
}

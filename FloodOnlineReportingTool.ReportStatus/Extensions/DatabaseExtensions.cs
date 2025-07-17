using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Microsoft.AspNetCore.Builder;

internal static class DatabaseExtensions
{
    internal static IServiceCollection AddReportStatusDatabase(this IServiceCollection services, string? connectionString, bool logSensitiveData = false)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ConfigurationMissingException("Missing configuration setting: The connection string for the database is missing.");
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            IncludeErrorDetail = logSensitiveData,
        };

        // Double check that a schema has been set
        if (string.IsNullOrWhiteSpace(builder.SearchPath))
        {
            throw new ConfigurationMissingException("Missing configuration setting: The connection string for the database needs a schema set in the Search Path parameter.");
        }

        // Add the database context
        services.AddDbContextPool<ApplicationDbContext>(
            options =>
            {
                options.UseNpgsql(builder.ToString(), o =>
                {
                    o.EnableRetryOnFailure(3);
                    o.MigrationsHistoryTable("__EFMigrationsHistory", builder.SearchPath);
                });
                options.EnableSensitiveDataLogging(logSensitiveData);
            });

        return services;
    }
}

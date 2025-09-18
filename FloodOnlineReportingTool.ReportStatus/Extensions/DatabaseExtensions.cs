using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class DatabaseExtensions
{
    internal static TBuilder AddReportStatusDatabase<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var logSensitiveData = builder.Environment.IsDevelopment();
        var connectionStringBuilder = builder.Configuration.GetReportStatusConnectionString(logSensitiveData);
        
        // Add the database context
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionStringBuilder.ToString(), o =>
            {
                o.EnableRetryOnFailure(3);
                o.MigrationsHistoryTable("__EFMigrationsHistory", connectionStringBuilder.SearchPath);
                o.UseNetTopologySuite();
            });
            options.EnableSensitiveDataLogging(logSensitiveData);
        });

        // Database services
        builder.Services
            .AddScoped<IReportService, ReportService>()
            .AddScoped<IReportDuplicateService, ReportDuplicateService>()
            .AddScoped<IReportSourceService, ReportSourceService>()
            .AddScoped<ISearchService, SearchService>();

        return builder;
    }

    private static NpgsqlConnectionStringBuilder GetReportStatusConnectionString(this IConfigurationManager configuration, bool logSensitiveData)
    {
        var connectionString = configuration.GetConnectionString("FloodReportingReportStatus");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ConfigurationMissingException("Missing configuration setting: The connection string for the database is missing.");
        }

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            IncludeErrorDetail = logSensitiveData,
        };

        // Double check that a schema has been set
        if (string.IsNullOrWhiteSpace(connectionStringBuilder.SearchPath))
        {
            throw new ConfigurationMissingException("Missing configuration setting: The connection string for the database needs a schema set in the Search Path parameter.");
        }
        if (!connectionStringBuilder.SearchPath.Equals(SchemaNames.ReportStatus, StringComparison.Ordinal))
        {
            throw new ConfigurationMissingException($"Invalid configuration setting: The connection string for the database has an invalid schema set in the Search Path parameter. Expected '{SchemaNames.ReportStatus}', but found '{connectionStringBuilder.SearchPath}'.");
        }

        return connectionStringBuilder;
    }
}

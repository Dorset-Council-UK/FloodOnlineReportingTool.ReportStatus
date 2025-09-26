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
        var connectionString = builder.Configuration.GetReportStatusConnectionString(logSensitiveData);

        // Add the database context
        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o =>
            {
                o.EnableRetryOnFailure(3);
                o.MigrationsHistoryTable("__EFMigrationsHistory", SchemaNames.ReportStatus);
                o.UseNetTopologySuite();
            });
            options.EnableSensitiveDataLogging(logSensitiveData);
        });

        // Database services
        builder.Services
            .AddScoped<IFloodLocationService, FloodLocationService>()
            .AddScoped<IFloodLocationFloodReportService, FloodLocationFloodReportService>()
            .AddScoped<IFloodPropertyService, FloodPropertyService>()
            .AddScoped<IFloodPropertyFloodReportService, FloodPropertyFloodReportService>()
            .AddScoped<IFloodRoadService, FloodRoadService>()
            .AddScoped<IFloodRoadFloodReportService, FloodRoadFloodReportService>()
            .AddScoped<IFloodReportService, FloodReportService>()
            .AddScoped<IFloodReportSourceService, FloodReportSourceService>()
            .AddScoped<ISearchService, SearchService>();

        return builder;
    }

    private static string GetReportStatusConnectionString(this IConfigurationManager configuration, bool logSensitiveData)
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
        var hasSchema1 = connectionStringBuilder.SearchPath.Contains(SchemaNames.ReportStatus, StringComparison.Ordinal);
        var hasSchema2 = connectionStringBuilder.SearchPath.Contains(SchemaNames.Public, StringComparison.Ordinal);
        if (!(hasSchema1 && hasSchema2))
        {
            throw new ConfigurationMissingException($"Invalid configuration setting: The connection string for the database has an invalid schema set in the Search Path parameter. Expected '{SchemaNames.ReportStatus}' and '{SchemaNames.Public}', but found '{connectionStringBuilder.SearchPath}'.");
        }

        return connectionStringBuilder.ToString();
    }
}

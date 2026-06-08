using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Outbox;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

//builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

// add database
string? connectionString = builder.Configuration.GetConnectionString(ConnectionStringNames.ReportStatus);
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception($"Missing configuration setting: The database connection string '{ConnectionStringNames.ReportStatus}' is missing.");
}
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

// add service bus
if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString(ConnectionStringNames.ServiceBus)))
{
    throw new Exception($"Missing configuration setting: The service bus connection string '{ConnectionStringNames.ServiceBus}' is missing.");
}
builder.AddAzureServiceBusClient(ConnectionStringNames.ServiceBus);

await builder.Build().RunAsync();
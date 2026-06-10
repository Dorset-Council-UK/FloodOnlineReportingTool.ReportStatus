using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Outbox;
using ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<OutboxWorker>();
builder.Services.AddHostedService<InboxWorker>();
builder.Services
    .AddScoped<IFloodReportRepository, FloodReportRepository>()
    .AddScoped<IFloodReportSourceDuplicateRepository, FloodReportSourceDuplicateRepository>()
    .AddScoped<IFloodReportService, FloodReportService>();

// add database
string? connectionString = builder.Configuration.GetConnectionString(ConnectionStringNames.ReportStatus);
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception($"Missing configuration setting: The database connection string '{ConnectionStringNames.ReportStatus}' is missing.");
}
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.UseNetTopologySuite()));

// add service bus
if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString(ConnectionStringNames.ServiceBus)))
{
    throw new Exception($"Missing configuration setting: The service bus connection string '{ConnectionStringNames.ServiceBus}' is missing.");
}
string? serviceBusConnectionString = builder.Configuration.GetConnectionString(ConnectionStringNames.ServiceBus);
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder
        .AddServiceBusClient(serviceBusConnectionString)
        .ConfigureOptions(options =>
        {
            options.TransportType = ServiceBusTransportType.AmqpWebSockets;
        });
});
await builder.Build().RunAsync();
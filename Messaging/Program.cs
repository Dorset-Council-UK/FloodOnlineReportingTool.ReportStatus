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
string? databaseConnectionString = builder.Configuration.GetConnectionString("FloodReportingReportStatus");
if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new InvalidOperationException("Missing configuration setting: The database connection string 'FloodReportingReportStatus' is missing.");
}
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(databaseConnectionString, npgsqlOptions => npgsqlOptions.UseNetTopologySuite()));

// add service bus
string? serviceBusConnectionString = builder.Configuration.GetConnectionString("service-bus");
if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
{
    throw new InvalidOperationException("Missing configuration setting: The service bus connection string 'service-bus' is missing.");
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
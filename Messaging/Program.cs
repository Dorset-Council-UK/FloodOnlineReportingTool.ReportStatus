using Azure.Messaging.ServiceBus;
using FloodOnlineReportingTool.Database.DbContexts;
using FloodOnlineReportingTool.Database.Repositories;
using FloodOnlineReportingTool.Database.Services;
using Messaging;
using Messaging.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = Host.CreateApplicationBuilder(args);

// Services
builder.Services
    .AddScoped<IFloodEventService, FloodEventService>()
    .AddScoped<IFloodEventRepository, FloodEventRepository>()
    .AddScoped<IFloodReportService, FloodReportService>()
    .AddScoped<IFloodReportRepository, FloodReportRepository>()
    .AddScoped<IFloodReportSourceDuplicateRepository, FloodReportSourceDuplicateRepository>();

// Consumers
builder.Services
    .AddSingleton<IInboxConsumer, FloodEventCreatedConsumer>()
    .AddSingleton<IInboxConsumer, FloodEventS19TriggeredConsumer>()
    .AddSingleton<IInboxConsumer, FloodEventUpdatedConsumer>()
    .AddSingleton<IInboxConsumer, FloodSourceCreatedConsumer>();

builder.Services.AddHostedService<OutboxWorker>();
builder.Services.AddHostedService<InboxWorker>();

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
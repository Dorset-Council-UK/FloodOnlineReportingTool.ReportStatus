using FloodOnlineReportingTool.ReportStatus.Services;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.Json;
using NetTopologySuite.IO.Converters;

var builder = WebApplication.CreateBuilder(args);

// Configure all the settings.
var (identityOptions, messagingSettings) = builder.Services.AddReportStatusSettings(builder.Configuration);

builder.Services.AddReportStatusAuthentication(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddReportStatusHealthChecks(messagingSettings);
builder.Services.AddReportStatusVersioning();
builder.Services.AddReportStatusOpenApi(identityOptions);
builder.AddReportStatusDatabase();
builder.Services.AddMessageSystem(messagingSettings);
builder.AddGovNotify();

// Add services to DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IGazetteerSearchService, GazetteerSearchService>();
builder.Services.AddScoped<ISearchFloodLocationService, SearchFloodLocationService>();
builder.Services.AddScoped<ISearchFloodReportService, SearchFloodReportService>();

// json serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new GeoJsonConverterFactory());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseScalarUI(identityOptions);
    app.UseSwagger(identityOptions);
}

app.UseHttpsRedirection();
app.MapReportStatusHealthChecks();
app.MapReportStatusEndpoints();

await app
    .RunAsync()
    .ConfigureAwait(false);
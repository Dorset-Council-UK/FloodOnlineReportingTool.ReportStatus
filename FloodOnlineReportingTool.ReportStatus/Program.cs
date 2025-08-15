using FloodOnlineReportingTool.ReportStatus.Database.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure all the settings.
var (identityOptions, messagingSettings) = builder.Services.AddReportStatusSettings(builder.Configuration);

builder.Services.AddReportStatusAuthentication(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddReportStatusHealthChecks(messagingSettings);
builder.Services.AddReportStatusVersioning();
builder.Services.AddReportStatusOpenApi(identityOptions);
builder.Services.AddReportStatusDatabase(builder.Configuration.GetConnectionString("FloodReportingReportStatus"), builder.Environment.IsDevelopment());
builder.Services.AddMessageSystem(messagingSettings);

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportDuplicateService, ReportDuplicateService>();
builder.Services.AddScoped<ISearchService, SearchService>();

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
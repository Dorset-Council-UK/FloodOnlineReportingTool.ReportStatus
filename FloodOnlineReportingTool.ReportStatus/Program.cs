using FloodOnlineReportingTool.ReportStatus.Database.Services;

var builder = WebApplication.CreateBuilder(args);

var messagingSettings = builder.Services.AddReportStatusSettings(builder.Configuration);

builder.Services.AddReportStatusAuthentication(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddReportStatusHealthChecks(messagingSettings);
builder.Services.AddReportStatusVersioning();
builder.Services.AddReportStatusOpenApi(builder.Configuration);
builder.Services.AddReportStatusDatabase(builder.Configuration.GetConnectionString("FloodReportingReportStatus"), builder.Environment.IsDevelopment());
builder.Services.AddMessageSystem(messagingSettings);

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportDuplicateService, ReportDuplicateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseScalarUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapReportStatusHealthChecks();
app.MapReportStatusEndpoints();

await app
    .RunAsync()
    .ConfigureAwait(false);
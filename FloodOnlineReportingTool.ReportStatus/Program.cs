var builder = WebApplication.CreateBuilder(args);

var messagingSettings = builder.Services.AddReportStatusSettings(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddReportStatusHealthChecks(messagingSettings);
builder.Services.AddReportStatusVersioning();
builder.Services.AddReportStatusOpenApi();
builder.Services.AddReportStatusDatabase(builder.Configuration.GetConnectionString("FloodReportingReport"), builder.Environment.IsDevelopment());
builder.Services.AddMessageSystem(messagingSettings);

//builder.Services.AddScoped<ReportStatusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseScalarUI();
}

app.UseHttpsRedirection();
app.MapReportStatusHealthChecks();
app.MapReportStatusEndpoints();

await app
    .RunAsync()
    .ConfigureAwait(false);
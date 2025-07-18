using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;

var builder = WebApplication.CreateBuilder(args);

var messagingSettings = builder.Services.AddReportStatusSettings(builder.Configuration);

// Add authentication
builder.Services
    .AddAuthentication()
    .AddBearerToken();

// Add authorization
builder.Services
    .AddAuthorizationBuilder()
    .AddDefaultPolicy(PolicyNames.Admin, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole(RoleNames.Admin);
        //policy.AddAuthenticationSchemes(BearerTokenDefaults.AuthenticationScheme);
    });

builder.Services.AddProblemDetails();
builder.Services.AddReportStatusHealthChecks(messagingSettings);
builder.Services.AddReportStatusVersioning();
builder.Services.AddReportStatusOpenApi();
builder.Services.AddReportStatusDatabase(builder.Configuration.GetConnectionString("FloodReportingReportStatus"), builder.Environment.IsDevelopment());
builder.Services.AddMessageSystem(messagingSettings);

builder.Services.AddScoped<IReportService, ReportService>();

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
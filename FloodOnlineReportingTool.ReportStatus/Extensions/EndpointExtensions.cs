using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

namespace Microsoft.AspNetCore.Builder;

internal static class EndpointExtensions
{
    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        MapReportEndpoints(app);
        MapDuplicateEndpoints(app);

        return app;
    }

    private static void MapReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports")
            .RequireAuthorization(PolicyNames.Admin)
            .HasApiVersion(Versions.V1.Version);

        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetReports)
            .WithSummary("Get all reports")
            .WithDescription("Gets all reports from the database");

        group
            .MapGet(pattern: "/{id}", ReportStatusEndpoints.GetReportById)
            .WithSummary("Get report")
            .WithDescription("Gets a report by id from the database");

        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateReport)
            .WithSummary("Create report")
            .WithDescription("Creates a new report in the database");

        group
            .MapPut(pattern: "/{id}", ReportStatusEndpoints.UpdateReport)
            .WithSummary("Update report")
            .WithDescription("Updates a report by id in the database");

        group
            .MapDelete(pattern: "/{id}", ReportStatusEndpoints.DeleteReport)
            .WithSummary("Delete report")
            .WithDescription("Deletes a report by id from the database");

        group
            .MapGet(pattern: "/test", ReportStatusEndpoints.Test)
            .WithSummary("Test")
            .AllowAnonymous();
    }

    private static void MapDuplicateEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports/duplicates")
            .RequireAuthorization(PolicyNames.Admin)
            .HasApiVersion(Versions.V1.Version);

        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetDuplicateReports)
            .WithSummary("Get all duplicate reports")
            .WithDescription("Gets all duplicate reports from the database");

        group
            .MapGet(pattern: "/{id}", ReportStatusEndpoints.GetDuplicateReportById)
            .WithSummary("Get duplicate report")
            .WithDescription("Gets a duplicate report by id from the database");

        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateDuplicateReport)
            .WithSummary("Create duplicate report")
            .WithDescription("Creates a new duplicate report in the database");

        group
            .MapPut(pattern: "/{id}", ReportStatusEndpoints.UpdateDuplicateReport)
            .WithSummary("Update duplicate report")
            .WithDescription("Updates a duplicate report by id in the database");

        group
            .MapDelete(pattern: "/{id}", ReportStatusEndpoints.DeleteDuplicateReport)
            .WithSummary("Delete duplicate report")
            .WithDescription("Deletes a duplicate report by id from the database");
    }
}

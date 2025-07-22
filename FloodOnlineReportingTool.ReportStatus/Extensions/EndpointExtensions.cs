using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

namespace Microsoft.AspNetCore.Builder;

internal static class EndpointExtensions
{
    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports")
            .RequireAuthorization(PolicyNames.Admin)
            //.AllowAnonymous()
            .HasApiVersion(Versions.V1.Version);

        MapReportEndpoints(group);
        MapDuplicateEndpoints(group);

        return app;
    }

    private static void MapReportEndpoints(RouteGroupBuilder group)
    {
        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetReports)
            .WithSummary("Reports - All")
            .WithDescription("Gets all reports from the database");

        group
            .MapGet(pattern: "/{id}", ReportStatusEndpoints.GetReportById)
            .WithSummary("Reports - Get")
            .WithDescription("Gets a report by id from the database");

        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateReport)
            .WithSummary("Reports - Create")
            .WithDescription("Creates a new report in the database");

        group
            .MapPut(pattern: "/{id}", ReportStatusEndpoints.UpdateReport)
            .WithSummary("Reports - Update")
            .WithDescription("Updates a report by id in the database");

        group
            .MapDelete(pattern: "/{id}", ReportStatusEndpoints.DeleteReport)
            .WithSummary("Reports - Delete")
            .WithDescription("Deletes a report by id from the database");

        group
            .MapGet(pattern: "/test", ReportStatusEndpoints.Test)
            .WithSummary("Test")
            .AllowAnonymous();
    }

    private static void MapDuplicateEndpoints(RouteGroupBuilder group)
    {
        group
            .MapGet(pattern: "/duplicates", ReportStatusEndpoints.GetDuplicateReports)
            .WithSummary("Duplicates - All")
            .WithDescription("Gets all duplicate reports from the database");

        group
            .MapGet(pattern: "/duplicates/{id}", ReportStatusEndpoints.GetDuplicateReportById)
            .WithSummary("Duplicates - Get")
            .WithDescription("Gets a duplicate report by id from the database");

        group
            .MapGet(pattern: "/{reportId}/duplicates", ReportStatusEndpoints.GetReportDuplicates)
            .WithSummary("Reports - Duplicates")
            .WithDescription("Gets all duplicates for a report from the database");

        group
            .MapPost(pattern: "/duplicates", ReportStatusEndpoints.CreateDuplicateReport)
            .WithSummary("Duplicates - Create")
            .WithDescription("Creates a new duplicate report in the database");

        group
            .MapPut(pattern: "/duplicates/{id}", ReportStatusEndpoints.UpdateDuplicateReport)
            .WithSummary("Duplicates - Update")
            .WithDescription("Updates a duplicate report by id in the database");

        group
            .MapDelete(pattern: "/duplicates/{id}", ReportStatusEndpoints.DeleteDuplicateReport)
            .WithSummary("Duplicates - Delete")
            .WithDescription("Deletes a duplicate report by id from the database");
    }
}

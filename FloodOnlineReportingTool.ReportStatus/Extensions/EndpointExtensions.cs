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
            .HasApiVersion(Versions.V1.Version);

        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetReports)
            .WithSummary("Get all")
            .WithDescription("Gets all reports from the database")
            .HasApiVersion(1, 0);

        group
            .MapGet(pattern: "/{id}", ReportStatusEndpoints.GetReportById)
            .WithSummary("Get")
            .WithDescription("Gets a report by id from the database");

        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateReport)
            .WithSummary("Create")
            .WithDescription("Creates a new report in the database");

        group
            .MapPut(pattern: "/{id}", ReportStatusEndpoints.UpdateReport)
            .WithSummary("Update")
            .WithDescription("Updates a report by id in the database");

        group
            .MapDelete(pattern: "/{id}", ReportStatusEndpoints.DeleteReport)
            .WithSummary("Delete")
            .WithDescription("Deletes a report by id from the database");

        return app;
    }
}

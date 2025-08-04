using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

namespace Microsoft.AspNetCore.Builder;

internal static class EndpointExtensions
{
    internal const string tagReport = "Reports";
    internal const string tagReportDuplicate = "Report Duplicates";
    internal const string tagDuplicate = "Duplicates";

    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        var reportsGroup = app
            .NewVersionedApi()
            .MapGroup("/api/reports")
            .HasApiVersion(Versions.V1.Version);

        var reportDuplicatesGroup = app
            .NewVersionedApi()
            .MapGroup("/api/reports/{reportId}/duplicates")
            .HasApiVersion(Versions.V1.Version);

        var duplicateGroup = app
            .NewVersionedApi()
            .MapGroup("/api/duplicates")
            .HasApiVersion(Versions.V1.Version);

        MapReportEndpoints(reportsGroup);
        MapReportDuplicateEndpoints(reportDuplicatesGroup);
        MapDuplicateEndpoints(duplicateGroup);

        return app;
    }

    private static void MapReportEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetReports)
            .WithSummary("Reports - All")
            .WithDescription("Gets all reports from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", ReportStatusEndpoints.GetReportById)
            .WithSummary("Reports - Get")
            .WithDescription("Gets a report by id from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateReport)
            .WithSummary("Reports - Create")
            .WithDescription("Creates a new report in the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", ReportStatusEndpoints.UpdateReport)
            .WithSummary("Reports - Update")
            .WithDescription("Updates a report by id in the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", ReportStatusEndpoints.DeleteReport)
            .WithSummary("Reports - Delete")
            .WithDescription("Deletes a report by id from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapReportDuplicateEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetReportDuplicates)
            .WithSummary("Reports - Duplicates")
            .WithDescription("Gets all duplicates associated with a report from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportStatusEndpoints.GetReportDuplicate)
            .WithSummary("Reports - Get Duplicate")
            .WithDescription("Gets a duplicate associated with a report by id from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportStatusEndpoints.CreateReportDuplicate)
            .WithSummary("Reports - Create Duplicate")
            .WithDescription("Creates a new duplicate associated with a report in the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportStatusEndpoints.UpdateReportDuplicate)
            .WithSummary("Reports - Update Duplicate")
            .WithDescription("Updates a duplicate associated with a report by id in the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportStatusEndpoints.DeleteReportDuplicate)
            .WithSummary("Reports - Delete Duplicate")
            .WithDescription("Deletes a duplicate associated with a report by id from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapDuplicateEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportStatusEndpoints.GetDuplicates)
            .WithSummary("Duplicates - All")
            .WithDescription("Gets all duplicate reports from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportStatusEndpoints.GetDuplicate)
            .WithSummary("Duplicates - Get")
            .WithDescription("Gets a duplicate report by id from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportStatusEndpoints.UpdateDuplicate)
            .WithSummary("Duplicates - Update")
            .WithDescription("Updates a duplicate report by id in the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportStatusEndpoints.DeleteDuplicate)
            .WithSummary("Duplicates - Delete")
            .WithDescription("Deletes a duplicate report by id from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Writer);
    }
}

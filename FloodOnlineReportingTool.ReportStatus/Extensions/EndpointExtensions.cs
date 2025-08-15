using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

namespace Microsoft.AspNetCore.Builder;

internal static class EndpointExtensions
{
    internal const string tagReport = "Reports";
    internal const string tagReportDuplicate = "Report Duplicates";
    internal const string tagDuplicate = "Duplicates";
    internal const string tagSearch = "Search";

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

        var searchGroup = app
            .NewVersionedApi()
            .MapGroup("/api/search")
            .HasApiVersion(Versions.V1.Version);

        MapReportEndpoints(reportsGroup);
        MapReportDuplicateEndpoints(reportDuplicatesGroup);
        MapDuplicateEndpoints(duplicateGroup);
        MapSearchEndpoints(searchGroup);

        return app;
    }

    private static void MapReportEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportEndpoints.GetReports)
            .WithSummary("Reports - All")
            .WithDescription("Gets all reports from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", ReportEndpoints.GetReportById)
            .WithSummary("Reports - Get")
            .WithDescription("Gets a report by id from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportEndpoints.CreateReport)
            .WithSummary("Reports - Create")
            .WithDescription("Creates a new report in the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", ReportEndpoints.UpdateReport)
            .WithSummary("Reports - Update")
            .WithDescription("Updates a report by id in the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", ReportEndpoints.DeleteReport)
            .WithSummary("Reports - Delete")
            .WithDescription("Deletes a report by id from the database")
            .WithTags(tagReport)
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapReportDuplicateEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportDuplicateEndpoints.GetReportDuplicates)
            .WithSummary("Reports - Duplicates")
            .WithDescription("Gets all duplicates associated with a report from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportDuplicateEndpoints.GetReportDuplicate)
            .WithSummary("Reports - Get Duplicate")
            .WithDescription("Gets a duplicate associated with a report by id from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportDuplicateEndpoints.CreateReportDuplicate)
            .WithSummary("Reports - Create Duplicate")
            .WithDescription("Creates a new duplicate associated with a report in the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportDuplicateEndpoints.UpdateReportDuplicate)
            .WithSummary("Reports - Update Duplicate")
            .WithDescription("Updates a duplicate associated with a report by id in the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportDuplicateEndpoints.DeleteReportDuplicate)
            .WithSummary("Reports - Delete Duplicate")
            .WithDescription("Deletes a duplicate associated with a report by id from the database")
            .WithTags(tagReportDuplicate)
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapDuplicateEndpoints(RouteGroupBuilder group)
    {
        // Get all
        group
            .MapGet(pattern: "/", ReportDuplicateEndpoints.GetDuplicates)
            .WithSummary("Duplicates - All")
            .WithDescription("Gets all duplicate reports from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportDuplicateEndpoints.GetDuplicate)
            .WithSummary("Duplicates - Get")
            .WithDescription("Gets a duplicate report by id from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Reader);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportDuplicateEndpoints.UpdateDuplicate)
            .WithSummary("Duplicates - Update")
            .WithDescription("Updates a duplicate report by id in the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportDuplicateEndpoints.DeleteDuplicate)
            .WithSummary("Duplicates - Delete")
            .WithDescription("Deletes a duplicate report by id from the database")
            .WithTags(tagDuplicate)
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapSearchEndpoints(RouteGroupBuilder group)
    {
        // Search reports by UPRN
        group
            .MapGet(pattern: "/uprn", SearchEndpoints.GetReportsByUPRN)
            .WithSummary("Search - Reports by UPRN")
            .WithDescription("Searches for reports based on the provided UPRN")
            .WithTags(tagSearch)
            .RequireAuthorization(PolicyNames.Reader);

        // Search reports by coordinates
        group
            .MapGet(pattern: "/coordinates", SearchEndpoints.GetReportsByCoordinates)
            .WithSummary("Search - Reports by Coordinates")
            .WithDescription("Searches for reports based on the provided Easting and Northing coordinates")
            .WithTags(tagSearch)
            .RequireAuthorization(PolicyNames.Reader);

        // Search reports by start date
        group
            .MapGet(pattern: "/startdate", SearchEndpoints.GetReportsByStartDate)
            .WithSummary("Search - Reports by Start Date")
            .WithDescription("Searches for reports based on the date the flood started")
            .WithTags(tagSearch)
            .RequireAuthorization(PolicyNames.Reader);
    }
}

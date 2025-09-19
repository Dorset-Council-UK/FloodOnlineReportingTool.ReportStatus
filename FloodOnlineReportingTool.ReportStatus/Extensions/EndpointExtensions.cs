using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class EndpointExtensions
{
    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        MapFloodLocationEndpoints(app);

        MapReportEndpoints(app);
        MapReportDuplicateEndpoints(app);
        MapReportSourceEndpoints(app);
        MapDuplicateEndpoints(app);
        MapSearchEndpoints(app);
        MapTestEndpoints(app);

        return app;
    }

    private static void MapFloodLocationEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/location")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Location");

        // Get all
        group
            .MapGet(pattern: "/", FloodLocationEndpoints.GetFloodLocations)
            .WithSummary("Flood location - All")
            .WithDescription("Gets all flood locations from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/with-reports/", FloodLocationEndpoints.GetFloodLocationsWithReports)
            .WithSummary("Flood location - All with flood reports")
            .WithDescription("Gets all flood locations and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", FloodLocationEndpoints.GetFloodLocation)
            .WithSummary("Flood location - Get")
            .WithDescription("Gets a flood location by id from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/{id}/with-reports/", FloodLocationEndpoints.GetFloodLocationWithReports)
            .WithSummary("Flood location - Get with flood reports")
            .WithDescription("Gets a flood location by id and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", FloodLocationEndpoints.CreateFloodLocation)
            .WithSummary("Flood location - Create")
            .WithDescription("Creates a new flood location in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", FloodLocationEndpoints.UpdateFloodLocation)
            .WithSummary("Flood location - Update")
            .WithDescription("Updates a flood location by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", FloodLocationEndpoints.DeleteFloodLocation)
            .WithSummary("Flood location - Delete")
            .WithDescription("Deletes a flood location by id from the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapDelete(pattern: "/{id}/exists", FloodLocationEndpoints.FloodLocationExists)
            .WithSummary("Flood location - Exists")
            .WithDescription("Checks if a flood location exists in the database")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Reports");

        // Get all
        group
            .MapGet(pattern: "/", ReportEndpoints.GetReports)
            .WithSummary("Reports - All")
            .WithDescription("Gets all reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", ReportEndpoints.GetReportById)
            .WithSummary("Reports - Get")
            .WithDescription("Gets a report by id from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportEndpoints.CreateReport)
            .WithSummary("Reports - Create")
            .WithDescription("Creates a new report in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", ReportEndpoints.UpdateReport)
            .WithSummary("Reports - Update")
            .WithDescription("Updates a report by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", ReportEndpoints.DeleteReport)
            .WithSummary("Reports - Delete")
            .WithDescription("Deletes a report by id from the database")
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapReportDuplicateEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports/{reportId}/duplicates")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Report Duplicates");

        // Get all
        group
            .MapGet(pattern: "/", ReportDuplicateEndpoints.GetReportDuplicates)
            .WithSummary("Reports - Duplicates")
            .WithDescription("Gets all duplicates associated with a report from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportDuplicateEndpoints.GetReportDuplicate)
            .WithSummary("Reports - Get Duplicate")
            .WithDescription("Gets a duplicate associated with a report by id from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", ReportDuplicateEndpoints.CreateReportDuplicate)
            .WithSummary("Reports - Create Duplicate")
            .WithDescription("Creates a new duplicate associated with a report in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportDuplicateEndpoints.UpdateReportDuplicate)
            .WithSummary("Reports - Update Duplicate")
            .WithDescription("Updates a duplicate associated with a report by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportDuplicateEndpoints.DeleteReportDuplicate)
            .WithSummary("Reports - Delete Duplicate")
            .WithDescription("Deletes a duplicate associated with a report by id from the database")
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapReportSourceEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/reports/{reportId}/sources")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Report Sources");

        // Get all
        group
            .MapGet(pattern: "/", ReportSourceEndpoints.GetReportSources)
            .WithSummary("Reports - Sources")
            .WithDescription("Gets all flood sources associated with a report from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{sourceId}", ReportSourceEndpoints.GetReportSource)
            .WithSummary("Reports - Get Source")
            .WithDescription("Gets a flood source associated with a report from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{sourceId}", ReportSourceEndpoints.CreateReportSource)
            .WithSummary("Reports - Create Source")
            .WithDescription("Creates a new flood source associated with a report in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{sourceId}", ReportSourceEndpoints.UpdateReportSource)
            .WithSummary("Reports - Update Source")
            .WithDescription("Updates a source associated with a report in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{sourceId}", ReportSourceEndpoints.DeleteReportSource)
            .WithSummary("Reports - Delete Source")
            .WithDescription("Deletes a flood source associated with a report from the database")
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapDuplicateEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/duplicates")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Duplicates");

        // Get all
        group
            .MapGet(pattern: "/", ReportDuplicateEndpoints.GetDuplicates)
            .WithSummary("Duplicates - All")
            .WithDescription("Gets all duplicate reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{duplicateId}", ReportDuplicateEndpoints.GetDuplicate)
            .WithSummary("Duplicates - Get")
            .WithDescription("Gets a duplicate report by id from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Update
        group
            .MapPut(pattern: "/{duplicateId}", ReportDuplicateEndpoints.UpdateDuplicate)
            .WithSummary("Duplicates - Update")
            .WithDescription("Updates a duplicate report by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{duplicateId}", ReportDuplicateEndpoints.DeleteDuplicate)
            .WithSummary("Duplicates - Delete")
            .WithDescription("Deletes a duplicate report by id from the database")
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapSearchEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/search")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Search");

        // Search reports by UPRN
        group
            .MapGet(pattern: "/uprn", SearchEndpoints.GetReportsByUPRN)
            .WithSummary("Search - Reports by UPRN")
            .WithDescription("Searches for reports based on the provided UPRN")
            .RequireAuthorization(PolicyNames.Reader);

        // Search reports by coordinates
        group
            .MapGet(pattern: "/coordinates", SearchEndpoints.GetReportsByCoordinates)
            .WithSummary("Search - Reports by Coordinates")
            .WithDescription("Searches for reports based on the provided Easting and Northing coordinates")
            .RequireAuthorization(PolicyNames.Reader);

        // Search reports by start date
        group
            .MapGet(pattern: "/startdate", SearchEndpoints.GetReportsByStartDate)
            .WithSummary("Search - Reports by Start Date")
            .WithDescription("Searches for reports based on the date the flood started")
            .RequireAuthorization(PolicyNames.Reader);
    }
    
    private static void MapTestEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/test")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Testing");

        group
            .MapGet(pattern: "/notifications/vulnerable", TestEndpoints.TestVulnerableNotification)
            .WithSummary("Test the vulnerable notification")
            .WithDescription("Test the vulnerable notification Gov Notify functionality")
            .RequireAuthorization(PolicyNames.Admin);
    }
}

using FloodOnlineReportingTool.ReportStatus.Authorization;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Builder;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class EndpointExtensions
{
    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        MapFloodReportEndpoints(app);
        MapFloodReportDuplicateEndpoints(app);
        MapFloodReportSourceEndpoints(app);

        MapSearchFloodReportEndpoints(app);

        MapTestEndpoints(app);

        return app;
    }

    private static void MapFloodReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/reports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Reports");

        // Get all
        group
            .MapGet(pattern: "/", FloodReportEndpoints.GetFloodReports)
            .WithSummary("Flood report - All")
            .WithDescription("Gets all flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/uprn", FloodReportEndpoints.GetFloodReportsByUprn)
            .WithSummary("Flood report - All")
            .WithDescription("Gets all flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/usrn", FloodReportEndpoints.GetFloodReportsByUsrn)
            .WithSummary("Flood report - All")
            .WithDescription("Gets all flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/within-metres", FloodReportEndpoints.GetFloodReportsWithinMetres)
            .WithSummary("Flood report - All")
            .WithDescription("Gets all flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", FloodReportEndpoints.GetFloodReportById)
            .WithSummary("Flood report - Get")
            .WithDescription("Gets a flood report by id from the database")
            .RequireAuthorization(PolicyNames.Reader);

        group
            .MapGet(pattern: "/reference/{reference}", FloodReportEndpoints.GetFloodReportByReference)
            .WithSummary("Flood report - Get by reference")
            .WithDescription("Gets a flood report by reference. Use searchType=Report to match the flood report reference, or searchType=Source to match the originating source reference.")
            .RequireAuthorization(PolicyNames.Reader);

        // Create - not currently available as done via messages instead

        // Update
        group
            .MapPut(pattern: "/{id}", FloodReportEndpoints.UpdateFloodReport)
            .WithSummary("Flood report - Update")
            .WithDescription("Updates a flood report by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", FloodReportEndpoints.DeleteFloodReport)
            .WithSummary("Flood report - Delete")
            .WithDescription("Deletes a flood report by id from the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{id}/exists", FloodReportEndpoints.FloodReportExists)
            .WithSummary("Flood report - Exists")
            .WithDescription("Checks if a flood report exists in the database")
            .RequireAuthorization(PolicyNames.Reader);

        group.
            MapGet(pattern: "/reference/{reference}/exists", FloodReportEndpoints.FloodReportReferenceExists)
            .WithSummary("Flood report - Reference exists")
            .WithDescription("Checks if a flood report reference exists. Use searchType=Report to match the flood report reference, or searchType=Source to match the originating source reference.")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodReportDuplicateEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/report/duplicates")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Duplicate Records");

        // Get duplicates for a specific flood report
        group
            .MapGet(pattern: "/{floodReportId}", FloodReportSourceDuplicateEndpoints.GetFloodReportSources)
            .WithSummary("Flood report duplicates - Get")
            .WithDescription("Gets duplicate records for flood report from the database")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodReportSourceEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/report/{floodReportId}/sources")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Report - Sources");

        // Get all
        group
            .MapGet(pattern: "/", FloodReportSourceEndpoints.GetFloodReportSources)
            .WithSummary("Flood sources for all flood reports")
            .WithDescription("Gets all flood sources for a flood report")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{sourceReference}", FloodReportSourceEndpoints.GetFloodReportSourceByReference)
            .WithSummary("Flood source for a flood report")
            .WithDescription("Gets a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{sourceId:guid}", FloodReportSourceEndpoints.GetFloodReportSource)
            .WithSummary("Flood source for a flood report")
            .WithDescription("Gets a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{sourceId}", FloodReportSourceEndpoints.CreateReportSource)
            .WithSummary("Create a flood source for a flood report")
            .WithDescription("Creates a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Writer);
    }

    private static void MapSearchFloodReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/search/floodreports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Search - Flood Reports");


        // Search all flood reports
        group
            .MapGet(pattern: "/", SearchFloodReportEndpoints.GetFloodReports)
            .WithSummary("Search - Flood Reports")
            .WithDescription("Searches for flood reports")
            .RequireAuthorization(PolicyNames.Reader);

        // Search flood reports within metres of coordinates
        group
            .MapGet(pattern: "/within-metres", SearchFloodReportEndpoints.GetFloodReportsWithinMetres)
            .WithSummary("Search - Flood Reports within Metres of Coordinates")
            .WithDescription("Searches for flood reports within a specified distance of the provided coordinates")
            .RequireAuthorization(PolicyNames.Reader);

        // Search flood reports by UPRN
        group
            .MapGet(pattern: "/uprn", SearchFloodReportEndpoints.GetFloodReportsByUprn)
            .WithSummary("Search - Flood reports by UPRN")
            .WithDescription("Searches for flood reports based on the provided UPRN")
            .RequireAuthorization(PolicyNames.Reader);

        // Search flood reports by USRN
        group
            .MapGet(pattern: "/usrn", SearchFloodReportEndpoints.GetFloodReportsByUsrn)
            .WithSummary("Search - Flood reports by USRN")
            .WithDescription("Searches for flood reports based on the date the provided USRN")
            .RequireAuthorization(PolicyNames.Reader);

        // General search for flood reports
        group
            .MapPost(pattern: "/search", SearchFloodReportEndpoints.SearchFloodReports)
            .WithSummary("Search - Flood reports by criteria")
            .WithDescription("Searches for flood reports using a combination of optional criteria including date range, flood sources, UPRN, USRN, and geometry. At least one criterion must be provided.")
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

        group
            .MapGet(pattern: "/consumer/property", TestEndpoints.TestFloodReportMessage)
            .WithSummary("Test the consumer flood report message")
            .WithDescription("Test the consumer flood report message functionality in the flood report service")
            .RequireAuthorization(PolicyNames.Admin);
    }
}

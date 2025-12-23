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
        MapFloodLocationFloodReportEndpoints(app);

        MapFloodPropertyEndpoints(app);
        MapFloodPropertyFloodReportEndpoints(app);

        MapFloodRoadEndpoints(app);
        MapFloodRoadFloodReportEndpoints(app);

        MapFloodReportEndpoints(app);
        MapFloodReportSourceEndpoints(app);

        MapSearchFloodLocationEndpoints(app);
        MapSearchFloodPropertyEndpoints(app);
        MapSearchFloodRoadEndpoints(app);
        MapSearchFloodReportEndpoints(app);

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
            .MapGet(pattern: "/by-point", FloodLocationEndpoints.GetFloodLocationsByPoint)
            .WithSummary("Flood location - All by point")
            .WithDescription("Gets all flood locations within a buffer of the provided easting and northing")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/with-floodreports/", FloodLocationEndpoints.GetFloodLocationsWithReports)
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
            .MapGet(pattern: "/{id}/exists", FloodLocationEndpoints.FloodLocationExists)
            .WithSummary("Flood location - Exists")
            .WithDescription("Checks if a flood location exists in the database")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodLocationFloodReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/location/{floodLocationId}/floodreports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Location - Flood Reports");

        // Get all
        group
            .MapGet(pattern: "/", FloodLocationFloodReportEndpoints.GetFloodReports)
            .WithSummary("Flood reports for a flood location")
            .WithDescription("Gets all flood reports for a flood location")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{floodReportId}", FloodLocationFloodReportEndpoints.GetFloodReport)
            .WithSummary("Flood report for a flood location")
            .WithDescription("Gets a flood report for a flood location")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{floodReportId}", FloodLocationFloodReportEndpoints.CreateFloodReport)
            .WithSummary("Create a flood report for a flood location")
            .WithDescription("Creates an unconfirmed flood report for a flood location")
            .RequireAuthorization(PolicyNames.Writer);
        group
            .MapPost(pattern: "/{floodReportId}/confirmed", FloodLocationFloodReportEndpoints.CreateFloodReportConfirmed)
            .WithSummary("Create a flood report for a flood location")
            .WithDescription("Creates a confirmed flood report for a flood location")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{floodReportId}/confirmed", FloodLocationFloodReportEndpoints.ConfirmFloodReport)
            .WithSummary("Confirms a flood report for a flood location")
            .WithDescription("Confirms a flood report is correctly related to a flood location")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{floodReportId}", FloodLocationFloodReportEndpoints.DeleteFloodReport)
            .WithSummary("Deletes a flood report for a flood location")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{floodReportId}/exists", FloodLocationFloodReportEndpoints.FloodReportExists)
            .WithSummary("Checks if a flood report exists for a flood location")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodPropertyEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/property")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Property");

        // Get all
        group
            .MapGet(pattern: "/", FloodPropertyEndpoints.GetFloodProperties)
            .WithSummary("Flood properties - All")
            .WithDescription("Gets all flood properties from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/uprn/{uprn:long}", FloodPropertyEndpoints.GetFloodPropertiesByUprn)
            .WithSummary("Flood properties - All by UPRN")
            .WithDescription("Gets all flood properties for the provided UPRN from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/with-floodreports/", FloodPropertyEndpoints.GetFloodPropertiesWithReports)
            .WithSummary("Flood properties - All with flood reports")
            .WithDescription("Gets all flood properties and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", FloodPropertyEndpoints.GetFloodProperty)
            .WithSummary("Flood property - Get")
            .WithDescription("Gets a flood property by id from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/{id}/with-reports/", FloodPropertyEndpoints.GetFloodPropertyWithReports)
            .WithSummary("Flood property - Get with flood reports")
            .WithDescription("Gets a flood property by id and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", FloodPropertyEndpoints.CreateFloodProperty)
            .WithSummary("Flood property - Create")
            .WithDescription("Creates a new flood property in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", FloodPropertyEndpoints.UpdateFloodProperty)
            .WithSummary("Flood property - Update")
            .WithDescription("Updates a flood property by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", FloodPropertyEndpoints.DeleteFloodProperty)
            .WithSummary("Flood property - Delete")
            .WithDescription("Deletes a flood property by id from the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{id}/exists", FloodPropertyEndpoints.FloodPropertyExists)
            .WithSummary("Flood property - Exists")
            .WithDescription("Checks if a flood property exists in the database")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodPropertyFloodReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/property/{floodPropertyId}/floodreports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Property - Flood Reports");

        // Get all
        group
            .MapGet(pattern: "/", FloodPropertyFloodReportEndpoints.GetFloodReports)
            .WithSummary("Flood reports for a flood property")
            .WithDescription("Gets all flood reports for a flood property")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{floodReportId}", FloodPropertyFloodReportEndpoints.GetFloodReport)
            .WithSummary("Flood report for a flood property")
            .WithDescription("Gets a flood report for a flood property")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{floodReportId}", FloodPropertyFloodReportEndpoints.CreateFloodReport)
            .WithSummary("Create a flood report for a flood property")
            .WithDescription("Creates an unconfirmed flood report for a flood property")
            .RequireAuthorization(PolicyNames.Writer);
        group
            .MapPost(pattern: "/{floodReportId}/confirmed", FloodPropertyFloodReportEndpoints.CreateFloodReportConfirmed)
            .WithSummary("Create a flood report for a flood property")
            .WithDescription("Creates a confirmed flood report for a flood property")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{floodReportId}/confirmed", FloodPropertyFloodReportEndpoints.ConfirmFloodReport)
            .WithSummary("Confirms a flood report for a flood property")
            .WithDescription("Confirms a flood report is correctly related to a flood property")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{floodReportId}", FloodPropertyFloodReportEndpoints.DeleteFloodReport)
            .WithSummary("Deletes a flood report for a flood property")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{floodReportId}/exists", FloodPropertyFloodReportEndpoints.FloodReportExists)
            .WithSummary("Checks if a flood report exists for a flood property")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodRoadEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/road")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Road");

        // Get all
        group
            .MapGet(pattern: "/", FloodRoadEndpoints.GetFloodRoads)
            .WithSummary("Flood road - All")
            .WithDescription("Gets all flood roads from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/by-usrn/{usrn:long}", FloodRoadEndpoints.GetFloodRoadsByUsrn)
            .WithSummary("Flood road - All by USRN")
            .WithDescription("Gets all flood roads for the provided USRN from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/with-floodreports/", FloodRoadEndpoints.GetFloodRoadsWithReports)
            .WithSummary("Flood road - All with flood reports")
            .WithDescription("Gets all flood roads and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{id}", FloodRoadEndpoints.GetFloodRoad)
            .WithSummary("Flood road - Get")
            .WithDescription("Gets a flood road by id from the database")
            .RequireAuthorization(PolicyNames.Reader);
        group
            .MapGet(pattern: "/{id}/with-reports/", FloodRoadEndpoints.GetFloodRoadWithReports)
            .WithSummary("Flood road - Get with flood reports")
            .WithDescription("Gets a flood road by id and related flood reports from the database")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/", FloodRoadEndpoints.CreateFloodRoad)
            .WithSummary("Flood road - Create")
            .WithDescription("Creates a new flood road in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{id}", FloodRoadEndpoints.UpdateFloodRoad)
            .WithSummary("Flood road - Update")
            .WithDescription("Updates a flood road by id in the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{id}", FloodRoadEndpoints.DeleteFloodRoad)
            .WithSummary("Flood road - Delete")
            .WithDescription("Deletes a flood road by id from the database")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{id}/exists", FloodRoadEndpoints.FloodRoadExists)
            .WithSummary("Flood road - Exists")
            .WithDescription("Checks if a flood road exists in the database")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapFloodRoadFloodReportEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/flood/road/{floodRoadId}/floodreports")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Flood Road - Flood Reports");

        // Get all
        group
            .MapGet(pattern: "/", FloodRoadFloodReportEndpoints.GetFloodReports)
            .WithSummary("Flood reports for a flood road")
            .WithDescription("Gets all flood reports for a flood road")
            .RequireAuthorization(PolicyNames.Reader);

        // Get one
        group
            .MapGet(pattern: "/{floodReportId}", FloodRoadFloodReportEndpoints.GetFloodReport)
            .WithSummary("Flood report for a flood road")
            .WithDescription("Gets a flood report for a flood road")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{floodReportId}", FloodRoadFloodReportEndpoints.CreateFloodReport)
            .WithSummary("Create a flood report for a flood road")
            .WithDescription("Creates an unconfirmed flood report for a flood road")
            .RequireAuthorization(PolicyNames.Writer);
        group
            .MapPost(pattern: "/{floodReportId}/confirmed", FloodRoadFloodReportEndpoints.CreateFloodReportConfirmed)
            .WithSummary("Create a flood report for a flood road")
            .WithDescription("Creates a confirmed flood report for a flood road")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{floodReportId}/confirmed", FloodRoadFloodReportEndpoints.ConfirmFloodReport)
            .WithSummary("Confirms a flood report for a flood road")
            .WithDescription("Confirms a flood report is correctly related to a flood road")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{floodReportId}", FloodRoadFloodReportEndpoints.DeleteFloodReport)
            .WithSummary("Deletes a flood report for a flood road")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{floodReportId}/exists", FloodRoadFloodReportEndpoints.FloodReportExists)
            .WithSummary("Checks if a flood report exists for a flood road")
            .RequireAuthorization(PolicyNames.Reader);
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

        // Create
        group
            .MapPost(pattern: "/", FloodReportEndpoints.CreateFloodReport)
            .WithSummary("Flood report - Create")
            .WithDescription("Creates a new flood report in the database")
            .RequireAuthorization(PolicyNames.Writer);

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
            .MapGet(pattern: "/{sourceId}", FloodReportSourceEndpoints.GetFloodReportSource)
            .WithSummary("Flood source for a flood report")
            .WithDescription("Gets a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Reader);

        // Create
        group
            .MapPost(pattern: "/{sourceId}", FloodReportSourceEndpoints.CreateReportSource)
            .WithSummary("Create a flood source for a flood report")
            .WithDescription("Creates a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Writer);

        // Update
        group
            .MapPut(pattern: "/{sourceId}", FloodReportSourceEndpoints.UpdateReportSource)
            .WithSummary("Updates a flood source for a flood report")
            .WithDescription("Updates a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Writer);

        // Delete
        group
            .MapDelete(pattern: "/{sourceId}", FloodReportSourceEndpoints.DeleteReportSource)
            .WithSummary("Deletes a flood source for a flood report")
            .RequireAuthorization(PolicyNames.Writer);

        // Exists
        group
            .MapGet(pattern: "/{sourceId}/exists", FloodReportSourceEndpoints.FloodReportSourceExists)
            .WithSummary("Checks if a flood report exists for a flood location")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapSearchFloodLocationEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/search/floodlocations")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Search - Flood Locations");

        // Get all
        group
            .MapGet(pattern: "/", SearchFloodLocationEndpoints.GetFloodLocations)
            .WithSummary("Search - Flood Locations")
            .WithDescription("Searches for flood locations")
            .RequireAuthorization(PolicyNames.Reader);

        group
            .MapGet(pattern: "/with-floodreports", SearchFloodLocationEndpoints.GetFloodLocationsWithReports)
            .WithSummary("Search - Flood Locations")
            .WithDescription("Searches for flood locations")
            .RequireAuthorization(PolicyNames.Reader);

        group
            .MapGet(pattern: "/by-point", SearchFloodLocationEndpoints.GetFloodLocationsByPoint)
            .WithSummary("Search - Flood Locations within Metres of Coordinates")
            .WithDescription("Searches for flood locations within a buffer of the provided easting and northing")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapSearchFloodPropertyEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/search/floodproperties")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Search - Flood Properties");

        // Get all
        group
            .MapGet(pattern: "/", SearchFloodPropertyEndpoints.GetFloodProperties)
            .WithSummary("Search - Flood Properties")
            .WithDescription("Searches for flood properties")
            .RequireAuthorization(PolicyNames.Reader);

        group
            .MapGet(pattern: "/with-floodreports", SearchFloodPropertyEndpoints.GetFloodPropertiesWithReports)
            .WithSummary("Search - Flood Properties with Reports")
            .WithDescription("Searches for flood properties with reports")
            .RequireAuthorization(PolicyNames.Reader);
    }

    private static void MapSearchFloodRoadEndpoints(WebApplication app)
    {
        var group = app
            .NewVersionedApi()
            .MapGroup("/api/search/floodroads")
            .HasApiVersion(Versions.V1.Version)
            .WithTags("Search - Flood Roads");

        // Get all
        group
            .MapGet(pattern: "/", SearchFloodRoadEndpoints.GetFloodRoads)
            .WithSummary("Search - Flood Roads")
            .WithDescription("Searches for flood roads")
            .RequireAuthorization(PolicyNames.Reader);

        group
            .MapGet(pattern: "/with-floodreports", SearchFloodRoadEndpoints.GetFloodRoadsWithReports)
            .WithSummary("Search - Flood Roads with Reports")
            .WithDescription("Searches for flood roads with reports")
            .RequireAuthorization(PolicyNames.Reader);
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
            .MapGet(pattern: "/consumer/property", TestEndpoints.TestFloodPropertyMessage)
            .WithSummary("Test the consumer flood property message")
            .WithDescription("Test the consumer flood property message functionality in the flood property service")
            .RequireAuthorization(PolicyNames.Admin);
    }
}

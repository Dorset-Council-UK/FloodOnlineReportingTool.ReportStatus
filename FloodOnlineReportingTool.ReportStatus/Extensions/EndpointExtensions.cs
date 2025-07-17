using FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

namespace Microsoft.AspNetCore.Builder;

internal static class EndpointExtensions
{
    internal static WebApplication MapReportStatusEndpoints(this WebApplication app)
    {
        var builder = app.NewVersionedApi();

        var allVersionsGroup = builder
            .MapGroup("/api/reports")
            .HasApiVersion(1, 0)
            .HasApiVersion(2, 0);

        allVersionsGroup
            .MapGet(pattern: "/union/{id}", GetAddressByIdUnionTypes)
            .WithSummary("Get address by id using union types")
            .WithDescription("Get address by id from the database");

        return app;
    }
}

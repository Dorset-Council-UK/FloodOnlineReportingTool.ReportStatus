using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

/// <summary>
/// Ensure that geometry points are represented correctly in the OpenAPI schema.
/// </summary>
internal class GeometryPointSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type == typeof(NetTopologySuite.Geometries.Point))
        {
            schema.Type = "object";
            schema.Properties.Clear();
            schema.Properties["type"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("Point"),
            };
            schema.Properties["coordinates"] = new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema {
                    Type = "number",
                    Format = "double",
                },
                MinItems = 2,
                MaxItems = 2,
            };
            schema.Required = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "type", "coordinates" };
        }

        return Task.CompletedTask;
    }
}

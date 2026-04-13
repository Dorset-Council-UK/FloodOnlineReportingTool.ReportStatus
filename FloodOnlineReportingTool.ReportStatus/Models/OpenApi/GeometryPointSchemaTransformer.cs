using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

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
            schema.Type = JsonSchemaType.Object;
            schema.Properties ??= new Dictionary<string, IOpenApiSchema>(StringComparer.OrdinalIgnoreCase);
            schema.Properties.Add("type", new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Example = JsonValue.Create("Point"),
            });
            schema.Properties.Add("coordinates", new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Items = new OpenApiSchema
                {
                    Type = JsonSchemaType.Number,
                    Format = "double",
                },
                MinItems = 2,
                MaxItems = 2,
            });
            schema.Required = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "type", "coordinates" };
        }

        return Task.CompletedTask;
    }
}

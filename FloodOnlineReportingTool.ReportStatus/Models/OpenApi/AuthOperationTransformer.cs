using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

internal sealed class AuthOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var badRequest = StatusCodes.Status400BadRequest.ToString();
        if (!operation.Responses.ContainsKey(badRequest))
        {
            operation.Responses.Add(badRequest, new() { Description = "Bad Request" });
        }

        var unauthorized = StatusCodes.Status401Unauthorized.ToString();
        if (!operation.Responses.ContainsKey(unauthorized))
        {
            operation.Responses.Add(unauthorized, new() { Description = "Unauthorized" });
        }

        return Task.CompletedTask;
    }
}

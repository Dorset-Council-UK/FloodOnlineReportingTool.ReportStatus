using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace FloodOnlineReportingTool.ReportStatus.Models.OpenApi;

internal sealed class DocumentTransformer(ApiVersion version) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new()
        {
            Version = version.ToString("VVVV", ApiVersionFormatProvider.CurrentCulture),
            Title = "Flood Online Reporting Tool - Report Status",
            Description = "Administration and status reporting for the flood online reporting tool. Built for professional flood risk colleagues, tracking reports from both the public and lead authorities.",
            TermsOfService = new("https://github.com/Dorset-Council-UK/FloodOnlineReportingTool.ReportStatus"),
            Contact = new()
            {
                Name = "Contact",
                Url = new("https://github.com/Dorset-Council-UK/FloodOnlineReportingTool.ReportStatus"),
            },
            License = new()
            {
                Name = "MIT",
                Url = new("https://opensource.org/license/mit"),
            },
        };

        return Task.CompletedTask;
    }
}
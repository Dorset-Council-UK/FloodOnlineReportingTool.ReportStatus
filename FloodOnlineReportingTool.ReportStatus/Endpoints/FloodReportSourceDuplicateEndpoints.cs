using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodReportSourceDuplicateEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReportSourceDuplicate>>, NotFound>> GetFloodReportSources(IFloodReportService floodReportService, Guid floodReportId, CancellationToken ct)
    {
        var floodReportSourceDuplicates = await floodReportService.GetPotentialDuplicatesById(floodReportId, ct).ConfigureAwait(false);
        return floodReportSourceDuplicates is null ? TypedResults.NotFound() : TypedResults.Ok(floodReportSourceDuplicates);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

internal static class FloodReportEndpoints
{
    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetFloodReports(IFloodReportService floodReportService, CancellationToken ct)
    {
        var floodReports = await floodReportService.GetFloodReports(ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReports);
    }

    internal static async Task<Results<Ok<FloodReport>, NotFound>> GetFloodReportById(IFloodReportService floodReportService, Guid id, CancellationToken ct)
    {
        var floodReport = await floodReportService.GetFloodReportById(id, ct).ConfigureAwait(false);
        return floodReport == null ? TypedResults.NotFound() : TypedResults.Ok(floodReport);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetFloodReportsByUprn(IFloodReportService floodReportService, long uprn, CancellationToken ct)
    {
        var floodReports = await floodReportService.GetFloodReportsByUprn(uprn, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReports);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, BadRequest>> GetFloodReportsByUsrn(IFloodReportService floodReportService, long usrn, CancellationToken ct)
    {
        var floodReports = await floodReportService.GetFloodReportsByUsrn(usrn, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReports);
    }

    internal static async Task<Results<Ok<IReadOnlyCollection<FloodReport>>, ValidationProblem>> GetFloodReportsWithinMetres(IFloodReportService floodReportService, double easting, double northing, double metres, CancellationToken ct)
    {
        if (metres is < 0 or > IFloodReportService.MaxMetres)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(metres), [string.Create(CultureInfo.InvariantCulture, $"Metres must be between 0 and {IFloodReportService.MaxMetres} metres.")] },
            });
        }

        var floodReports = await floodReportService.GetFloodReportsWithinMetres(easting, northing, metres, ct).ConfigureAwait(false);
        return TypedResults.Ok(floodReports);
    }

    internal static async Task<Results<Created<FloodReport>, BadRequest>> CreateFloodReport(IFloodReportService floodReportService, FloodReportDto dto, CancellationToken ct)
    {
        var floodReport = await floodReportService.CreateFloodReport(dto, ct).ConfigureAwait(false);
        return TypedResults.Created($"/api/flood/reports/{floodReport.Id}", floodReport);
    }

    internal static async Task<Results<Ok<FloodReport>, NotFound, BadRequest>> UpdateFloodReport(IFloodReportService floodReportService, Guid id, FloodReportDto dto, CancellationToken ct)
    {
        var floodReport = await floodReportService.UpdateFloodReport(id, dto, ct).ConfigureAwait(false);
        return floodReport == null ? TypedResults.NotFound() : TypedResults.Ok(floodReport);
    }

    internal static async Task<Results<NoContent, NotFound, BadRequest>> DeleteFloodReport(IFloodReportService floodReportService, Guid id, CancellationToken ct)
    {
        var deleted = await floodReportService.DeleteFloodReport(id, ct).ConfigureAwait(false);
        return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportExists(IFloodReportService floodReportService, Guid id, CancellationToken ct)
    {
        var exists = await floodReportService.FloodReportExists(id, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }
}

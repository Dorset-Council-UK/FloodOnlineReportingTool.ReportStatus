using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.Database.Models;
using FloodOnlineReportingTool.Database.Services;
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

    internal static async Task<Results<Ok<bool>, BadRequest>> FloodReportExists(IFloodReportService floodReportService, Guid id, CancellationToken ct)
    {
        var exists = await floodReportService.FloodReportExists(id, ct).ConfigureAwait(false);
        return TypedResults.Ok(exists);
    }

    internal static async Task<Results<Ok<bool>, BadRequest, ValidationProblem>> FloodReportReferenceExists(
        IFloodReportService floodReportService,
        string reference,
        FloodReferenceSearchType searchType,
        CancellationToken ct)
    {
        if (!Enum.IsDefined(searchType))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(searchType), [$"Invalid search type '{searchType}'. Valid values are: {string.Join(", ", Enum.GetNames<FloodReferenceSearchType>())}."] },
            });
        }

        var exists = searchType switch
        {
            FloodReferenceSearchType.Source => await floodReportService.ReportWithSourceReferenceExists(reference, ct).ConfigureAwait(false),
            _ => await floodReportService.ReportWithReferenceExists(reference, ct).ConfigureAwait(false),
        };

        return TypedResults.Ok(exists);
    }

    internal static async Task<Results<Ok<FloodReportWithSourceReferences>, NotFound, ValidationProblem>> GetFloodReportByReference(
        IFloodReportService floodReportService,
        string reference,
        FloodReferenceSearchType searchType,
        CancellationToken ct)
    {
        if (!Enum.IsDefined(searchType))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                { nameof(searchType), [$"Invalid search type '{searchType}'. Valid values are: {string.Join(", ", Enum.GetNames<FloodReferenceSearchType>())}."] },
            });
        }

        var floodReport = searchType switch
        {
            FloodReferenceSearchType.Source => await floodReportService.GetFloodReportBySourceReference(reference, ct).ConfigureAwait(false),
            _ => await floodReportService.GetFloodReportByReference(reference, ct).ConfigureAwait(false),
        };

        return floodReport == null ? TypedResults.NotFound() : TypedResults.Ok(floodReport.ToWithSourceReferences());
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
}

using Microsoft.AspNetCore.Http.HttpResults;

namespace Microsoft.AspNetCore.Builder;

internal static class ReportStatusEndpoints
{
    /// <summary>
    /// Get all addresses
    /// </summary>
    private static async Task<IResult> GetReports(ReportService reportService, CancellationToken ct)
    {
        var addresses = await addressService
            .GetAddresses(ct)
            .ConfigureAwait(false);

        return Results.Ok(addresses);
    }

    /// <summary>
    /// Get address by id
    /// </summary>
    private static async Task<IResult> GetReportById(ReportService reportService, Guid id, CancellationToken ct)
    {
        var address = await addressService
            .GetAddressById(id, ct)
            .ConfigureAwait(false);

        return address is null
            ? Results.NotFound()
            : Results.Ok(address);
    }

    private static async Task<Results<Ok<AddressV2Dto>, NotFound, BadRequest>> GetAddressByIdUnionTypes(ReportService reportService, Guid id, CancellationToken ct)
    {
        var address = await addressService
            .GetAddressById(id, ct)
            .ConfigureAwait(false);

        return address is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(address);
    }

    /// <summary>
    /// Create address
    /// </summary>
    private static async Task<IResult> CreateReport(ReportService reportService, ReportDto dto, CancellationToken ct)
    {
        var address = await addressService
            .CreateAddress(dto, ct)
            .ConfigureAwait(false);

        return Results.Created($"/api/address/{address.Id}", address);
    }

    /// <summary>
    /// Update address
    /// </summary>
    private static async Task<IResult> UpdateReport(ReportService reportService, Guid id, ReportDto dto, CancellationToken ct)
    {
        var address = await addressService
            .UpdateAddress(id, dto, ct)
            .ConfigureAwait(false);

        return address is null
            ? Results.NotFound()
            : Results.Ok(address);
    }

    /// <summary>
    /// Delete address
    /// </summary>
    private static async Task<IResult> DeleteReport(ReportService reportService, Guid id, CancellationToken ct)
    {
        var deleted = await addressService
            .DeleteAddress(id, ct)
            .ConfigureAwait(false);

        return deleted
            ? Results.NoContent()
            : Results.NotFound();
    }
}

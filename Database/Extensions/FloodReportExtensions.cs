using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class FloodReportExtensions
{
    public static FloodReportDto ToDto(this FloodReport floodReport)
    {
        return new FloodReportDto
        {
            Id = floodReport.Id,
            Reference = floodReport.Reference,
            RecordStatus = floodReport.RecordStatus,
            SourceRecords = floodReport.SourceRecords
        };
    }

    public static FloodReportCreated ToMessageCreated(this FloodReport floodReport)
    {
        return new FloodReportCreated(
            floodReport.Reference,
            floodReport.Geom.X,
            floodReport.Geom.Y,
            floodReport.SourceRecords.Any(sr => sr.HasContacts),
            floodReport.SourceRecords.SelectMany(sr => sr.ContactRecordTypes).Distinct().ToList()
        );
    }

    public static FloodReportUpdated ToMessageUpdated(this FloodReport floodReport)
    {
        return new FloodReportUpdated(
            floodReport.Reference,
            floodReport.RecordStatus,
            floodReport.Geom.X,
            floodReport.Geom.Y,
            floodReport.SourceRecords.Any(sr => sr.HasInvestigation),
            floodReport.SourceRecords.Any(sr => sr.HasContacts),
            floodReport.SourceRecords.SelectMany(sr => sr.ContactRecordTypes).Distinct().ToList(),
            null
        );
    }

    public static SearchResultFloodReport ToSearchResult(this FloodReport floodReport)
    {
        return new(
            floodReport.Id,
            floodReport.Reference,
            floodReport.RecordStatus,
            floodReport.SourceRecords.ToSearchResults()
        );
    }

    public static IQueryable<SearchResultFloodReport> ToSearchResult(this IQueryable<FloodReport> floodReportQuery)
    {
        return floodReportQuery.Select(fr => new SearchResultFloodReport(
            fr.Id,
            fr.Reference,
            fr.RecordStatus,
            fr.SourceRecords.Select(sr => new SearchResultFloodReportSource(
                sr.Id,
                sr.Reference,
                sr.ViewUri,
                sr.CreatedUtc,
                sr.UpdatedUtc,
                sr.Geom,
                sr.Easting,
                sr.Northing,
                sr.Latitude,
                sr.Longitude,
                sr.ImpactStartUtc,
                sr.FloodCauses.Select(fs => new SearchResultFloodReportCause(
                    fs.CauseId,
                    fs.Name
                )).ToList(),
                sr.Organisations.Select(o => new SearchResultFloodReportOrganisation(
                    o.OrganisationId,
                    o.OrganisationName,
                    o.FloodAuthorityId,
                    o.FloodAuthorityName
                )).ToList()
            )).ToList()
        ));
    }

    /// <summary>
    /// Converts a collection of <see cref="FloodReport"/> to <see cref="IReadOnlyCollection{SearchResultFloodReport}"/>.
    /// </summary>
    public static IReadOnlyCollection<SearchResultFloodReport> ToSearchResults(this ICollection<FloodReport> floodReports)
    {
        return [.. floodReports.Select(fr => fr.ToSearchResult())];
    }
    public static FloodReportWithSourceReferences ToWithSourceReferences(this FloodReport floodReport) =>
        new()
        {
            Id = floodReport.Id,
            Reference = floodReport.Reference,
            RecordStatus = floodReport.RecordStatus,
            RecordType = floodReport.RecordType,
            CreatedUtc = floodReport.CreatedUtc,
            UpdatedUtc = floodReport.UpdatedUtc,
            Easting = floodReport.Easting,
            Northing = floodReport.Northing,
            Latitude = floodReport.Latitude,
            Longitude = floodReport.Longitude,
            SourceReferences = floodReport.SourceRecords.Select(s => s.Reference).ToList(),
        };
}

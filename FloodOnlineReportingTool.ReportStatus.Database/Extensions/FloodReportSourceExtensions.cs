using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared.Search;

#pragma warning disable IDE0130
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130

public static class FloodReportSourceExtensions
{
    public static SearchResultFloodReportSource ToSearchResult(this FloodReportSource source)
    {
        return new SearchResultFloodReportSource(
            source.Id,
            source.Reference,
            source.ViewUri,
            source.CreatedUtc,
            source.UpdatedUtc,
            source.Geom,
            source.Easting,
            source.Northing,
            source.Latitude,
            source.Longitude,
            source.ImpactStartUtc,
            source.FloodCauses.ToSearchResults(),
            source.Organisations.ToSearchResults()
        );
    }

    public static IReadOnlyCollection<SearchResultFloodReportSource> ToSearchResults(this ICollection<FloodReportSource> sources)
    {
        return [.. sources.Select(s => s.ToSearchResult())];
    }

    // Fixes errors 1 & 2 - ToSearchResults for the new wrapper types
    public static IReadOnlyCollection<SearchResultFloodReportCause> ToSearchResults(this ICollection<FloodReportSourceFloodCause> floodCauses)
    {
        return [.. floodCauses.Select(fc => new SearchResultFloodReportCause(fc.CauseId, fc.Name))];
    }

    public static IReadOnlyCollection<SearchResultFloodReportOrganisation> ToSearchResults(this ICollection<FloodReportSourceOrganisation> organisations)
    {
        return [.. organisations.Select(org => new SearchResultFloodReportOrganisation(org.OrganisationId, org.OrganisationName, org.FloodAuthorityId, org.FloodAuthorityName))];
    }

    // Kept for use where contract types are still in scope (e.g. EligibilityCheckRecord)
    public static SearchResultFloodReportOrganisation ToSearchResult(this EligibilityCheckOrganisation organisation)
    {
        return new SearchResultFloodReportOrganisation(organisation.Id, organisation.Name, organisation.FloodAuthorityId, organisation.FloodAuthorityName);
    }

    public static IReadOnlyCollection<SearchResultFloodReportOrganisation> ToSearchResults(this IReadOnlyCollection<EligibilityCheckOrganisation> organisations)
    {
        return [.. organisations.Select(o => o.ToSearchResult())];
    }

    public static SearchResultFloodReportCause ToSearchResult(this EligibilityCheckFloodSource floodSource)
    {
        return new SearchResultFloodReportCause(floodSource.Id, floodSource.Name);
    }

    public static IReadOnlyCollection<SearchResultFloodReportCause> ToSearchResults(this IReadOnlyCollection<EligibilityCheckFloodSource> floodSources)
    {
        return [.. floodSources.Select(fs => fs.ToSearchResult())];
    }

    public static FloodReportSourceFloodCause ToFloodCause(this EligibilityCheckFloodSource source, Guid floodReportSourceId)
    {
        return new FloodReportSourceFloodCause
        {
            FloodReportSourceId = floodReportSourceId,
            CauseId = source.Id,
            Name = source.Name,
        };
    }

    public static FloodReportSourceOrganisation ToOrganisation(this EligibilityCheckOrganisation source, Guid floodReportSourceId)
    {
        return new FloodReportSourceOrganisation
        {
            FloodReportSourceId = floodReportSourceId,
            OrganisationId = source.Id,
            OrganisationName = source.Name,
            FloodAuthorityId = source.FloodAuthorityId,
            FloodAuthorityName = source.FloodAuthorityName
        };
    }

    public static (ICollection<FloodReportSourceFloodCause> FloodCauses, ICollection<FloodReportSourceOrganisation> Organisations) ToSourceCollections(
        this FloodReportSourceCreated message,
        Guid floodReportSourceId)
    {
        var floodCauses = message.EligibilityCheckRecord.FloodSources
            .Select(fs => fs.ToFloodCause(floodReportSourceId))
            .ToList();

        var organisations = message.EligibilityCheckRecord.Organisations
            .Select(org => org.ToOrganisation(floodReportSourceId))
            .ToList();

        return (floodCauses, organisations);
    }
}

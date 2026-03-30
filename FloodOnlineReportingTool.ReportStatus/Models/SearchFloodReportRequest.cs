using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Models;

public record SearchFloodReportRequest
{
    public DateTimeOffset? ResultsFrom { get; init; }
    public DateTimeOffset? ResultsTo { get; init; }
    public ICollection<Guid> FloodSources { get; init; } = [];
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public string Postcode { get; init; } = string.Empty; // TODO: Not yet implemented - placeholder for future use
    public MultiPolygon? Geom { get; init; }

    internal bool HasSearchCriteria =>
        ResultsFrom.HasValue ||
        ResultsTo.HasValue ||
        FloodSources.Count > 0 ||
        Uprn.HasValue ||
        Usrn.HasValue ||
        Geom is not null;
}

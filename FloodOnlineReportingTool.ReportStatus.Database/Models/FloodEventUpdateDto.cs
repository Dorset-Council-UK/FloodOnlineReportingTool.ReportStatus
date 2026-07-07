using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodEventUpdateDto
{
    // Optional: external caller may not know internal ID
    public Guid? Id { get; init; }

    // Primary business key for update matching
    public required string Reference { get; init; }

    public required string Title { get; init; }
    public required Guid RecordStatus { get; init; }
    public required Guid FloodStatus { get; init; }
    public required Guid InvestigationStatus { get; init; }

    public IReadOnlyCollection<string>? LinkedFloodReports { get; init; }

    // Optional in messages (if missing, keep existing value)
    public MultiPolygon? Geom { get; init; }

    public required DateTimeOffset FloodStartDate { get; init; }
    public required DateTimeOffset FloodEndDate { get; init; }

    public ICollection<Guid>? FloodCauses { get; init; }
    public ICollection<Guid>? Organisations { get; init; }
    public ICollection<Guid>? StatusFilter { get; init; }
}

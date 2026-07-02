using FloodOnlineReportingTool.Contracts.Shared;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodEvent : ICreatedUpdated
{
    public required Guid Id { get; init; }
    public required string Reference { get; init; }
    public required string Title { get; set; }
    public required Guid RecordStatus { get; set; }
    public Guid FloodStatus { get; set; }
    public Guid InvestigationStatus { get; set; } = Section19StatusIds.NoSection19;

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; set; }

    public IReadOnlyCollection<Guid> LinkedFloodReports { get; set; } = [];

    // Search parameters
    public MultiPolygon? Geom { get; init; } // Optional because it can't be sent via service bus
    public DateTimeOffset FloodStartDate { get; init; } 
    public DateTimeOffset FloodEndDate { get; init; } 
    public required ICollection<Guid> FloodCauses { get; init; }  
    public required ICollection<Guid> Organisations { get; init; }
    public required ICollection<Guid> StatusFilter { get; init; }
}

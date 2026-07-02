using FloodOnlineReportingTool.Contracts.Shared;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodEvent : ICreatedUpdated
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Reference { get; init; }
    public required string Title { get; set; }
    public required Guid RecordStatus { get; set; }
    public Guid FloodStatus { get; set; }
    public Guid InvestigationStatus { get; set; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedUtc { get; set; }

    public IReadOnlyCollection<Guid> LinkedFloodReports { get; set; } = [];

    // Search parameters
    public MultiPolygon? Geom { get; set; } // Optional because it can't be sent via service bus
    public DateTimeOffset FloodStartDate { get; init; } 
    public DateTimeOffset FloodEndDate { get; init; } 
    public required ICollection<Guid> FloodCauses { get; init; }  
    public required ICollection<Guid> Organisations { get; init; }
    public required ICollection<Guid> StatusFilter { get; init; }
}

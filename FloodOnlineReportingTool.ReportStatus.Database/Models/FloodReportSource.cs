using FloodOnlineReportingTool.Contracts;
using NetTopologySuite.Geometries;

namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public record FloodReportSource : ICreatedUpdated
{
    public required Guid Id { get; init; }
    public required string Reference { get; init; }
    public required Uri ViewUri { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; init; }

    // Other properties
    //public Guid? EligibilityCheckId { get; init; }
    public long? Uprn { get; init; }
    public long? Usrn { get; init; }
    public required Point Geom { get; init; }
    public double Easting { get; init; }
    public double Northing { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
    public DateTimeOffset? ImpactStartUtc { get; init; }
    public int? ImpactDurationHours { get; init; }
    public bool IsOnGoing { get; init; }
    public bool IsUninhabitable { get; init; }
    public int? VulnerableCount { get; init; }
    public string? LocationDescription { get; init; }

    // Navigation properties using wrapper models instead of contract types directly
    public ICollection<FloodReportSourceFloodCause> FloodCauses { get; init; } = [];
    public ICollection<FloodReportSourceOrganisation> Organisations { get; init; } = [];

    public required Guid FloodReportId { get; init; }
    public FloodReport? FloodReport { get; init; }
}
namespace FloodOnlineReportingTool.Database.Models;

public record FloodEventCreateDto 
{
    public required string Reference { get; init; }
    public required string Title { get; init; }
    public required Guid RecordStatus { get; init; }
    public required Guid FloodStatus { get; init; }
    public required Guid InvestigationStatus { get; init; }

    public IReadOnlyCollection<string>? LinkedFloodReports { get; init; }

    public required DateTimeOffset FloodStartDate { get; init; }
    public required DateTimeOffset FloodEndDate { get; init; }

    public required ICollection<Guid> FloodCauses { get; init; }  
    public required ICollection<Guid> Organisations { get; init; }
    public required ICollection<Guid> StatusFilter { get; init; }
}

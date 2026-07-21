using FloodOnlineReportingTool.Contracts.Shared;
using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace FloodOnlineReportingTool.Database.Models;
public record FloodReport : ICreatedUpdated
{
    public required Guid Id { get; init; }
    public required string Reference { get; init; }
    public required Guid RecordStatus { get; init; }
    public required FloodReportType RecordType { get; init; }

    // ICreatedUpdated implementation
    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? UpdatedUtc { get; set; }

    // Source records might have a slightly different location if the UPRN/USRN has changed between reports or if the location match option has been used. This is the official location for the whole report so we have a separate Geom and coordinates here. This will be used for user range filtering. 
    public required Point Geom { get; init; }
    public double Easting { get; init; }
    public double Northing { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }

    // Other properties
    [JsonIgnore]
    public ICollection<FloodReportSource> SourceRecords { get; init; } = [];
}

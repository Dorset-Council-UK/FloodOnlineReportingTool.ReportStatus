using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.ReportStatus.Database.Services;

public interface IFloodReportService
{
    const double MaxMetres = 100;

    /// <summary>
    /// Creates a new flood report or sets existing rows to duplicate based on incoming message.
    /// </summary>
    Task CreateFromFloodReportCreated(FloodReportSourceCreated message, CancellationToken ct);

    /// <summary>
    /// Creates a new flood report or returns updated duplicate rows using the API endpoint.
    /// </summary>
    Task<ICollection<FloodReport>?> CreateFloodReport(FloodReportSource dto, double buffer, CancellationToken ct);

    /// <summary>
    /// Updates an existing flood report in the database.
    /// </summary>
    Task<FloodReport?> UpdateFloodReport(Guid id, FloodReportDto dto, CancellationToken ct);

    /// <summary>
    /// Gets all flood reports from the database.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReports(CancellationToken ct);

    /// <summary>
    /// Checks if a report with the provided reference exists in the database.
    /// </summary>
    Task<bool> ReportWithReferenceExists(string reference, CancellationToken ct);

    /// <summary>
    /// Checks if a report with the provided source reference exists in the database.
    /// </summary>
    Task<bool> ReportWithSourceReferenceExists(string reference, CancellationToken ct);

    /// <summary>
    /// Gets a flood report by its reference from the database.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<FloodReport?> GetFloodReportByReference(string reference, CancellationToken ct);

    /// <summary>
    /// Gets a flood report by its source reference from the database.
    /// </summary>
    Task<FloodReport?> GetFloodReportBySourceReference(string reference, CancellationToken ct);

    /// <summary>
    /// Gets a flood report by its ID from the database.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<FloodReport?> GetFloodReportById(Guid id, CancellationToken ct);

    /// <summary>
    /// Get flood reports by there matching UPRN's from the database.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUprn(long uprn, CancellationToken ct);

    /// <summary>
    /// Get flood reports by there matching USRN's from the database.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsByUsrn(long usrn, CancellationToken ct);

    /// <summary>
    /// Get flood reports that are within a specified buffer distance.
    /// </summary>
    /// <remarks>Flood report sources are included</remarks>
    Task<IReadOnlyCollection<FloodReport>> GetFloodReportsWithinMetres(double easting, double northing, double metres, CancellationToken ct);

    /// <summary>
    /// Deletes an existing flood report in the database.
    /// </summary>
    Task<bool> DeleteFloodReport(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if the flood report exists
    /// </summary>
    Task<bool> FloodReportExists(Guid id, CancellationToken ct);

    Task<IReadOnlyCollection<Guid>> FindPotentialDuplicates(FloodReportType ReportType, double buffer, EligibilityCheckRecord incomingRecord, CancellationToken ct);

    Task<IReadOnlyCollection<FloodReportSourceDuplicate>?> GetPotentialDuplicatesById(Guid id, CancellationToken ct);

}

using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using Tests.Data;
using Tests.Fixtures;

namespace Tests.Services;

public class FloodReportServiceTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context = fixture.Context;
    private readonly ILogger<FloodReportService> _logger = Substitute.For<ILogger<FloodReportService>>();

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _context.FloodReports.RemoveRange(_context.FloodReports);
        await _context.SaveChangesAsync(cancellationToken);
        _context.FloodReports.AddRange(expected);
        await _context.SaveChangesAsync(cancellationToken);

        var service = new FloodReportService(_logger, _context);

        // Act
        var result = await service.GetFloodReports(cancellationToken);

        // Assert
        var expectedJson = JsonSerializer.Serialize(expected);
        var resultJson = JsonSerializer.Serialize(result);
        Assert.Equal(expectedJson, resultJson);
    }
}

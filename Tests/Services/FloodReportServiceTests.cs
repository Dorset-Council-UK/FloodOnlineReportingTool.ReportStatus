using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Data;
using Tests.Fixtures;

namespace Tests.Services;

public class FloodReportServiceTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context = fixture.Context;
    private readonly ILogger<FloodReportService> _logger = Substitute.For<ILogger<FloodReportService>>();

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful_MemoryDatabase(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _context.FloodReports.RemoveRange(_context.FloodReports);
        _context.FloodReportSources.RemoveRange(_context.FloodReportSources);
        await _context.SaveChangesAsync(cancellationToken);
        await _context.FloodReports.AddRangeAsync(expected, cancellationToken);
        await _context.FloodReportSources.AddRangeAsync(expected.SelectMany(fr => fr.Sources), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var service = new FloodReportService(_logger, _context);

        // Act
        var result = await service.GetFloodReports(cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful_Interface(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var service = Substitute.For<IFloodReportService>();
        service.GetFloodReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await service.GetFloodReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful_Pure(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var fakeFloodReports = expected.AsQueryable();
        var mockSet = Substitute.For<DbSet<FloodReport>, IQueryable<FloodReport>>();
        ((IQueryable<FloodReport>)mockSet).Provider.Returns(fakeFloodReports.Provider);
        ((IQueryable<FloodReport>)mockSet).Expression.Returns(fakeFloodReports.Expression);
        ((IQueryable<FloodReport>)mockSet).ElementType.Returns(fakeFloodReports.ElementType);
        ((IQueryable<FloodReport>)mockSet).GetEnumerator().Returns(fakeFloodReports.GetEnumerator());

        var mockContext = Substitute.For<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        mockContext.FloodReports.Returns(mockSet);

        var service = new FloodReportService(_logger, mockContext);

        // Act
        var result = await service.GetFloodReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful_Pure2(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var context = Substitute.For<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        context.FloodReports.Returns(expected.AsQueryable());

        var service = new FloodReportService(_logger, context);

        // Act
        var result = await service.GetFloodReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }
}

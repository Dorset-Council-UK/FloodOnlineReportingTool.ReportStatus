using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using NSubstitute;
using Tests.Data;

namespace Tests.Services;

public class FloodReportServiceTests
{
    private readonly ApplicationDbContext _context = new(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
    private readonly ILogger<FloodReportService> _logger = Substitute.For<ILogger<FloodReportService>>();

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> collection = [.. expected]; // We need ICollection for MockQueryable.NSubstitute
        var query = collection.BuildMock();
        repository.GetFloodReports().Returns(query);

        var service = new FloodReportService(_logger, repository, _context);

        // Act
        var result = await service.GetFloodReports(cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_3), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReportById_ReturnsFloodReport_WhenSuccessful(FloodReport expected)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> collection = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var query = collection.BuildMock();
        repository.GetFloodReportById(expected.Id).Returns(query);

        var service = new FloodReportService(_logger, repository, _context);

        // Act
        var result = await service.GetFloodReportById(expected.Id, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }
}

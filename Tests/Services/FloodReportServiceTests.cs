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
    /// <summary>
    /// Creates a substitute ApplicationDbContext for unit testing purposes.
    /// </summary>
    /// <remarks>Some tests you only need to mock the repository, but FloodReportService requires a DbContext in its constructor.</remarks>
    private static ApplicationDbContext MockDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        return Substitute.For<ApplicationDbContext>(options);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var repository = Substitute.For<IFloodReportRepository>();
        var query = expected.BuildMock();
        repository.GetFloodReports().Returns(query);

        var service = new FloodReportService(logger, repository, MockDbContext());

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
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> collection = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var query = collection.BuildMock();
        repository.GetFloodReportById(expected.Id).Returns(query);

        var service = new FloodReportService(logger, repository, MockDbContext());

        // Act
        var result = await service.GetFloodReportById(expected.Id, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Repositories;

/// <summary>
/// Unit tests for the <see cref="FloodRoadRepository"/> class.
/// </summary>
public class FloodRoadRepositoryTests
{
    /// <summary>
    /// Creates a substitute ApplicationDbContext for unit testing purposes.
    /// </summary>
    private static ApplicationDbContext MockDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        return Substitute.For<ApplicationDbContext>(options);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Collection_Four), MemberType = typeof(FloodRoadData))]
    public void GetFloodRoads_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodRoad> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodRoads.Returns(mockDbSet);

        var repository = new FloodRoadRepository(mockContext);

        // Act
        var query = repository.GetFloodRoads();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Collection_Four_WithReports), MemberType = typeof(FloodRoadData))]
    public void GetFloodRoadsWithReports_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodRoad> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodRoads.Returns(mockDbSet);

        var repository = new FloodRoadRepository(mockContext);

        // Act
        var query = repository.GetFloodRoadsWithReports();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Repositories;

/// <summary>
/// Unit tests for the <see cref="FloodPropertyRepository"/> class.
/// </summary>
public class FloodPropertyRepositoryTests
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
    [MemberData(nameof(FloodPropertyData.Collection_Four), MemberType = typeof(FloodPropertyData))]
    public void GetFloodProperties_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodProperty> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodProperties.Returns(mockDbSet);

        var repository = new FloodPropertyRepository(mockContext);

        // Act
        var query = repository.GetFloodProperties();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four_WithReports), MemberType = typeof(FloodPropertyData))]
    public void GetFloodPropertiesWithReports_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodProperty> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodProperties.Returns(mockDbSet);

        var repository = new FloodPropertyRepository(mockContext);

        // Act
        var query = repository.GetFloodPropertiesWithReports();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }
}

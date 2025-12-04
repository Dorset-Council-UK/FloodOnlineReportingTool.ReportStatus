using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Repositories;

/// <summary>
/// Unit tests for the <see cref="FloodLocationRepository"/> class.
/// </summary>
/// <remarks>Because <see cref="FloodLocationRepository.GetFloodLocationsByPoint"/> uses <see cref="NetTopologySuite.Geometries.Geometry.Intersects(NetTopologySuite.Geometries.Geometry)">Geometry.Intersects()</see> and <see cref="NetTopologySuite.Geometries.Geometry.Buffer(double)">Geometry.Buffer()</see> the results are not accurate in unit tests, that must be an integration test not a unit test.</remarks>
public class FloodLocationRepositoryTests
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
    [MemberData(nameof(FloodLocationData.Collection_Four), MemberType = typeof(FloodLocationData))]
    public void GetFloodLocations_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodLocation> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodLocations.Returns(mockDbSet);

        var repository = new FloodLocationRepository(mockContext);

        // Act
        var query = repository.GetFloodLocations();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four_WithReports), MemberType = typeof(FloodLocationData))]
    public void GetFloodLocationsWithReports_ReturnsOrderedQuery_WhenSuccessful(ICollection<FloodLocation> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodLocations.Returns(mockDbSet);

        var repository = new FloodLocationRepository(mockContext);

        // Act
        var query = repository.GetFloodLocationsWithReports();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }
}

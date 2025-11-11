using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Repositories;

public class FloodReportRepositoryTests
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
    [MemberData(nameof(FloodReportData.FloodReport_Collections_4), MemberType = typeof(FloodReportData))]
    public void GetFloodReports_ReturnsQuery_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReports();
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_3), MemberType = typeof(FloodReportData))]
    public void GetFloodReportById_ReturnsQuery_WhenSuccessful(FloodReport expected)
    {
        // Arrange
        ICollection<FloodReport> collection = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = collection.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportById(expected.Id);
        var result = query.FirstOrDefault();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReport_Within50Metres_Collections), MemberType = typeof(FloodReportData))]
    public void GetFloodReportsWithinMetres_ReturnsQuery_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var metres = 50.0;
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportsWithinMetres(FloodReportData.Easting, FloodReportData.Northing, metres);
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReportWithUprn_Collections_4), MemberType = typeof(FloodReportData))]
    public void GetFloodReportsByUprn_ReturnsQuery_WhenFound(ICollection<FloodReport> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportsByUprn(FloodReportData.FixedUprn);
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReportWithUsrn_Collections_4), MemberType = typeof(FloodReportData))]
    public void GetFloodReportsByUprn_ReturnsEmptyQuery_WhenNoneFound(ICollection<FloodReport> expected)
    {
        // Note: The USRN data has the UPRN field set to null, meaning it works for this test

        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportsByUprn(FloodReportData.FixedUprn);
        var result = query.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReportWithUsrn_Collections_4), MemberType = typeof(FloodReportData))]
    public void GetFloodReportsByUsrn_ReturnsQuery_WhenFound(ICollection<FloodReport> expected)
    {
        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportsByUsrn(FloodReportData.FixedUsrn);
        var result = query.ToList();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.FloodReportWithUprn_Collections_4), MemberType = typeof(FloodReportData))]
    public void GetFloodReportsByUsrn_ReturnsEmptyQuery_WhenNoneFound(ICollection<FloodReport> expected)
    {
        // Note: The UPRN data has the USRN field set to null, meaning it works for this test

        // Arrange
        var mockDbSet = expected.BuildMockDbSet();
        var mockContext = MockDbContext();
        mockContext.FloodReports.Returns(mockDbSet);

        var repository = new FloodReportRepository(mockContext);

        // Act
        var query = repository.GetFloodReportsByUsrn(FloodReportData.FixedUsrn);
        var result = query.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

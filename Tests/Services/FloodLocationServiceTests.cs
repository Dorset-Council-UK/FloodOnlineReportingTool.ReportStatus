using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Services;

public class FloodLocationServiceTests
{
    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocations_ReturnsCollection_WhenSuccessful(ICollection<FloodLocation> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        repository.GetFloodLocations().Returns(expected.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocations(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four_WithPoint), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocationsByPoint_ReturnsCollection_WhenSuccessful(double easting, double northing, double buffer, ICollection<FloodLocation> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        repository.GetFloodLocationsByPoint(easting, northing, buffer).Returns(expected.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocationsByPoint(easting, northing, buffer, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four_WithReports), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocationsWithReports_ReturnsCollection_WhenSuccessful(ICollection<FloodLocation> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        repository.GetFloodLocationsWithReports().Returns(expected.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocationsWithReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocation_ReturnsFloodLocation_WhenFound(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocations().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocation(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFloodLocation_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocations().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocation(anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four_WithReports), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocationWithReports_ReturnsFloodLocationWithReports_WhenFound(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocationsWithReports().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocationWithReports(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFloodLocationWithReports_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocationsWithReports().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.GetFloodLocationWithReports(anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task CreateFloodLocation_ReturnsFloodLocation_WhenCreated(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.CreateFloodLocation(expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(default, result.CreatedUtc);
        Assert.Equal(expected.Title, result.Title);
        Assert.Equal(expected.Notes, result.Notes);
        Assert.Equal(expected.Geom, result.Geom);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    // CreateFromEligibilityCheckCreated GOES HERE

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task UpdateFloodLocation_ReturnsFloodLocation_WhenUpdated(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocations().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.UpdateFloodLocation(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(default, result.UpdatedUtc);
        Assert.Equal(expected.Title, result.Title);
        Assert.Equal(expected.Notes, result.Notes);
        Assert.Equal(expected.Geom, result.Geom);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task UpdateFloodLocation_ReturnsNull_WhenNotFound(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();
        ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodLocations().Returns(testData.BuildMock());

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.UpdateFloodLocation(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task DeleteFloodLocation_ReturnsTrue_WhenDeleted(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();

        ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodLocations.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodLocation?>(mockDbSet.FirstOrDefault(fl => fl.Id.Equals(expected.Id)));
        });
        context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.DeleteFloodLocation(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task DeleteFloodLocation_ReturnsFalse_WhenNotFound(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();

        ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodLocations.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodLocation?>((FloodLocation?)null);
        });

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.DeleteFloodLocation(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task FloodLocationExists_ReturnsTrue_WhenExists(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();

        ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodLocations.Returns(mockDbSet);

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.FloodLocationExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task FloodLocationExists_ReturnsFalse_WhenDoesntExist(FloodLocation expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodLocationService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodLocationRepository>();

        ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodLocations.Returns(mockDbSet);

        var service = new FloodLocationService(logger, context, repository);

        // Act
        var result = await service.FloodLocationExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }
}

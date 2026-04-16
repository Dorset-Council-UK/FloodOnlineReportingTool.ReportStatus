using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MockQueryable.NSubstitute;
using NetTopologySuite.Geometries;
using NSubstitute;
using Tests.Data;

namespace Tests.Services;

public class FloodLocationServiceTests
{
    //[Theory]
    //[MemberData(nameof(FloodLocationData.Collection_Four), MemberType = typeof(FloodLocationData))]
    //public async Task GetFloodLocations_ReturnsCollection_WhenSuccessful(ICollection<FloodLocation> expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    repository.GetFloodLocations().Returns(expected.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocations(TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Collection_Four_WithPoint), MemberType = typeof(FloodLocationData))]
    //public async Task GetFloodLocationsByPoint_ReturnsCollection_WhenSuccessful(double easting, double northing, double buffer, ICollection<FloodLocation> expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    repository.GetFloodLocationsByPoint(easting, northing, buffer).Returns(expected.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocationsByPoint(easting, northing, buffer, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Collection_Four_WithReports), MemberType = typeof(FloodLocationData))]
    //public async Task GetFloodLocationsWithReports_ReturnsCollection_WhenSuccessful(ICollection<FloodLocation> expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    repository.GetFloodLocationsWithReports().Returns(expected.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocationsWithReports(TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task GetFloodLocation_ReturnsFloodLocation_WhenFound(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocation(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Fact]
    //public async Task GetFloodLocation_ReturnsNull_WhenNotFound()
    //{
    //    // Arrange
    //    var anyId = Guid.CreateVersion7();
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocation(anyId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four_WithReports), MemberType = typeof(FloodLocationData))]
    //public async Task GetFloodLocationWithReports_ReturnsFloodLocationWithReports_WhenFound(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocationsWithReports().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocationWithReports(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Fact]
    //public async Task GetFloodLocationWithReports_ReturnsNull_WhenNotFound()
    //{
    //    // Arrange
    //    var anyId = Guid.CreateVersion7();
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocationsWithReports().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodLocationWithReports(anyId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task CreateFloodLocation_ReturnsFloodLocation_WhenCreated(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.CreateFloodLocation(expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.NotEqual(Guid.Empty, result.Id);
    //    Assert.NotEqual(default, result.CreatedUtc);
    //    Assert.Equal(expected.Title, result.Title);
    //    Assert.Equal(expected.Notes, result.Notes);
    //    Assert.Equal(expected.Geom, result.Geom);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodReport_WhenSuccessful(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    ICollection<FloodLocation> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = duplicates.BuildMockDbSet();
    //    context.FloodLocations.Returns(mockDbSet);

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, 50, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodReports.Received(1).Add(Arg.Any<FloodReportSource>());
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodLocation_WhenNoDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    ICollection<FloodLocation> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = duplicates.BuildMockDbSet();
    //    context.FloodLocations.Returns(mockDbSet);

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, 50, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodLocations.Received(1).Add(Arg.Any<FloodLocation>());
    //    context.FloodLocationFloodReports.DidNotReceive().AddRange(Arg.Any<IEnumerable<FloodLocationFloodReport>>());
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodLocationFloodReports_WhenDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    // Mock FindPotentialDuplicates data/tables
    //    var floodLocationId = Guid.NewGuid();
    //    var floodReportId = Guid.NewGuid();
    //    var geom = new Point(ecc.Easting, ecc.Northing) { SRID = 27700 };

    //    FloodLocation floodLocation = new()
    //    {
    //        Id = floodLocationId,
    //        CreatedUtc = ecc.CreatedUtc,
    //        Title = "Flood location",
    //        Geom = geom,
    //        FloodReports = [
    //            new FloodLocationFloodReport()
    //            {
    //                FloodLocationId = floodLocationId,
    //                FloodReportId = floodReportId,
    //                CreatedUtc = ecc.CreatedUtc,
    //                FloodReport = new FloodReportSource()
    //                {
    //                    Id = floodReportId,
    //                    CreatedUtc = ecc.CreatedUtc,
    //                    EligibilityCheckId = ecc.Id,
    //                    Uprn = ecc.Uprn,
    //                    Usrn = ecc.Usrn,
    //                    Geom = geom,
    //                    ImpactStartUtc = ecc.ImpactStartUTC,
    //                    ImpactDurationHours = 5,
    //                    Sources = [
    //                        FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.River, "River"),
    //                        FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
    //                    ],
    //                },
    //            },
    //        ],
    //    };

    //    // We need ICollection for MockQueryable.NSubstitute
    //    ICollection<FloodLocation> floodLocationDuplicates = [floodLocation];
    //    var mockFloodLocationDbSet = floodLocationDuplicates.BuildMockDbSet();
    //    context.FloodLocations.Returns(mockFloodLocationDbSet);

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, 50, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodLocations.DidNotReceive().Add(Arg.Any<FloodLocation>());
    //    context.FloodLocationFloodReports.Received(1).AddRange(Arg.Any<IEnumerable<FloodLocationFloodReport>>());
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task UpdateFloodLocation_ReturnsFloodLocation_WhenUpdated(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    // We need ICollection for MockQueryable.NSubstitute
    //    var existing = expected with
    //    {
    //        UpdatedUtc = null,
    //        Title = "FloodReportSource Title",
    //        Notes = "FloodReportSource Notes",
    //        Geom = new Point(expected.Geom.X + 10, expected.Geom.Y + 10) { SRID = 27700 },
    //    };
    //    ICollection<FloodLocation> testData = [existing];
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.UpdateFloodLocation(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.NotEqual(default, result.UpdatedUtc);
    //    Assert.Equal(expected.Title, result.Title);
    //    Assert.Equal(expected.Notes, result.Notes);
    //    Assert.Equal(expected.Geom, result.Geom);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task UpdateFloodLocation_ReturnsNull_WhenNotFound(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.UpdateFloodLocation(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //    await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task DeleteFloodLocation_ReturnsTrue_WhenDeleted(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = testData.BuildMockDbSet();
    //    context.FloodLocations.Returns(mockDbSet);
    //    mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
    //    {
    //        return new ValueTask<FloodLocation?>(mockDbSet.FirstOrDefault(fl => fl.Id.Equals(expected.Id)));
    //    });
    //    context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.DeleteFloodLocation(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.True(result);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task DeleteFloodLocation_ReturnsFalse_WhenNotFound(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = testData.BuildMockDbSet();
    //    context.FloodLocations.Returns(mockDbSet);
    //    mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
    //    {
    //        return new ValueTask<FloodLocation?>((FloodLocation?)null);
    //    });

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.DeleteFloodLocation(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.False(result);
    //    await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task FloodLocationExists_ReturnsTrue_WhenExists(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();
    //    ICollection<FloodLocation> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.FloodLocationExists(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    //public async Task FloodLocationExists_ReturnsFalse_WhenDoesntExist(FloodLocation expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodLocationService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodLocationRepository>();

    //    ICollection<FloodLocation> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodLocations().Returns(testData.BuildMock());

    //    var service = new FloodLocationService(logger, context, repository);

    //    // Act
    //    var result = await service.FloodLocationExists(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.False(result);
    //}
}

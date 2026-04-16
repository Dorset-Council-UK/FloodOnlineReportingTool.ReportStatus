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

public class FloodRoadServiceTests
{
    //[Theory]
    //[MemberData(nameof(FloodRoadData.Collection_Four), MemberType = typeof(FloodRoadData))]
    //public async Task GetFloodRoads_ReturnsCollection_WhenSuccessful(ICollection<FloodRoad> expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    repository.GetFloodRoads().Returns(expected.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoads(TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Collection_Four_WithUsrn), MemberType = typeof(FloodRoadData))]
    //public async Task GetFloodRoadsByUsrn_ReturnsCollection_WhenSuccessful(long usrn, ICollection<FloodRoad> allRoads, ICollection<FloodRoad> expectedRoads)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    var expectedAllRoads = allRoads.BuildMock();
    //    repository.GetFloodRoadsWithReports().Returns(expectedAllRoads);

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoadsByUsrn(usrn, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expectedRoads, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Collection_Four_WithReports), MemberType = typeof(FloodRoadData))]
    //public async Task GetFloodRoadsWithReports_ReturnsCollection_WhenSuccessful(ICollection<FloodRoad> expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    repository.GetFloodRoadsWithReports().Returns(expected.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoadsWithReports(TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task GetFloodRoad_ReturnsFloodRoad_WhenFound(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoad(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Fact]
    //public async Task GetFloodRoad_ReturnsNull_WhenNotFound()
    //{
    //    // Arrange
    //    var anyId = Guid.CreateVersion7();
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoad(anyId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four_WithReports), MemberType = typeof(FloodRoadData))]
    //public async Task GetFloodRoadWithReports_ReturnsFloodRoadWithReports_WhenFound(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoadsWithReports().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoadWithReports(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Equal(expected, result);
    //}

    //[Fact]
    //public async Task GetFloodRoadWithReports_ReturnsNull_WhenNotFound()
    //{
    //    // Arrange
    //    var anyId = Guid.CreateVersion7();
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoadsWithReports().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.GetFloodRoadWithReports(anyId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task CreateFloodRoad_ReturnsFloodRoad_WhenCreated(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.CreateFloodRoad(expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.NotEqual(Guid.Empty, result.Id);
    //    Assert.NotEqual(default, result.CreatedUtc);
    //    Assert.Equal(expected.Title, result.Title);
    //    Assert.Equal(expected.Usrn, result.Usrn);
    //    Assert.Equal(expected.Notes, result.Notes);
    //    Assert.Equal(expected.Geom, result.Geom);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodRoad_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodReport_WhenSuccessful(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    ICollection<FloodRoad> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = duplicates.BuildMockDbSet();
    //    context.FloodRoads.Returns(mockDbSet);

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodReports.Received(1).Add(Arg.Any<FloodReportSource>());
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodRoad_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodRoad_WhenNoDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    ICollection<FloodRoad> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = duplicates.BuildMockDbSet();
    //    context.FloodRoads.Returns(mockDbSet);

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodRoads.Received(1).Add(Arg.Any<FloodRoad>());
    //    context.FloodRoadFloodReports.DidNotReceive().AddRange(Arg.Any<IEnumerable<FloodRoadFloodReport>>());
    //}

    //[Theory]
    //[MemberData(nameof(EligibilityCheckCreatedData.FloodRoad_4), MemberType = typeof(EligibilityCheckCreatedData))]
    //public async Task CreateFromEligibilityCheckCreated_AddsFloodRoadFloodReports_WhenDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    // Mock FindPotentialDuplicates data/tables
    //    var floodRoadId = Guid.NewGuid();
    //    var floodReportId = Guid.NewGuid();
    //    var geom = new Point(ecc.Easting, ecc.Northing) { SRID = 27700 };
    //    var usrn = ecc.Usrn!.Value;

    //    FloodRoad floodRoad = new()
    //    {
    //        Id = floodRoadId,
    //        CreatedUtc = ecc.CreatedUtc,
    //        Title = locationDescription,
    //        Usrn = usrn,
    //        Geom = geom,
    //        FloodReports = [
    //            new FloodRoadFloodReport()
    //            {
    //                FloodRoadId = floodRoadId,
    //                FloodReportId = floodReportId,
    //                CreatedUtc = ecc.CreatedUtc,
    //                FloodReport = new FloodReportSource()
    //                {
    //                    Id = floodReportId,
    //                    CreatedUtc = ecc.CreatedUtc,
    //                    EligibilityCheckId = ecc.Id,
    //                    Uprn = ecc.Uprn,
    //                    Usrn = usrn,
    //                    Geom = geom,
    //                    ImpactStartUtc = ecc.ImpactStartUTC,
    //                    ImpactDurationHours = ecc.ImpactDurationHours,
    //                    Sources = [
    //                        FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.River, "River"),
    //                        FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
    //                    ],
    //                },
    //            },
    //        ],
    //    };

    //    // We need ICollection for MockQueryable.NSubstitute
    //    ICollection<FloodRoad> floodRoadDuplicates = [floodRoad];
    //    var mockFloodRoadDbSet = floodRoadDuplicates.BuildMockDbSet();
    //    context.FloodRoads.Returns(mockFloodRoadDbSet);

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

    //    // Assert
    //    context.FloodRoads.DidNotReceive().Add(Arg.Any<FloodRoad>());
    //    context.FloodRoadFloodReports.Received(1).AddRange(Arg.Any<IEnumerable<FloodRoadFloodReport>>());
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task UpdateFloodRoad_ReturnsFloodRoad_WhenUpdated(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    // We need ICollection for MockQueryable.NSubstitute
    //    var existing = expected with {
    //        UpdatedUtc = null,
    //        Title = "FloodReportSource Title",
    //        Usrn = expected.Usrn + 1,
    //        Notes = "FloodReportSource Notes",
    //        Geom = new Point(expected.Geom.X + 10, expected.Geom.Y + 10) { SRID = 27700 },
    //    };
    //    ICollection<FloodRoad> testData = [existing];
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.UpdateFloodRoad(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.NotEqual(default, result.UpdatedUtc);
    //    Assert.Equal(expected.Title, result.Title);
    //    Assert.Equal(expected.Usrn, result.Usrn);
    //    Assert.Equal(expected.Notes, result.Notes);
    //    Assert.Equal(expected.Geom, result.Geom);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task UpdateFloodRoad_ReturnsNull_WhenNotFound(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.UpdateFloodRoad(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.Null(result);
    //    await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task DeleteFloodRoad_ReturnsTrue_WhenDeleted(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    ICollection<FloodRoad> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = testData.BuildMockDbSet();
    //    context.FloodRoads.Returns(mockDbSet);
    //    mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
    //    {
    //        return new ValueTask<FloodRoad?>(mockDbSet.FirstOrDefault(fl => fl.Id.Equals(expected.Id)));
    //    });
    //    context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.DeleteFloodRoad(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.True(result);
    //    await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task DeleteFloodRoad_ReturnsFalse_WhenNotFound(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();

    //    ICollection<FloodRoad> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    var mockDbSet = testData.BuildMockDbSet();
    //    context.FloodRoads.Returns(mockDbSet);
    //    mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
    //    {
    //        return new ValueTask<FloodRoad?>((FloodRoad?)null);
    //    });

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.DeleteFloodRoad(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.False(result);
    //    await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task FloodRoadExists_ReturnsTrue_WhenExists(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.FloodRoadExists(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.True(result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    //public async Task FloodRoadExists_ReturnsFalse_WhenDoesntExist(FloodRoad expected)
    //{
    //    // Arrange
    //    var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
    //    var logger = Substitute.For<ILogger<FloodRoadService>>();
    //    var context = Substitute.For<ApplicationDbContext>(options);
    //    var repository = Substitute.For<IFloodRoadRepository>();
    //    ICollection<FloodRoad> testData = []; // We need ICollection for MockQueryable.NSubstitute
    //    repository.GetFloodRoads().Returns(testData.BuildMock());

    //    var service = new FloodRoadService(logger, context, repository);

    //    // Act
    //    var result = await service.FloodRoadExists(expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.False(result);
    //}
}

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

public class FloodPropertyServiceTests
{
    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodProperties_ReturnsCollection_WhenSuccessful(ICollection<FloodProperty> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        repository.GetFloodProperties().Returns(expected.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodProperties(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four_WithUprn), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertiesByUprn_ReturnsCollection_WhenSuccessful(long uprn, ICollection<FloodProperty> allProperties, ICollection<FloodProperty> expectedProperties)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        var expectedAllProperties = allProperties.BuildMock();
        repository.GetFloodPropertiesWithReports().Returns(expectedAllProperties);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodPropertiesByUprn(uprn, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedProperties, result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four_WithReports), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertiesWithReports_ReturnsCollection_WhenSuccessful(ICollection<FloodProperty> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        repository.GetFloodPropertiesWithReports().Returns(expected.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodPropertiesWithReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodProperty_ReturnsFloodProperty_WhenFound(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        ICollection<FloodProperty> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodProperties().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodProperty(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFloodProperty_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        ICollection<FloodProperty> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodProperties().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodProperty(anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four_WithReports), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertyWithReports_ReturnsFloodPropertyWithReports_WhenFound(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        ICollection<FloodProperty> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodPropertiesWithReports().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodPropertyWithReports(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFloodPropertyWithReports_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        ICollection<FloodProperty> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodPropertiesWithReports().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.GetFloodPropertyWithReports(anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task CreateFloodProperty_ReturnsFloodProperty_WhenCreated(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.CreateFloodProperty(expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(default, result.CreatedUtc);
        Assert.Equal(expected.Title, result.Title);
        Assert.Equal(expected.Uprn, result.Uprn);
        Assert.Equal(expected.Notes, result.Notes);
        Assert.Equal(expected.Geom, result.Geom);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    public async Task CreateFromEligibilityCheckCreated_AddsFloodReport_WhenSuccessful(EligibilityCheckCreated ecc, string locationDescription)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = duplicates.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

        // Assert
        context.FloodReports.Received(1).Add(Arg.Any<FloodReport>());
    }

    [Theory]
    [MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    public async Task CreateFromEligibilityCheckCreated_AddsFloodProperty_WhenNoDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> duplicates = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = duplicates.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

        // Assert
        context.FloodProperties.Received(1).Add(Arg.Any<FloodProperty>());
        context.FloodPropertyReports.DidNotReceive().AddRange(Arg.Any<IEnumerable<FloodPropertyFloodReport>>());
    }

    [Theory]
    [MemberData(nameof(EligibilityCheckCreatedData.FloodProperty_4), MemberType = typeof(EligibilityCheckCreatedData))]
    public async Task CreateFromEligibilityCheckCreated_AddsFloodPropertyFloodReports_WhenDuplicates(EligibilityCheckCreated ecc, string locationDescription)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        // Mock FindPotentialDuplicates data/tables
        var FloodPropertyId = Guid.NewGuid();
        var floodReportId = Guid.NewGuid();
        var geom = new Point(ecc.Easting, ecc.Northing) { SRID = 27700 };
        var uprn = ecc.Uprn!.Value;

        FloodProperty FloodProperty = new()
        {
            Id = FloodPropertyId,
            CreatedUtc = ecc.CreatedUtc,
            Title = locationDescription,
            Uprn = uprn,
            Geom = geom,
            FloodReports = [
                new FloodPropertyFloodReport()
                {
                    FloodPropertyId = FloodPropertyId,
                    FloodReportId = floodReportId,
                    CreatedUtc = ecc.CreatedUtc,
                    FloodReport = new FloodReport()
                    {
                        Id = floodReportId,
                        CreatedUtc = ecc.CreatedUtc,
                        EligibilityCheckId = ecc.Id,
                        Uprn = uprn,
                        Usrn = ecc.Usrn,
                        Geom = geom,
                        ImpactStartUtc = ecc.ImpactStartUTC,
                        ImpactDurationHours = ecc.ImpactDurationHours,
                        Sources = [
                            FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.River, "River"),
                            FloodReportSourceData.Create(floodReportId, PrimaryCauseIds.RainwaterFlowingOverTheGround, "Rainwater flowing over the ground"),
                        ],
                    },
                },
            ],
        };

        // We need ICollection for MockQueryable.NSubstitute
        ICollection<FloodProperty> FloodPropertyDuplicates = [FloodProperty];
        var mockFloodPropertyDbSet = FloodPropertyDuplicates.BuildMockDbSet();
        context.FloodProperties.Returns(mockFloodPropertyDbSet);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        await service.CreateFromEligibilityCheckCreated(ecc, locationDescription, TestContext.Current.CancellationToken);

        // Assert
        context.FloodProperties.DidNotReceive().Add(Arg.Any<FloodProperty>());
        context.FloodPropertyReports.Received(1).AddRange(Arg.Any<IEnumerable<FloodPropertyFloodReport>>());
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task UpdateFloodProperty_ReturnsFloodProperty_WhenUpdated(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        // We need ICollection for MockQueryable.NSubstitute
        var existing = expected with {
            UpdatedUtc = null,
            Title = "Old Title",
            Uprn = expected.Uprn + 1,
            Notes = "Old Notes",
            Geom = new Point(expected.Geom.X + 10, expected.Geom.Y + 10) { SRID = 27700 },
        };
        ICollection<FloodProperty> testData = [existing];
        repository.GetFloodProperties().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.UpdateFloodProperty(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(default, result.UpdatedUtc);
        Assert.Equal(expected.Title, result.Title);
        Assert.Equal(expected.Uprn, result.Uprn);
        Assert.Equal(expected.Notes, result.Notes);
        Assert.Equal(expected.Geom, result.Geom);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task UpdateFloodProperty_ReturnsNull_WhenNotFound(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();
        ICollection<FloodProperty> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodProperties().Returns(testData.BuildMock());

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.UpdateFloodProperty(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task DeleteFloodProperty_ReturnsTrue_WhenDeleted(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodProperty?>(mockDbSet.FirstOrDefault(fl => fl.Id.Equals(expected.Id)));
        });
        context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.DeleteFloodProperty(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task DeleteFloodProperty_ReturnsFalse_WhenNotFound(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> testData = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodProperty?>((FloodProperty?)null);
        });

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.DeleteFloodProperty(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task FloodPropertyExists_ReturnsTrue_WhenExists(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.FloodPropertyExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task FloodPropertyExists_ReturnsFalse_WhenDoesntExist(FloodProperty expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodPropertyService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodPropertyRepository>();

        ICollection<FloodProperty> testData = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodProperties.Returns(mockDbSet);

        var service = new FloodPropertyService(logger, context, repository);

        // Act
        var result = await service.FloodPropertyExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }
}

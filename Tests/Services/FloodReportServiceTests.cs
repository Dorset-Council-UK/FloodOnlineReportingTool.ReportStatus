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

public class FloodReportServiceTests
{
    [Theory]
    [MemberData(nameof(FloodReportData.Collection_Four), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsCollection_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        var query = expected.BuildMock();
        repository.GetFloodReports().Returns(query);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReports(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReportById_ReturnsFloodReport_WhenSuccessful(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> collection = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var query = collection.BuildMock();
        repository.GetFloodReportById(expected.Id).Returns(query);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReportById(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetFloodReportById_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodReportById(anyId).Returns(testData.BuildMock());

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReportById(anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.CollectionWithUprn_Four), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReportsByUprn_ReturnsCollection_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        var query = expected.BuildMock();
        repository.GetFloodReportsByUprn(FloodReportData.FixedUprn).Returns(query);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReportsByUprn(FloodReportData.FixedUprn, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.CollectionWithUsrn_Four), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReportsByUsrn_ReturnsCollection_WhenSuccessful(ICollection<FloodReport> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        var query = expected.BuildMock();
        repository.GetFloodReportsByUsrn(FloodReportData.FixedUsrn).Returns(query);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReportsByUsrn(FloodReportData.FixedUsrn, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Four_Within50Metres), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReportsWithinMetres_ReturnsCollection_WhenSuccessful(double easting, double northing, double metres, ICollection<FloodReport> expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        var query = expected.BuildMock();
        repository.GetFloodReportsWithinMetres(easting, northing, metres).Returns(query);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.GetFloodReportsWithinMetres(easting, northing, metres, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task CreateFloodReport_ReturnsFloodReport_WhenCreated(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.CreateFloodReport(expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(default, result.CreatedUtc);
        Assert.EquivalentWithExclusions(expected, result, nameof(expected.Id), nameof(expected.CreatedUtc), nameof(expected.Sources));
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task UpdateFloodReport_ReturnsFloodReport_WhenUpdated(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();

        // We need ICollection for MockQueryable.NSubstitute
        var existing = expected with
        {
            UpdatedUtc = null,
            EligibilityCheckId = Guid.Empty,
            Uprn = expected.Uprn + 1,
            Usrn = expected.Usrn + 1,
            Geom = new Point(expected.Geom.X + 10, expected.Geom.Y + 10) { SRID = 27700 },
            ImpactStartUtc = expected.ImpactStartUtc?.AddHours(1),
            ImpactDurationHours = expected.ImpactDurationHours + 1,
        };
        ICollection<FloodReport> testData = [existing];
        repository.GetFloodReports().Returns(testData.BuildMock());

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.UpdateFloodReport(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.UpdatedUtc);
        Assert.Equal(expected.EligibilityCheckId, result.EligibilityCheckId);
        Assert.Equal(expected.Uprn, result.Uprn);
        Assert.Equal(expected.Usrn, result.Usrn);
        Assert.Equal(expected.Geom, result.Geom);
        Assert.Equal(expected.ImpactStartUtc, result.ImpactStartUtc);
        Assert.Equal(expected.ImpactDurationHours, result.ImpactDurationHours);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task UpdateFloodReport_ReturnsNull_WhenNotFound(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodReports().Returns(testData.BuildMock());

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.UpdateFloodReport(expected.Id, expected.ToDto(), TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task DeleteFloodReport_ReturnsTrue_WhenDeleted(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();

        ICollection<FloodReport> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodReports.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodReport?>(mockDbSet.FirstOrDefault(fl => fl.Id.Equals(expected.Id)));
        });
        context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.DeleteFloodReport(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
        await context.Received(1).SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task DeleteFloodReport_ReturnsFalse_WhenNotFound(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();

        ICollection<FloodReport> testData = []; // We need ICollection for MockQueryable.NSubstitute
        var mockDbSet = testData.BuildMockDbSet();
        context.FloodReports.Returns(mockDbSet);
        mockDbSet.FindAsync(Arg.Any<object?[]?>(), Arg.Any<CancellationToken>()).Returns(callInfo =>
        {
            return new ValueTask<FloodReport?>((FloodReport?)null);
        });

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.DeleteFloodReport(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
        await context.DidNotReceive().SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> testData = [expected]; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodReports().Returns(testData.BuildMock());

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.FloodReportExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodReport expected)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var context = Substitute.For<ApplicationDbContext>(options);
        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> testData = []; // We need ICollection for MockQueryable.NSubstitute
        repository.GetFloodReports().Returns(testData.BuildMock());

        var service = new FloodReportService(logger, repository, context);

        // Act
        var result = await service.FloodReportExists(expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result);
    }
}

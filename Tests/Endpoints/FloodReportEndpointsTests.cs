using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Helpers;

namespace Tests.Endpoints;

public class FloodReportEndpointsTests
{
    [Fact]
    internal async Task GetFloodReports_ReturnsAll_WhenSuccessful()
    {
        // Arrange
        IReadOnlyCollection<FloodReport> expected = FloodReportTestHelpers.Test100FloodReports;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    internal async Task GetFloodReports_ReturnsNone_WhenThereAreNoFloodReports()
    {
        // Arrange
        IReadOnlyCollection<FloodReport> expected = [];
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportById_ReturnsFloodReport_WhenFound(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportById(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);
        
        // Act
        var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportById_ReturnsNotFound_WhenNotFound(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportById(expected.Id, Arg.Any<CancellationToken>()).Returns((FloodReport?)null);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task GetFloodReportsByUprn_ReturnsFloodReports_WhenFound()
    {
        // Arrange
        IReadOnlyCollection<FloodReport> expected = FloodReportTestHelpers.CreateFloodReports(10, sameUprn: true);
        var uprn = expected.First().Uprn ?? 0;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(uprn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, uprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
        // all UPRNs are the same
        Assert.All(okResult.Value, fr => Assert.Equal(uprn, fr.Uprn));
    }
    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test10TheoryUprns), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUprn2_ReturnsFloodReports_WhenFound(long uprn)
    {
        // Arrange
        IReadOnlyCollection<FloodReport> expected = FloodReportTestHelpers.CreateFloodReports(10, uprn: uprn);
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(uprn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, uprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
        // all UPRNs are the same
        Assert.All(okResult.Value, fr => Assert.Equal(uprn, fr.Uprn));
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUprn_ReturnsNone_WhenNotFound(FloodReport expected)
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUsrn_ReturnsFloodReports_WhenFound(FloodReport expected)
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUsrn_ReturnsNone_WhenNotFound(FloodReport expected)
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsWithinMetres_ReturnsFloodReports_WhenFound(FloodReport expected)
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsWithinMetres_ReturnsNone_WhenNotFound(FloodReport expected)
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task CreateFloodReport_ReturnsFloodReport_WhenCreated(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        var floodReportDto = expected.ToDto();
        floodReportService.CreateFloodReport(floodReportDto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.CreateFloodReport(floodReportService, floodReportDto, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodReport>>(result.Result);
        Assert.Equal($"/api/flood/reports/{expected.Id}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task UpdateFloodReport_ReturnsFloodReport_WhenUpdated(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        var floodReportDto = expected.ToDto();
        floodReportService.UpdateFloodReport(expected.Id, floodReportDto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.UpdateFloodReport(floodReportService, expected.Id, floodReportDto, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task UpdateFloodReport_ReturnsNotFound_WhenNotFound(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        var floodReportDto = expected.ToDto();
        floodReportService.UpdateFloodReport(expected.Id, floodReportDto, Arg.Any<CancellationToken>()).Returns((FloodReport?)null);

        // Act
        var result = await FloodReportEndpoints.UpdateFloodReport(floodReportService, expected.Id, floodReportDto, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task DeleteFloodReport_ReturnsNoContent_WhenDeleted(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.DeleteFloodReport(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodReportEndpoints.DeleteFloodReport(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task DeleteFloodReport_ReturnsNotFound_WhenNotFound(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.DeleteFloodReport(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodReportEndpoints.DeleteFloodReport(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.FloodReportExists(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodReportEndpoints.FloodReportExists(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.Test100TheoryFloodReports), MemberType = typeof(FloodReportTestHelpers))]
    public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodReport expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.FloodReportExists(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodReportEndpoints.FloodReportExists(floodReportService, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

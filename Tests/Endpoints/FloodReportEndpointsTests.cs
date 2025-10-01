using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Helpers;

namespace Tests.Endpoints;

public class FloodReportEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Collection), MemberType = typeof(FloodReportTestHelpers))]
    internal async Task GetFloodReports_ReturnsOkay_WhenSuccessful(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    internal async Task GetFloodReports_ReturnsOkayAndEmpty_WhenNoReportsFound()
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Empty(okResult.Value!);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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

    [Fact]
    public async Task GetFloodReportById_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodReport?)null);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Collection_WithUprn), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUprn_ReturnsFloodReports_WhenFound(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var uprn = expected.FirstOrDefault()?.Uprn ?? 1234567890;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(uprn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, uprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReportsByUprn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    {
        // Arrange
        var uprn = 1234567890;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, uprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Empty(okResult.Value!);
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Collection_WithUsrn), MemberType = typeof(FloodReportTestHelpers))]
    public async Task GetFloodReportsByUsrn_ReturnsOkay_WhenFound(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var usrn = expected.FirstOrDefault()?.Usrn ?? 123456;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUsrn(usrn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, usrn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReportsByUsrn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    {
        // Arrange
        var usrn = 1234567890;
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, usrn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Empty(okResult.Value!);
    }

    [Fact]
    public async Task GetFloodReportsWithinMetres_ReturnsOkay_WhenWithinDistance()
    {
        Assert.Fail("Not implemented");
    }

    [Fact]
    public async Task GetFloodReportsWithinMetres_ReturnsOkayAndEmpty_WhenNotWithinDistance()
    {
        Assert.Fail("Not implemented");
    }

    [Theory]
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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
    [MemberData(nameof(FloodReportTestHelpers.FloodReport_Record_10), MemberType = typeof(FloodReportTestHelpers))]
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

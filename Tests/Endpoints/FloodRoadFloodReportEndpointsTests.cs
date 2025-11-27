using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodRoadFloodReportEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.ForFloodRoad), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(Guid expectedFloodRoadId, ICollection<FloodRoadFloodReport> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodRoadFloodReport> expected = [.. testData];
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.GetFloodRoadFloodReports(expectedFloodRoadId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadFloodReportEndpoints.GetFloodReports(service, expectedFloodRoadId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodRoadFloodReport>>>(result.Result);
        var floodRoadFloodReports = okResult.Value;
        Assert.Equal(expected, okResult.Value);
        Assert.All(okResult.Value!, flfr => Assert.Equal(expectedFloodRoadId, flfr.FloodRoadId));
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task GetFloodReport_ReturnsFloodRoadFloodReport_WhenFound(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.GetFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadFloodReportEndpoints.GetFloodReport(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodRoadFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReport_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyFloodRoadId = Guid.CreateVersion7();
        var anyFloodReportId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.GetFloodRoadFloodReport(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodRoadFloodReport?)null);

        // Act
        var result = await FloodRoadFloodReportEndpoints.GetFloodReport(service, anyFloodRoadId, anyFloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task CreateFloodReport_ReturnsFloodRoadFloodReport_WhenCreated(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.CreateFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadFloodReportEndpoints.CreateFloodReport(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodRoadFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/road/{expected.FloodRoadId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.FourConfirmed), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task CreateFloodReportConfirmed_ReturnsConfirmedFloodRoadFloodReport_WhenCreated(FloodRoadFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodRoadFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodRoadFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.CreateFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, FloodRoadFloodReportData.TestConfirmedUserId, FloodRoadFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadFloodReportEndpoints.CreateFloodReportConfirmed(service, currentUserService, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodRoadFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/road/{expected.FloodRoadId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.FourConfirmed), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task ConfirmFloodReport_ReturnsConfirmedFloodRoadFloodReport_WhenConfirmed(FloodRoadFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodRoadFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodRoadFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.ConfirmFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, FloodRoadFloodReportData.TestConfirmedUserId, FloodRoadFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadFloodReportEndpoints.ConfirmFloodReport(service, currentUserService, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodRoadFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNoContent_WhenDeleted(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.DeleteFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodRoadFloodReportEndpoints.DeleteFloodReport(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNotFound_WhenNotFound(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.DeleteFloodRoadFloodReport(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodRoadFloodReportEndpoints.DeleteFloodReport(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.FloodRoadFloodReportExists(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodRoadFloodReportEndpoints.FloodReportExists(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadFloodReportData.Four), MemberType = typeof(FloodRoadFloodReportData))]
    public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodRoadFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadFloodReportService>();
        service.FloodRoadFloodReportExists(expected.FloodRoadId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodRoadFloodReportEndpoints.FloodReportExists(service, expected.FloodRoadId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

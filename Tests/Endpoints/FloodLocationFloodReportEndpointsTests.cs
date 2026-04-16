using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodLocationFloodReportEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.ForFloodLocation), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(Guid expectedFloodLocationId, ICollection<FloodLocationFloodReport> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodLocationFloodReport> expected = [.. testData];
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.GetFloodLocationFloodReports(expectedFloodLocationId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationFloodReportEndpoints.GetFloodReports(service, expectedFloodLocationId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodLocationFloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
        Assert.All(okResult.Value!, report => Assert.Equal(expectedFloodLocationId, report.FloodLocationId));
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task GetFloodReport_ReturnsFloodLocationFloodReport_WhenFound(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.GetFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationFloodReportEndpoints.GetFloodReport(service, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodLocationFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReport_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyFloodLocationId = Guid.CreateVersion7();
        var anyFloodReportId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.GetFloodLocationFloodReport(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodLocationFloodReport?)null);

        // Act
        var result = await FloodLocationFloodReportEndpoints.GetFloodReport(service, anyFloodLocationId, anyFloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task CreateFloodReport_ReturnsFloodLocationFloodReport_WhenCreated(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.CreateFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, expected.FloodReportSourceId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationFloodReportEndpoints.CreateFloodReport(service, expected.FloodLocationId, expected.FloodReportId, expected.FloodReportSourceId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodLocationFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/location/{expected.FloodLocationId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.FourConfirmed), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task CreateFloodReportConfirmed_ReturnsConfirmedFloodLocationFloodReport_WhenCreated(FloodLocationFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodLocationFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodLocationFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.CreateFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, expected.FloodReportSourceId, FloodLocationFloodReportData.TestConfirmedUserId, FloodLocationFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationFloodReportEndpoints.CreateFloodReportConfirmed(service, currentUserService, expected.FloodLocationId, expected.FloodReportId, expected.FloodReportSourceId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodLocationFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/location/{expected.FloodLocationId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.FourConfirmed), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task ConfirmFloodReport_ReturnsConfirmedFloodLocationFloodReport_WhenConfirmed(FloodLocationFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodLocationFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodLocationFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.ConfirmFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, FloodLocationFloodReportData.TestConfirmedUserId, FloodLocationFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationFloodReportEndpoints.ConfirmFloodReport(service, currentUserService, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodLocationFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNoContent_WhenDeleted(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.DeleteFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodLocationFloodReportEndpoints.DeleteFloodReport(service, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNotFound_WhenNotFound(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.DeleteFloodLocationFloodReport(expected.FloodLocationId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodLocationFloodReportEndpoints.DeleteFloodReport(service, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.FloodLocationFloodReportExists(expected.FloodLocationId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodLocationFloodReportEndpoints.FloodReportExists(service, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationFloodReportData.Four), MemberType = typeof(FloodLocationFloodReportData))]
    public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodLocationFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationFloodReportService>();
        service.FloodLocationFloodReportExists(expected.FloodLocationId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodLocationFloodReportEndpoints.FloodReportExists(service, expected.FloodLocationId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

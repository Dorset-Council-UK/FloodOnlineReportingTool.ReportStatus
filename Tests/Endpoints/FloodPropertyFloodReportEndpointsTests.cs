using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodPropertyFloodReportEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.ForFloodProperty), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(Guid expectedFloodPropertyId, ICollection<FloodPropertyFloodReport> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodPropertyFloodReport> expected = [.. testData];
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.GetFloodPropertyFloodReports(expectedFloodPropertyId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.GetFloodReports(service, expectedFloodPropertyId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodPropertyFloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
        Assert.All(okResult.Value!, flfr => Assert.Equal(expectedFloodPropertyId, flfr.FloodPropertyId));
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task GetFloodReport_ReturnsFloodPropertyFloodReport_WhenFound(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.GetFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.GetFloodReport(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodPropertyFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReport_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyFloodPropertyId = Guid.CreateVersion7();
        var anyFloodReportId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.GetFloodPropertyFloodReport(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodPropertyFloodReport?)null);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.GetFloodReport(service, anyFloodPropertyId, anyFloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task CreateFloodReport_ReturnsFloodPropertyFloodReport_WhenCreated(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.CreateFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.CreateFloodReport(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodPropertyFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/property/{expected.FloodPropertyId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.FourConfirmed), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task CreateFloodReportConfirmed_ReturnsConfirmedFloodPropertyFloodReport_WhenCreated(FloodPropertyFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodPropertyFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodPropertyFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.CreateFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, FloodPropertyFloodReportData.TestConfirmedUserId, FloodPropertyFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.CreateFloodReportConfirmed(service, currentUserService, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodPropertyFloodReport>>(result.Result);
        Assert.Equal($"/api/flood/property/{expected.FloodPropertyId}/reports/{expected.FloodReportId}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.FourConfirmed), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task ConfirmFloodReport_ReturnsConfirmedFloodPropertyFloodReport_WhenConfirmed(FloodPropertyFloodReport expected)
    {
        // Arrange
        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserId.Returns(FloodPropertyFloodReportData.TestConfirmedUserId);
        currentUserService.Name.Returns(FloodPropertyFloodReportData.TestConfirmedUserName);

        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.ConfirmFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, FloodPropertyFloodReportData.TestConfirmedUserId, FloodPropertyFloodReportData.TestConfirmedUserName, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.ConfirmFloodReport(service, currentUserService, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodPropertyFloodReport>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNoContent_WhenDeleted(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.DeleteFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.DeleteFloodReport(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task DeleteFloodReport_ReturnsNotFound_WhenNotFound(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.DeleteFloodPropertyFloodReport(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.DeleteFloodReport(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.FloodPropertyFloodReportExists(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.FloodReportExists(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyFloodReportData.Four), MemberType = typeof(FloodPropertyFloodReportData))]
    public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodPropertyFloodReport expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyFloodReportService>();
        service.FloodPropertyFloodReportExists(expected.FloodPropertyId, expected.FloodReportId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodPropertyFloodReportEndpoints.FloodReportExists(service, expected.FloodPropertyId, expected.FloodReportId, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

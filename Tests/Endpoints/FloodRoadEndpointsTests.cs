using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodRoadEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodRoadData.Collection_Four), MemberType = typeof(FloodRoadData))]
    public async Task GetFloodRoads_ReturnsOkay_WhenSuccessful(ICollection<FloodRoad> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodRoad> expected = [.. testData];
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoads(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoads(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodRoad>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Collection_Four_WithUsrn), MemberType = typeof(FloodRoadData))]
    public async Task GetFloodRoadsByUsrn_ReturnsOkay_WhenSuccessful(long usrn, ICollection<FloodRoad> _, ICollection<FloodRoad> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodRoad> expected = [.. testData];
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoadsByUsrn(usrn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoadsByUsrn(service, usrn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodRoad>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Collection_Four_WithReports), MemberType = typeof(FloodRoadData))]
    public async Task GetFloodRoadsWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodRoad> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodRoad> expected = [.. testData];
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoadsWithReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoadsWithReports(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodRoad>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task GetFloodRoad_ReturnsFloodRoad_WhenFound(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoad(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoad(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodRoad>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodRoad_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoad(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodRoad?)null);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoad(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four_WithReports), MemberType = typeof(FloodRoadData))]
    public async Task GetFloodRoadWithReports_ReturnsFloodRoadWithReports_WhenFound(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoadWithReports(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoadWithReports(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodRoad>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodRoadWithReports_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodRoadService>();
        service.GetFloodRoadWithReports(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodRoad?)null);

        // Act
        var result = await FloodRoadEndpoints.GetFloodRoadWithReports(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task CreateFloodRoad_ReturnsFloodRoad_WhenCreated(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        var dto = expected.ToDto();
        service.CreateFloodRoad(dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.CreateFloodRoad(service, dto, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodRoad>>(result.Result);
        Assert.Equal($"/api/flood/road/{expected.Id}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task UpdateFloodRoad_ReturnsFloodRoad_WhenUpdated(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        var dto = expected.ToDto();
        service.UpdateFloodRoad(expected.Id, dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodRoadEndpoints.UpdateFloodRoad(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodRoad>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task UpdateFloodRoad_ReturnsNotFound_WhenNotFound(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        var dto = expected.ToDto();
        service.UpdateFloodRoad(expected.Id, dto, Arg.Any<CancellationToken>()).Returns((FloodRoad?)null);

        // Act
        var result = await FloodRoadEndpoints.UpdateFloodRoad(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task DeleteFloodRoad_ReturnsNoContent_WhenDeleted(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.DeleteFloodRoad(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodRoadEndpoints.DeleteFloodRoad(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task DeleteFloodRoad_ReturnsNotFound_WhenNotFound(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.DeleteFloodRoad(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodRoadEndpoints.DeleteFloodRoad(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task FloodRoadExists_ReturnsTrue_WhenExists(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.FloodRoadExists(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodRoadEndpoints.FloodRoadExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodRoadData.Four), MemberType = typeof(FloodRoadData))]
    public async Task FloodRoadExists_ReturnsFalse_WhenDoesntExist(FloodRoad expected)
    {
        // Arrange
        var service = Substitute.For<IFloodRoadService>();
        service.FloodRoadExists(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodRoadEndpoints.FloodRoadExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

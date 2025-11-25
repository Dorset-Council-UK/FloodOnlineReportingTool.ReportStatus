using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodLocationEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocations_ReturnsOkay_WhenSuccessful(ICollection<FloodLocation> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodLocation> expected = [.. testData];
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocations(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocations(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodLocation>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Collection_Four_WithReports), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocationsWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodLocation> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodLocation> expected = [.. testData];
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocationsWithReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocationsWithReports(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodLocation>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocation_ReturnsFloodLocation_WhenFound(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocation(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocation(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodLocation>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodLocation_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocation(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodLocation?)null);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocation(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }


    [Theory]
    [MemberData(nameof(FloodLocationData.Four_WithReports), MemberType = typeof(FloodLocationData))]
    public async Task GetFloodLocationWithReports_ReturnsFloodLocationWithReports_WhenFound(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocationWithReports(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocationWithReports(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodLocation>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodLocationWithReports_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodLocationService>();
        service.GetFloodLocationWithReports(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodLocation?)null);

        // Act
        var result = await FloodLocationEndpoints.GetFloodLocationWithReports(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task CreateFloodLocation_ReturnsFloodLocation_WhenCreated(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        var dto = expected.ToDto();
       service.CreateFloodLocation(dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.CreateFloodLocation(service, dto, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodLocation>>(result.Result);
        Assert.Equal($"/api/flood/location/{expected.Id}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task UpdateFloodLocation_ReturnsFloodLocation_WhenUpdated(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        var dto = expected.ToDto();
        service.UpdateFloodLocation(expected.Id, dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodLocationEndpoints.UpdateFloodLocation(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodLocation>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task UpdateFloodLocation_ReturnsNotFound_WhenNotFound(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        var dto = expected.ToDto();
        service.UpdateFloodLocation(expected.Id, dto, Arg.Any<CancellationToken>()).Returns((FloodLocation?)null);

        // Act
        var result = await FloodLocationEndpoints.UpdateFloodLocation(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task DeleteFloodLocation_ReturnsNoContent_WhenDeleted(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.DeleteFloodLocation(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodLocationEndpoints.DeleteFloodLocation(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task DeleteFloodLocation_ReturnsNotFound_WhenNotFound(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.DeleteFloodLocation(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodLocationEndpoints.DeleteFloodLocation(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task FloodLocationExists_ReturnsTrue_WhenExists(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.FloodLocationExists(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodLocationEndpoints.FloodLocationExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodLocationData.Four), MemberType = typeof(FloodLocationData))]
    public async Task FloodLocationExists_ReturnsFalse_WhenDoesntExist(FloodLocation expected)
    {
        // Arrange
        var service = Substitute.For<IFloodLocationService>();
        service.FloodLocationExists(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodLocationEndpoints.FloodLocationExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

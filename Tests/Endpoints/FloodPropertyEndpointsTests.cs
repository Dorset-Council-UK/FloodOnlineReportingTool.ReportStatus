using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodPropertyEndpointsTests
{
    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodProperties_ReturnsOkay_WhenSuccessful(ICollection<FloodProperty> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodProperty> expected = [.. testData];
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodProperties(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodProperties(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodProperty>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four_WithUprn), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertiesByUprn_ReturnsOkay_WhenSuccessful(long uprn, ICollection<FloodProperty> _, ICollection<FloodProperty> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodProperty> expected = [.. testData];
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodPropertiesByUprn(uprn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodPropertiesByUprn(service, uprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodProperty>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Collection_Four_WithReports), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertiesWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodProperty> testData)
    {
        // Arrange
        IReadOnlyCollection<FloodProperty> expected = [.. testData];
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodPropertiesWithReports(Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodPropertiesWithReports(service, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodProperty>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodProperty_ReturnsFloodProperty_WhenFound(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodProperty(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodProperty(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodProperty>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodProperty_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodProperty(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodProperty?)null);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodProperty(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four_WithReports), MemberType = typeof(FloodPropertyData))]
    public async Task GetFloodPropertyWithReports_ReturnsFloodPropertyWithReports_WhenFound(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodPropertyWithReports(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodPropertyWithReports(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodProperty>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodPropertyWithReports_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var anyId = Guid.CreateVersion7();
        var service = Substitute.For<IFloodPropertyService>();
        service.GetFloodPropertyWithReports(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodProperty?)null);

        // Act
        var result = await FloodPropertyEndpoints.GetFloodPropertyWithReports(service, anyId, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task CreateFloodProperty_ReturnsFloodProperty_WhenCreated(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        var dto = expected.ToDto();
        service.CreateFloodProperty(dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.CreateFloodProperty(service, dto, TestContext.Current.CancellationToken);

        // Assert
        var createdResult = Assert.IsType<Created<FloodProperty>>(result.Result);
        Assert.Equal($"/api/flood/property/{expected.Id}", createdResult.Location);
        Assert.Equal(expected, createdResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task UpdateFloodProperty_ReturnsFloodProperty_WhenUpdated(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        var dto = expected.ToDto();
        service.UpdateFloodProperty(expected.Id, dto, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodPropertyEndpoints.UpdateFloodProperty(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodProperty>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task UpdateFloodProperty_ReturnsNotFound_WhenNotFound(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        var dto = expected.ToDto();
        service.UpdateFloodProperty(expected.Id, dto, Arg.Any<CancellationToken>()).Returns((FloodProperty?)null);

        // Act
        var result = await FloodPropertyEndpoints.UpdateFloodProperty(service, expected.Id, dto, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task DeleteFloodProperty_ReturnsNoContent_WhenDeleted(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.DeleteFloodProperty(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodPropertyEndpoints.DeleteFloodProperty(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task DeleteFloodProperty_ReturnsNotFound_WhenNotFound(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.DeleteFloodProperty(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodPropertyEndpoints.DeleteFloodProperty(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task FloodPropertyExists_ReturnsTrue_WhenExists(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.FloodPropertyExists(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await FloodPropertyEndpoints.FloodPropertyExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.True(okResult.Value);
    }

    [Theory]
    [MemberData(nameof(FloodPropertyData.Four), MemberType = typeof(FloodPropertyData))]
    public async Task FloodPropertyExists_ReturnsFalse_WhenDoesntExist(FloodProperty expected)
    {
        // Arrange
        var service = Substitute.For<IFloodPropertyService>();
        service.FloodPropertyExists(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await FloodPropertyEndpoints.FloodPropertyExists(service, expected.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result.Result);
        Assert.False(okResult.Value);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodReportSourceEndpointsTests
{
    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.ForFloodReport), MemberType = typeof(FloodReportSourceData))]
    //public async Task GetFloodReportSources_ReturnsOkay_WhenSuccessful(Guid expectedFloodReportId, ICollection<FloodReportSource> testData)
    //{
    //    // Arrange
    //    IReadOnlyCollection<FloodReportSource> expected = [.. testData];
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.GetFloodReportSources(expectedFloodReportId, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.GetFloodReportSources(service, expectedFloodReportId, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //    Assert.All(okResult.Value!, source => Assert.Equal(expectedFloodReportId, source.FloodReportId));
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task GetFloodReportSource_ReturnsFloodReportSource_WhenFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.GetFloodReportSource(expected.FloodReportId, expected.SourceId, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.GetFloodReportSource(service, expected.FloodReportId, expected.SourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<FloodReportSource>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task GetFloodReportSource_ReturnsNotFound_WhenNotFound()
    //{
    //    // Arrange
    //    var anyFloodReportId = Guid.NewGuid();
    //    var anySourceId = Guid.NewGuid();
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.GetFloodReportSource(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodReportSource?)null);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.GetFloodReportSource(service, anyFloodReportId, anySourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task CreateReportSource_ReturnsFloodReportSource_WhenCreated(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    var dto = expected.ToDto();
    //    service.CreateFloodReportSource(expected.FloodReportId, expected.SourceId, dto, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.CreateReportSource(service, expected.FloodReportId, expected.SourceId, dto, TestContext.Current.CancellationToken);

    //    // Assert
    //    var createdResult = Assert.IsType<Created<FloodReportSource>>(result.Result);
    //    Assert.Equal($"/api/flood/reports/{expected.FloodReportId}/sources/{expected.SourceId}", createdResult.Location);
    //    Assert.Equal(expected, createdResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task UpdateReportSource_ReturnsFloodReportSource_WhenUpdated(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    var dto = expected.ToDto();
    //    service.UpdateFloodReportSource(expected.FloodReportId, expected.SourceId, dto, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.UpdateReportSource(service, expected.FloodReportId, expected.SourceId, dto, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<FloodReportSource>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task UpdateReportSource_ReturnsNotFound_WhenNotFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    var dto = expected.ToDto();
    //    service.UpdateFloodReportSource(expected.FloodReportId, expected.SourceId, dto, Arg.Any<CancellationToken>()).Returns((FloodReportSource?)null);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.UpdateReportSource(service, expected.FloodReportId, expected.SourceId, dto, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task DeleteReportSource_ReturnsNoContent_WhenDeleted(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.DeleteFloodReportSource(expected.FloodReportId, expected.SourceId, Arg.Any<CancellationToken>()).Returns(true);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.DeleteReportSource(service, expected.FloodReportId, expected.SourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NoContent>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task DeleteReportSource_ReturnsNotFound_WhenNotFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.DeleteFloodReportSource(expected.FloodReportId, expected.SourceId, Arg.Any<CancellationToken>()).Returns(false);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.DeleteReportSource(service, expected.FloodReportId, expected.SourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task FloodReportSourceExists_ReturnsTrue_WhenExists(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.FloodReportSourceExists(expected.FloodReportId, expected.SourceId, Arg.Any<CancellationToken>()).Returns(true);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.FloodReportSourceExists(service, expected.FloodReportId, expected.SourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<bool>>(result.Result);
    //    Assert.True(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportSourceData.Four), MemberType = typeof(FloodReportSourceData))]
    //public async Task FloodReportSourceExists_ReturnsFalse_WhenDoesntExist(FloodReportSource expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<IFloodReportSourceService>();
    //    service.FloodReportSourceExists(expected.FloodReportId, expected.SourceId, Arg.Any<CancellationToken>()).Returns(false);

    //    // Act
    //    var result = await FloodReportSourceEndpoints.FloodReportSourceExists(service, expected.FloodReportId, expected.SourceId, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<bool>>(result.Result);
    //    Assert.False(okResult.Value);
    //}
}

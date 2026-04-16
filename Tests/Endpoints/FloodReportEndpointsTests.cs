using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodReportEndpointsTests
{
    //[Theory]
    //[MemberData(nameof(FloodReportData.Collection_Four), MemberType = typeof(FloodReportData))]
    //public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(ICollection<FloodReportSource> testData)
    //{
    //    // Arrange
    //    IReadOnlyCollection<FloodReportSource> expected = [.. testData];
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //internal async Task GetFloodReports_ReturnsOkayAndEmpty_WhenNoReportsFound()
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReports(Arg.Any<CancellationToken>()).Returns([]);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReports(floodReportService, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.NotNull(okResult.Value);
    //    Assert.Empty(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task GetFloodReportById_ReturnsFloodReport_WhenFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportById(expected.Id, Arg.Any<CancellationToken>()).Returns(expected);
        
    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<FloodReportSource>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task GetFloodReportById_ReturnsNotFound_WhenNotFound()
    //{
    //    // Arrange
    //    var anyId = Guid.CreateVersion7();
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((FloodReportSource?)null);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, anyId, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.CollectionWithUprn_Four), MemberType = typeof(FloodReportData))]
    //public async Task GetFloodReportsByUprn_ReturnsFloodReports_WhenFound(ICollection<FloodReportSource> testData)
    //{
    //    // Arrange
    //    IReadOnlyCollection<FloodReportSource> expected = [.. testData];
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsByUprn(10023242411, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, 10023242411, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task GetFloodReportsByUprn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, FloodReportData.FixedUprn, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.NotNull(okResult.Value);
    //    Assert.Empty(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.CollectionWithUsrn_Four), MemberType = typeof(FloodReportData))]
    //public async Task GetFloodReportsByUsrn_ReturnsOkay_WhenFound(ICollection<FloodReportSource> testData)
    //{
    //    // Arrange
    //    IReadOnlyCollection<FloodReportSource> expected = [.. testData];
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsByUsrn(FloodReportData.FixedUsrn, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, FloodReportData.FixedUsrn, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task GetFloodReportsByUsrn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, FloodReportData.FixedUsrn, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.NotNull(okResult.Value);
    //    Assert.Empty(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Four_Within50Metres), MemberType = typeof(FloodReportData))]
    //public async Task GetFloodReportsWithinMetres_ReturnsOkay_WhenWithinDistance(double easting, double northing, double metres, ICollection<FloodReportSource> testData)
    //{
    //    // Arrange
    //    IReadOnlyCollection<FloodReportSource> expected = [.. testData];
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsWithinMetres(easting, northing, metres, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsWithinMetres(floodReportService, easting, northing, metres, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task GetFloodReportsWithinMetres_ReturnsOkayAndEmpty_WhenNotWithinDistance()
    //{
    //    // Arrange
    //    var metres = 50.0;
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.GetFloodReportsWithinMetres(Arg.Any<double>(), Arg.Any<double>(), metres, Arg.Any<CancellationToken>()).Returns([]);

    //    // Act
    //    var result = await FloodReportEndpoints.GetFloodReportsWithinMetres(floodReportService, FloodReportData.Easting, FloodReportData.Northing, metres, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReportSource>>>(result.Result);
    //    Assert.NotNull(okResult.Value);
    //    Assert.Empty(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task CreateFloodReport_ReturnsFloodReport_WhenCreated(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    var floodReportDto = expected.ToDto();
    //    floodReportService.CreateFloodReport(floodReportDto, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.CreateFloodReport(floodReportService, floodReportDto, TestContext.Current.CancellationToken);

    //    // Assert
    //    var createdResult = Assert.IsType<Created<FloodReportSource>>(result.Result);
    //    Assert.Equal($"/api/flood/reports/{expected.Id}", createdResult.Location);
    //    Assert.Equal(expected, createdResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task UpdateFloodReport_ReturnsFloodReport_WhenUpdated(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    var floodReportDto = expected.ToDto();
    //    floodReportService.UpdateFloodReport(expected.Id, floodReportDto, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await FloodReportEndpoints.UpdateFloodReport(floodReportService, expected.Id, floodReportDto, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<FloodReportSource>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task UpdateFloodReport_ReturnsNotFound_WhenNotFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    var floodReportDto = expected.ToDto();
    //    floodReportService.UpdateFloodReport(expected.Id, floodReportDto, Arg.Any<CancellationToken>()).Returns((FloodReportSource?)null);

    //    // Act
    //    var result = await FloodReportEndpoints.UpdateFloodReport(floodReportService, expected.Id, floodReportDto, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task DeleteFloodReport_ReturnsNoContent_WhenDeleted(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.DeleteFloodReport(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

    //    // Act
    //    var result = await FloodReportEndpoints.DeleteFloodReport(floodReportService, expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NoContent>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task DeleteFloodReport_ReturnsNotFound_WhenNotFound(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.DeleteFloodReport(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

    //    // Act
    //    var result = await FloodReportEndpoints.DeleteFloodReport(floodReportService, expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task FloodReportExists_ReturnsTrue_WhenExists(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.FloodReportExists(expected.Id, Arg.Any<CancellationToken>()).Returns(true);

    //    // Act
    //    var result = await FloodReportEndpoints.FloodReportExists(floodReportService, expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<bool>>(result.Result);
    //    Assert.True(okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(FloodReportData.Three), MemberType = typeof(FloodReportData))]
    //public async Task FloodReportExists_ReturnsFalse_WhenDoesntExist(FloodReportSource expected)
    //{
    //    // Arrange
    //    var floodReportService = Substitute.For<IFloodReportService>();
    //    floodReportService.FloodReportExists(expected.Id, Arg.Any<CancellationToken>()).Returns(false);

    //    // Act
    //    var result = await FloodReportEndpoints.FloodReportExists(floodReportService, expected.Id, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<bool>>(result.Result);
    //    Assert.False(okResult.Value);
    //}
}

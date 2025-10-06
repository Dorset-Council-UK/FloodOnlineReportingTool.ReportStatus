using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class FloodReportEndpointsTests
{
    public static IEnumerable<object[]> FloodReport_3()
    {
        yield return new object[] { FloodReportData.FloodReport1 };
        yield return new object[] { FloodReportData.FloodReport2 };
        yield return new object[] { FloodReportData.FloodReport3 };
    }

    public static IEnumerable<object[]> FloodReport_Collections_4()
    {
        IReadOnlyCollection<FloodReport> empty = [];
        yield return new object[] { empty };

        IReadOnlyCollection<FloodReport> one = [FloodReportData.FloodReport1];
        yield return new object[] { one };

        IReadOnlyCollection<FloodReport> two = [FloodReportData.FloodReport1, FloodReportData.FloodReport2];
        yield return new object[] { two };

        IReadOnlyCollection<FloodReport> three = [FloodReportData.FloodReport1, FloodReportData.FloodReport2, FloodReportData.FloodReport3];
        yield return new object[] { three };
    }

    public static IEnumerable<object[]> FloodReportWithUprn_Collections_4()
    {
        IReadOnlyCollection<FloodReport> empty = [];
        yield return new object[] { empty };

        IReadOnlyCollection<FloodReport> one = [FloodReportData.FloodReportWithUprn1];
        yield return new object[] { one };

        IReadOnlyCollection<FloodReport> two = [FloodReportData.FloodReportWithUprn1, FloodReportData.FloodReportWithUprn2];
        yield return new object[] { two };

        IReadOnlyCollection<FloodReport> three = [FloodReportData.FloodReportWithUprn1, FloodReportData.FloodReportWithUprn2, FloodReportData.FloodReportWithUprn3];
        yield return new object[] { three };
    }

    public static IEnumerable<object[]> FloodReportWithUsrn_Collections_4()
    {
        IReadOnlyCollection<FloodReport> empty = [];
        yield return new object[] { empty };

        IReadOnlyCollection<FloodReport> one = [FloodReportData.FloodReportWithUsrn1];
        yield return new object[] { one };

        IReadOnlyCollection<FloodReport> two = [FloodReportData.FloodReportWithUsrn1, FloodReportData.FloodReportWithUsrn2];
        yield return new object[] { two };

        IReadOnlyCollection<FloodReport> three = [FloodReportData.FloodReportWithUsrn1, FloodReportData.FloodReportWithUsrn2, FloodReportData.FloodReportWithUsrn3];
        yield return new object[] { three };
    }

    [Theory]
    [MemberData(nameof(FloodReport_Collections_4))]
    public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(IReadOnlyCollection<FloodReport> expected)
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReportWithUprn_Collections_4))]
    public async Task GetFloodReportsByUprn_ReturnsFloodReports_WhenFound(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(10023242411, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, 10023242411, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReportsByUprn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUprn(floodReportService, FloodReportData.FixedUprn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Empty(okResult.Value!);
    }

    [Theory]
    [MemberData(nameof(FloodReportWithUsrn_Collections_4))]
    public async Task GetFloodReportsByUsrn_ReturnsOkay_WhenFound(IReadOnlyCollection<FloodReport> expected)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUsrn(FloodReportData.FixedUsrn, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, FloodReportData.FixedUsrn, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyCollection<FloodReport>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetFloodReportsByUsrn_ReturnsOkayAndEmpty_WhenNoFloodReportsFound()
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportsByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await FloodReportEndpoints.GetFloodReportsByUsrn(floodReportService, FloodReportData.FixedUsrn, TestContext.Current.CancellationToken);

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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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
    [MemberData(nameof(FloodReport_3))]
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

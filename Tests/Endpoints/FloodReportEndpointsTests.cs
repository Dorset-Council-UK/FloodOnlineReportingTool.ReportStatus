using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Helpers;

namespace Tests.Endpoints;

public class FloodReportEndpointsTests
{
    public static IEnumerable<TheoryDataRow<FloodReport>> GoodFloodReports => FloodReportTestHelpers.CreateFloodReports(1);

    //[Fact]
    //internal async Task GetReports_ReturnsList_WhenSuccessful()
    //{
    //    // Arrange
    //    IReadOnlyCollection<Report> reports = [
    //        CreateTestReport(),
    //        CreateTestReport(),
    //    ];
    //    _reportServiceMock.GetReports(Arg.Any<CancellationToken>()).Returns(reports);

    //    // Act
    //    var result = await ReportEndpoints.GetReports(_reportServiceMock, CancellationToken.None);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<IReadOnlyCollection<Report>>>(result.Result);
    //    Assert.Equal(reports, okResult.Value);
    //}

    [Theory]
    [MemberData(nameof(GoodFloodReports))]
    public async Task GetFloodReport_ById_ReturnsReport_WhenFound(FloodReport floodReport)
    {
        // Arrange
        var floodReportService = Substitute.For<IFloodReportService>();
        floodReportService.GetFloodReportById(floodReport.Id, Arg.Any<CancellationToken>()).Returns(floodReport);
        
        // Act
        var result = await FloodReportEndpoints.GetFloodReportById(floodReportService, floodReport.Id, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<FloodReport>>(result.Result);
        Assert.Equal(floodReport, okResult.Value);
        Assert.Equal(floodReport.Id, okResult.Value?.Id);
    }

    //[Fact]
    //public async Task GetReport_ById_ReturnsNotFound_WhenNotFound()
    //{
    //    // Arrange
    //    var reportId = Guid.CreateVersion7();
    //    _reportServiceMock.GetReportById(reportId, Arg.Any<CancellationToken>()).Returns((Report?)null);

    //    // Act
    //    var result = await ReportEndpoints.GetReportById(_reportServiceMock, reportId, CancellationToken.None);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Fact]
    //public async Task CreateReport_ReturnsReport_WhenCreated()
    //{
    //    // Arrange
    //    var createdReport = CreateTestReport();
    //    var reportDto = createdReport.ToDto();
    //    _reportServiceMock.CreateReport(reportDto, Arg.Any<CancellationToken>()).Returns(createdReport);

    //    // Act
    //    var result = await ReportEndpoints.CreateReport(_reportServiceMock, reportDto, CancellationToken.None);

    //    // Assert
    //    var createdResult = Assert.IsType<Created<Report>>(result.Result);
    //    Assert.Equal($"/api/reports/{createdReport.Id}", createdResult.Location);
    //    Assert.Equal(createdReport, createdResult.Value);
    //    Assert.Equal(createdReport.Id, createdResult.Value!.Id);
    //}

    //[Fact]
    //public async Task UpdateReport_ReturnsReport_WhenSuccessful()
    //{
    //    // Arrange
    //    var reportId = Guid.CreateVersion7();
    //    var updatedReport = CreateTestReport(reportId);
    //    var reportDto = updatedReport.ToDto();
    //    _reportServiceMock.UpdateReport(reportId, reportDto, Arg.Any<CancellationToken>()).Returns(updatedReport);

    //    // Act
    //    var result = await ReportEndpoints.UpdateReport(_reportServiceMock, reportId, reportDto, CancellationToken.None);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<Report>>(result.Result);
    //    Assert.Equal(updatedReport, okResult.Value);
    //    Assert.Equal(reportId, okResult.Value!.Id);
    //}

    //[Fact]
    //public async Task UpdateReport_ReturnsNotFound_WhenNotFound()
    //{
    //    // Arrange
    //    var reportId = Guid.CreateVersion7();
    //    var updatedReport = CreateTestReport(reportId);
    //    var reportDto = updatedReport.ToDto();
    //    _reportServiceMock.UpdateReport(reportId, reportDto, Arg.Any<CancellationToken>()).Returns((Report?)null);

    //    // Act
    //    var result = await ReportEndpoints.UpdateReport(_reportServiceMock, reportId, reportDto, CancellationToken.None);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}

    //[Fact]
    //public async Task DeleteReport_ReturnsNoContent_WhenDeleted()
    //{
    //    // Arrange
    //    var reportId = Guid.CreateVersion7();
    //    _reportServiceMock.DeleteReport(reportId, Arg.Any<CancellationToken>()).Returns(returnThis: true);

    //    // Act
    //    var result = await ReportEndpoints.DeleteReport(_reportServiceMock, reportId, CancellationToken.None);

    //    // Assert
    //    Assert.IsType<NoContent>(result.Result);
    //}

    //[Fact]
    //public async Task DeleteReport_ReturnsNotFound_WhenNotFound()
    //{
    //    // Arrange
    //    var reportId = Guid.CreateVersion7();
    //    _reportServiceMock.DeleteReport(reportId, Arg.Any<CancellationToken>()).Returns(returnThis: false);

    //    // Act
    //    var result = await ReportEndpoints.DeleteReport(_reportServiceMock, reportId, CancellationToken.None);

    //    // Assert
    //    Assert.IsType<NotFound>(result.Result);
    //}
}

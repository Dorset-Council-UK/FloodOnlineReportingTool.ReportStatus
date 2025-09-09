using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace Tests.Endpoints;

public class ReportEndpointsTests
{
    private readonly IReportService _reportServiceMock;

    public ReportEndpointsTests()
    {
        _reportServiceMock = Substitute.For<IReportService>();
    }

    private static Report CreateTestReport(Guid? id = null)
    {
        return new()
        {
            Id = id ?? Guid.CreateVersion7(),
            EligibilityCheckId = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,
            UpdatedUtc = DateTimeOffset.UtcNow,
            Uprn = 1234567890L,
            Easting = 123456.78,
            Northing = 987654.32,
            ImpactStartUtc = DateTimeOffset.UtcNow.AddHours(-1),
            ImpactDurationHours = 2,
        };
    }

    [Fact]
    internal async Task GetReports_ReturnsList_WhenSuccessful()
    {
        // Arrange
        IReadOnlyList<Report> reports = [
            CreateTestReport(),
            CreateTestReport(),
        ];
        _reportServiceMock.GetReports(Arg.Any<CancellationToken>()).Returns(reports);

        // Act
        var result = await ReportEndpoints.GetReports(_reportServiceMock, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyList<Report>>>(result.Result);
        Assert.Equal(reports, okResult.Value);
    }

    [Fact]
    public async Task GetReport_ById_ReturnsReport_WhenFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var report = CreateTestReport(reportId);
        _reportServiceMock.GetReportById(reportId, Arg.Any<CancellationToken>()).Returns(report);

        // Act
        var result = await ReportEndpoints.GetReportById(_reportServiceMock, reportId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<Report>>(result.Result);
        Assert.Equal(report, okResult.Value);
        Assert.Equal(reportId, okResult.Value!.Id);
    }

    [Fact]
    public async Task GetReport_ById_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        _reportServiceMock.GetReportById(reportId, Arg.Any<CancellationToken>()).Returns((Report?)null);

        // Act
        var result = await ReportEndpoints.GetReportById(_reportServiceMock, reportId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task CreateReport_ReturnsReport_WhenCreated()
    {
        // Arrange
        var createdReport = CreateTestReport();
        var reportDto = createdReport.ToDto();
        _reportServiceMock.CreateReport(reportDto, Arg.Any<CancellationToken>()).Returns(createdReport);

        // Act
        var result = await ReportEndpoints.CreateReport(_reportServiceMock, reportDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<Created<Report>>(result.Result);
        Assert.Equal($"/api/reports/{createdReport.Id}", createdResult.Location);
        Assert.Equal(createdReport, createdResult.Value);
        Assert.Equal(createdReport.Id, createdResult.Value!.Id);
    }

    [Fact]
    public async Task UpdateReport_ReturnsReport_WhenSuccessful()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var updatedReport = CreateTestReport(reportId);
        var reportDto = updatedReport.ToDto();
        _reportServiceMock.UpdateReport(reportId, reportDto, Arg.Any<CancellationToken>()).Returns(updatedReport);

        // Act
        var result = await ReportEndpoints.UpdateReport(_reportServiceMock, reportId, reportDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<Report>>(result.Result);
        Assert.Equal(updatedReport, okResult.Value);
        Assert.Equal(reportId, okResult.Value!.Id);
    }

    [Fact]
    public async Task UpdateReport_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var updatedReport = CreateTestReport(reportId);
        var reportDto = updatedReport.ToDto();
        _reportServiceMock.UpdateReport(reportId, reportDto, Arg.Any<CancellationToken>()).Returns((Report?)null);

        // Act
        var result = await ReportEndpoints.UpdateReport(_reportServiceMock, reportId, reportDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteReport_ReturnsNoContent_WhenDeleted()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        _reportServiceMock.DeleteReport(reportId, Arg.Any<CancellationToken>()).Returns(returnThis: true);

        // Act
        var result = await ReportEndpoints.DeleteReport(_reportServiceMock, reportId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task DeleteReport_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        _reportServiceMock.DeleteReport(reportId, Arg.Any<CancellationToken>()).Returns(returnThis: false);

        // Act
        var result = await ReportEndpoints.DeleteReport(_reportServiceMock, reportId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }
}

using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace Tests.Endpoints;

public class ReportDuplicateEndpointsTests
{
    private readonly IReportDuplicateService _reportDuplicateServiceMock;

    public ReportDuplicateEndpointsTests()
    {
        _reportDuplicateServiceMock = Substitute.For<IReportDuplicateService>();
    }

    private static ReportDuplicate CreateTestReportDuplicate(Guid? duplicateId = null, Guid? reportId = null)
    {
        return new()
        {
            Id = duplicateId ?? Guid.CreateVersion7(),
            ReportId = reportId ?? Guid.CreateVersion7(),
            EligibilityCheckId = Guid.CreateVersion7(),
            CreatedUtc = DateTimeOffset.UtcNow,
            UpdatedUtc = DateTimeOffset.UtcNow,
            Easting = 123456.78,
            Northing = 987654.32,
        };
    }

    [Fact]
    internal async Task GetDuplicates_ReturnsList_WhenSuccessful()
    {
        // Arrange
        IReadOnlyList<ReportDuplicate> duplicates = [
            CreateTestReportDuplicate(),
            CreateTestReportDuplicate(),
        ];
        _reportDuplicateServiceMock.GetDuplicates(Arg.Any<CancellationToken>()).Returns(duplicates);

        // Act
        var result = await ReportDuplicateEndpoints.GetDuplicates(_reportDuplicateServiceMock, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyList<ReportDuplicate>>>(result.Result);
        Assert.Equal(duplicates, okResult.Value);
    }


    [Fact]
    internal async Task GetReportDuplicates_ReturnsList_WhenSuccessful()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        IReadOnlyList<ReportDuplicate> duplicates = [
            CreateTestReportDuplicate(reportId: reportId),
            CreateTestReportDuplicate(reportId: reportId),
        ];
        _reportDuplicateServiceMock.GetDuplicates(reportId, Arg.Any<CancellationToken>()).Returns(duplicates);

        // Act
        var result = await ReportDuplicateEndpoints.GetReportDuplicates(_reportDuplicateServiceMock, reportId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<IReadOnlyList<ReportDuplicate>>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(duplicates, okResult.Value);
        Assert.All(okResult.Value, duplicate => Assert.Equal(reportId, duplicate.ReportId));
    }

    [Fact]
    public async Task GetDuplicate_ReturnsReportDuplicate_WhenFound()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId);
        _reportDuplicateServiceMock.GetDuplicate(duplicateId, Arg.Any<CancellationToken>()).Returns(duplicate);

        // Act
        var result = await ReportDuplicateEndpoints.GetDuplicate(_reportDuplicateServiceMock, duplicateId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ReportDuplicate>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(duplicate, okResult.Value);
        Assert.Equal(duplicateId, okResult.Value.Id);
    }

    [Fact]
    public async Task GetDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.GetDuplicate(duplicateId, Arg.Any<CancellationToken>()).Returns((ReportDuplicate?)null);

        // Act
        var result = await ReportDuplicateEndpoints.GetDuplicate(_reportDuplicateServiceMock, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task GetReportDuplicate_ReturnsReportDuplicate_WhenFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId, reportId);
        _reportDuplicateServiceMock.GetDuplicate(reportId, duplicateId, Arg.Any<CancellationToken>()).Returns(duplicate);

        // Act
        var result = await ReportDuplicateEndpoints.GetReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ReportDuplicate>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(duplicate, okResult.Value);
        Assert.Equal(duplicateId, okResult.Value.Id);
        Assert.Equal(reportId, okResult.Value.ReportId);
    }

    [Fact]
    public async Task GetReportDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.GetDuplicate(reportId, duplicateId, Arg.Any<CancellationToken>()).Returns((ReportDuplicate?)null);

        // Act
        var result = await ReportDuplicateEndpoints.GetReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task CreateReportDuplicate_ReturnsReportDuplicate_WhenCreated()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(reportId: reportId);
        var duplicateDto = duplicate.ToDto();
        _reportDuplicateServiceMock.CreateDuplicate(reportId, duplicateDto, Arg.Any<CancellationToken>()).Returns(duplicate);

        // Act
        var result = await ReportDuplicateEndpoints.CreateReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateDto, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<Created<ReportDuplicate>>(result.Result);
        Assert.NotNull(createdResult.Value);
        Assert.Equal($"/api/reports/{reportId}/duplicates/{duplicate.Id}", createdResult.Location);
        Assert.Equal(duplicate, createdResult.Value);
        Assert.Equal(reportId, createdResult.Value.ReportId);
    }

    [Fact]
    public async Task UpdateDuplicate_ReturnsReportDuplicate_WhenSuccessful()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId);
        var duplicateDto = duplicate.ToDto();
        _reportDuplicateServiceMock.UpdateDuplicate(duplicateId, duplicateDto, Arg.Any<CancellationToken>()).Returns(duplicate);

        // Act
        var result = await ReportDuplicateEndpoints.UpdateDuplicate(_reportDuplicateServiceMock, duplicateId, duplicateDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ReportDuplicate>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(duplicate, okResult.Value);
        Assert.Equal(duplicateId, okResult.Value.Id);
    }

    [Fact]
    public async Task UpdateDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId);
        var duplicateDto = duplicate.ToDto();
        _reportDuplicateServiceMock.UpdateDuplicate(duplicateId, duplicateDto, Arg.Any<CancellationToken>()).Returns((ReportDuplicate?)null);

        // Act
        var result = await ReportDuplicateEndpoints.UpdateDuplicate(_reportDuplicateServiceMock, duplicateId, duplicateDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task UpdateReportDuplicate_ReturnsReportDuplicate_WhenSuccessful()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId, reportId);
        var duplicateDto = duplicate.ToDto();
        _reportDuplicateServiceMock.UpdateDuplicate(reportId, duplicateId, duplicateDto, Arg.Any<CancellationToken>()).Returns(duplicate);

        // Act
        var result = await ReportDuplicateEndpoints.UpdateReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, duplicateDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ReportDuplicate>>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(duplicate, okResult.Value);
        Assert.Equal(duplicateId, okResult.Value.Id);
        Assert.Equal(reportId, okResult.Value.ReportId);
    }

    [Fact]
    public async Task UpdateReportDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        var duplicate = CreateTestReportDuplicate(duplicateId, reportId);
        var duplicateDto = duplicate.ToDto();
        _reportDuplicateServiceMock.UpdateDuplicate(reportId, duplicateId, duplicateDto, Arg.Any<CancellationToken>()).Returns((ReportDuplicate?)null);

        // Act
        var result = await ReportDuplicateEndpoints.UpdateReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, duplicateDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteDuplicate_ReturnsNoContent_WhenDeleted()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.DeleteDuplicate(duplicateId, Arg.Any<CancellationToken>()).Returns(returnThis: true);

        // Act
        var result = await ReportDuplicateEndpoints.DeleteDuplicate(_reportDuplicateServiceMock, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task DeleteDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.DeleteDuplicate(duplicateId, Arg.Any<CancellationToken>()).Returns(returnThis: false);

        // Act
        var result = await ReportDuplicateEndpoints.DeleteDuplicate(_reportDuplicateServiceMock, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task DeleteReportDuplicate_ReturnsNoContent_WhenDeleted()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.DeleteDuplicate(reportId, duplicateId, Arg.Any<CancellationToken>()).Returns(returnThis: true);

        // Act
        var result = await ReportDuplicateEndpoints.DeleteReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContent>(result.Result);
    }

    [Fact]
    public async Task DeleteReportDuplicate_ReturnsNotFound_WhenNotFound()
    {
        // Arrange
        var reportId = Guid.CreateVersion7();
        var duplicateId = Guid.CreateVersion7();
        _reportDuplicateServiceMock.DeleteDuplicate(reportId, duplicateId, Arg.Any<CancellationToken>()).Returns(returnThis: false);

        // Act
        var result = await ReportDuplicateEndpoints.DeleteReportDuplicate(_reportDuplicateServiceMock, reportId, duplicateId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }
}

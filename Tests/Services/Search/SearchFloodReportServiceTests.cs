using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.Extensions.Logging;
using MockQueryable;
using NSubstitute;
using System.Text.Json;
using Tests.Data;

namespace Tests.Services.Search;

public class SearchFloodReportServiceTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = JsonSerializerOptions.Web;

    [Theory]
    [MemberData(nameof(FloodReportData.Collection_Four), MemberType = typeof(FloodReportData))]
    public async Task GetFloodReports_ReturnsPagedResult_WhenSuccessful(ICollection<FloodReport> expectedData)
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var logger = Substitute.For<ILogger<SearchFloodReportService>>();
        int pageNumber = 1;
        int pageSize = 10;

        var repository = Substitute.For<IFloodReportRepository>();
        var query = expectedData.BuildMock();
        repository.GetFloodReports().Returns(query);

        var service = new SearchFloodReportService(logger, repository);

        // Act
        var result = await service.GetFloodReports(pageNumber, pageSize, cancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(query.ToSearchResult(), _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result.Results, _jsonSerializerOptions);

        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodReports_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var logger = Substitute.For<ILogger<SearchFloodReportService>>();
        int pageNumber = 1;
        int pageSize = 10;

        var repository = Substitute.For<IFloodReportRepository>();
        ICollection<FloodReport> expectedData = [];
        var query = expectedData.BuildMock();
        repository.GetFloodReports().Returns(query);

        var service = new SearchFloodReportService(logger, repository);

        // Act
        var result = await service.GetFloodReports(pageNumber, pageSize, cancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(query.ToSearchResult(), _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result.Results, _jsonSerializerOptions);

        Assert.Equal(expectedJson, resultJson);
    }
}

using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.Extensions.Logging;
using MockQueryable;
using NetTopologySuite.IO.Converters;
using NSubstitute;
using System.Text.Json;
using Tests.Data;

namespace Tests.Services.Search;

public class SearchFloodRoadserviceTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SearchFloodRoadserviceTests()
    {
        _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new GeoJsonConverterFactory() },
        };
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodRoadData.Four), MemberType = typeof(SearchResultFloodRoadData))]
    public async Task GetFloodRoads_ReturnsPagedResult_WhenSuccessful(ICollection<FloodRoad> floodRoads, PagedResult<SearchResultFloodRoad> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodRoadService>>();
        var repository = Substitute.For<IFloodRoadRepository>();
        var query = floodRoads.BuildMock();
        repository.GetFloodRoads().Returns(query);

        var service = new SearchFloodRoadService(logger, repository);

        // Act
        var result = await service.GetFloodRoads(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodRoads_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        ICollection<FloodRoad> floodRoads = [];
        PagedResult<SearchResultFloodRoad> expected = SearchResultFloodRoadData.NotFound;

        var logger = Substitute.For<ILogger<SearchFloodRoadService>>();
        var repository = Substitute.For<IFloodRoadRepository>();
        var query = floodRoads.BuildMock();
        repository.GetFloodRoads().Returns(query);

        var service = new SearchFloodRoadService(logger, repository);

        // Act
        var result = await service.GetFloodRoads(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodRoadData.Four_WithReports), MemberType = typeof(SearchResultFloodRoadData))]
    public async Task GetFloodRoadsWithReports_ReturnsPagedResult_WhenSuccessful(ICollection<FloodRoad> floodRoads, PagedResult<SearchResultFloodRoad> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodRoadService>>();
        var repository = Substitute.For<IFloodRoadRepository>();
        var query = floodRoads.BuildMock();
        repository.GetFloodRoadsWithReports().Returns(query);

        var service = new SearchFloodRoadService(logger, repository);

        // Act
        var result = await service.GetFloodRoadsWithReports(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodRoadsWithReports_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        ICollection<FloodRoad> floodRoads = [];
        PagedResult<SearchResultFloodRoad> expected = SearchResultFloodRoadData.NotFound;

        var logger = Substitute.For<ILogger<SearchFloodRoadService>>();
        var repository = Substitute.For<IFloodRoadRepository>();
        var query = floodRoads.BuildMock();
        repository.GetFloodRoadsWithReports().Returns(query);

        var service = new SearchFloodRoadService(logger, repository);

        // Act
        var result = await service.GetFloodRoadsWithReports(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }
}

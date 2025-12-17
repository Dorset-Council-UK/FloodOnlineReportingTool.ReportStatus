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

public class SearchFloodLocationServiceTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SearchFloodLocationServiceTests()
    {
        _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new GeoJsonConverterFactory() },
        };
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocations_ReturnsPagedResult_WhenSuccessful(ICollection<FloodLocation> floodLocations, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodLocationService>>();
        var repository = Substitute.For<IFloodLocationRepository>();
        var query = floodLocations.BuildMock();
        repository.GetFloodLocations().Returns(query);

        var service = new SearchFloodLocationService(logger, repository);

        // Act
        var result = await service.GetFloodLocations(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodLocations_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        ICollection<FloodLocation> floodLocations = [];
        PagedResult<SearchResultFloodLocation> expected = new(
            Results: floodLocations.ToSearchResults(),
            TotalCount: 0,
            PageSize: 10,
            CurrentPage: 1,
            TotalPages: 0
        );

        var logger = Substitute.For<ILogger<SearchFloodLocationService>>();
        var repository = Substitute.For<IFloodLocationRepository>();
        var query = floodLocations.BuildMock();
        repository.GetFloodLocations().Returns(query);

        var service = new SearchFloodLocationService(logger, repository);

        // Act
        var result = await service.GetFloodLocations(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four_WithReports), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocationsWithReports_ReturnsPagedResult_WhenSuccessful(ICollection<FloodLocation> floodLocations, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodLocationService>>();
        var repository = Substitute.For<IFloodLocationRepository>();
        var query = floodLocations.BuildMock();
        repository.GetFloodLocationsWithReports().Returns(query);

        var service = new SearchFloodLocationService(logger, repository);

        // Act
        var result = await service.GetFloodLocationsWithReports(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodLocationsWithReports_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        ICollection<FloodLocation> floodLocations = [];
        PagedResult<SearchResultFloodLocation> expected = new(
            Results: floodLocations.ToSearchResults(),
            TotalCount: 0,
            PageSize: 10,
            CurrentPage: 1,
            TotalPages: 0
        );

        var logger = Substitute.For<ILogger<SearchFloodLocationService>>();
        var repository = Substitute.For<IFloodLocationRepository>();
        var query = floodLocations.BuildMock();
        repository.GetFloodLocationsWithReports().Returns(query);

        var service = new SearchFloodLocationService(logger, repository);

        // Act
        var result = await service.GetFloodLocationsWithReports(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four_WithPoint), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocationsByPoint_ReturnsPagedResult_WhenSuccessful(double easting, double northing, double buffer, ICollection<FloodLocation> floodLocations, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodLocationService>>();
        var repository = Substitute.For<IFloodLocationRepository>();
        var query = floodLocations.BuildMock();
        repository.GetFloodLocationsByPoint(easting, northing, buffer).Returns(query);

        var service = new SearchFloodLocationService(logger, repository);

        // Act
        var result = await service.GetFloodLocationsByPoint(easting, northing, buffer, 1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }
}

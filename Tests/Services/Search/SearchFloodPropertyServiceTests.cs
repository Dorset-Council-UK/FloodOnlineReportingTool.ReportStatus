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

public class SearchFloodPropertyServiceTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SearchFloodPropertyServiceTests()
    {
        _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new GeoJsonConverterFactory() },
        };
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodPropertyData.Four), MemberType = typeof(SearchResultFloodPropertyData))]
    public async Task GetFloodProperties_ReturnsPagedResult_WhenSuccessful(ICollection<FloodProperty> floodProperties, PagedResult<SearchResultFloodProperty> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodPropertyService>>();
        var repository = Substitute.For<IFloodPropertyRepository>();
        var query = floodProperties.BuildMock();
        repository.GetFloodProperties().Returns(query);

        var service = new SearchFloodPropertyService(logger, repository);

        // Act
        var result = await service.GetFloodProperties(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Fact]
    public async Task GetFloodProperties_ReturnsPagedResult_WhenNotFound()
    {
        // Arrange
        ICollection<FloodProperty> FloodPropertys = [];
        PagedResult<SearchResultFloodProperty> expected = new(
            Results: FloodPropertys.ToSearchResults(),
            TotalCount: 0,
            PageSize: 10,
            CurrentPage: 1,
            TotalPages: 0
        );

        var logger = Substitute.For<ILogger<SearchFloodPropertyService>>();
        var repository = Substitute.For<IFloodPropertyRepository>();
        var query = FloodPropertys.BuildMock();
        repository.GetFloodProperties().Returns(query);

        var service = new SearchFloodPropertyService(logger, repository);

        // Act
        var result = await service.GetFloodProperties(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodPropertyData.Four_WithReports), MemberType = typeof(SearchResultFloodPropertyData))]
    public async Task GetFloodPropertiesWithReports_ReturnsPagedResult_WhenSuccessful(ICollection<FloodProperty> floodProperties, PagedResult<SearchResultFloodProperty> expected)
    {
        // Arrange
        var logger = Substitute.For<ILogger<SearchFloodPropertyService>>();
        var repository = Substitute.For<IFloodPropertyRepository>();
        var query = floodProperties.BuildMock();
        repository.GetFloodPropertiesWithReports().Returns(query);

        var service = new SearchFloodPropertyService(logger, repository);

        // Act
        var result = await service.GetFloodPropertiesWithReports(1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        // Converting to JSON as I had trouble comparing the records directly
        var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
        var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
        Assert.Equal(expectedJson, resultJson);
    }
}

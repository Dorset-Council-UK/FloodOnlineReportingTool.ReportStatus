using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.Database.Models;
using FloodOnlineReportingTool.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.Extensions.Logging;
using MockQueryable;
using NetTopologySuite.IO.Converters;
using NSubstitute;
using System.Text.Json;
using Tests.Data;

namespace Tests.Services.Search;

public class SearchFloodReportServiceTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SearchFloodReportServiceTests()
    {
        _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new GeoJsonConverterFactory() },
        };
    }

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReports_ReturnsPagedResult_WhenSuccessful(ICollection<FloodReportSource> floodReports, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var logger = Substitute.For<ILogger<SearchFloodReportService>>();
    //    var repository = Substitute.For<IFloodReportRepository>();
    //    var query = floodReports.BuildMock();
    //    repository.GetFloodReports().Returns(query);

    //    var service = new SearchFloodReportService(logger, repository);

    //    // Act
    //    var result = await service.GetFloodReports(1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    // Converting to JSON as I had trouble comparing the records directly
    //    var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
    //    var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
    //    Assert.Equal(expectedJson, resultJson);
    //}

    //[Fact]
    //public async Task GetFloodReports_ReturnsPagedResult_WhenNotFound()
    //{
    //    // Arrange
    //    ICollection<FloodReportSource> floodReports = [];
    //    PagedResult<SearchResultFloodReport> expected = new(
    //        Results: floodReports.ToSearchResults(),
    //        TotalCount: 0,
    //        PageSize: 10,
    //        CurrentPage: 1,
    //        TotalPages: 0
    //    );

    //    var logger = Substitute.For<ILogger<SearchFloodReportService>>();
    //    var repository = Substitute.For<IFloodReportRepository>();
    //    var query = floodReports.BuildMock();
    //    repository.GetFloodReports().Returns(query);

    //    var service = new SearchFloodReportService(logger, repository);

    //    // Act
    //    var result = await service.GetFloodReports(1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    // Converting to JSON as I had trouble comparing the records directly
    //    var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
    //    var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
    //    Assert.Equal(expectedJson, resultJson);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_Within50Metres), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsWithinMetres_ReturnsPagedResult_WhenSuccessful(double easting, double northing, double metres, ICollection<FloodReportSource> floodReports, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var logger = Substitute.For<ILogger<SearchFloodReportService>>();
    //    var repository = Substitute.For<IFloodReportRepository>();
    //    var query = floodReports.BuildMock();
    //    repository.GetFloodReportsWithinMetres(easting, northing, metres).Returns(query);

    //    var service = new SearchFloodReportService(logger, repository);

    //    // Act
    //    var result = await service.GetFloodReportsWithinMetres(easting, northing, metres, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    // Converting to JSON as I had trouble comparing the records directly
    //    var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
    //    var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
    //    Assert.Equal(expectedJson, resultJson);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_WithUprn), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsByUprn_ReturnsPagedResult_WhenSuccessful(long uprn, ICollection<FloodReportSource> floodReports, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var logger = Substitute.For<ILogger<SearchFloodReportService>>();
    //    var repository = Substitute.For<IFloodReportRepository>();
    //    var query = floodReports.BuildMock();
    //    repository.GetFloodReportsByUprn(uprn).Returns(query);

    //    var service = new SearchFloodReportService(logger, repository);

    //    // Act
    //    var result = await service.GetFloodReportsByUprn(uprn, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    // Converting to JSON as I had trouble comparing the records directly
    //    var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
    //    var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
    //    Assert.Equal(expectedJson, resultJson);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_WithUsrn), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsByUsrn_ReturnsPagedResult_WhenSuccessful(long usrn, ICollection<FloodReportSource> floodReports, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var logger = Substitute.For<ILogger<SearchFloodReportService>>();
    //    var repository = Substitute.For<IFloodReportRepository>();
    //    var query = floodReports.BuildMock();
    //    repository.GetFloodReportsByUsrn(usrn).Returns(query);

    //    var service = new SearchFloodReportService(logger, repository);

    //    // Act
    //    var result = await service.GetFloodReportsByUsrn(usrn, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    // Converting to JSON as I had trouble comparing the records directly
    //    var expectedJson = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
    //    var resultJson = JsonSerializer.Serialize(result, _jsonSerializerOptions);
    //    Assert.Equal(expectedJson, resultJson);
    //}
}

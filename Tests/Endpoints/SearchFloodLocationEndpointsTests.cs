using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class SearchFloodLocationEndpointsTests
{
    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocations_ReturnsOkay_WhenSuccessful(ICollection<FloodLocation> _, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodLocationService>();
        service.GetFloodLocations(1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodLocationEndpoints.GetFloodLocations(service, 1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodLocation>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four_WithReports), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocationsWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodLocation> _, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodLocationService>();
        service.GetFloodLocationsWithReports(1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodLocationEndpoints.GetFloodLocationsWithReports(service, 1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodLocation>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodLocationData.Four_WithPoint), MemberType = typeof(SearchResultFloodLocationData))]
    public async Task GetFloodLocationsByPoint_ReturnsOkay_WhenSuccessful(double easting, double northing, double buffer, ICollection<FloodLocation> _, PagedResult<SearchResultFloodLocation> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodLocationService>();
        service.GetFloodLocationsByPoint(easting, northing, buffer, 1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodLocationEndpoints.GetFloodLocationsByPoint(service, easting, northing, buffer, 1, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodLocation>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }
}

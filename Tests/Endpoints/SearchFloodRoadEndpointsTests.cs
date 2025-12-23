using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class SearchFloodRoadEndpointsTests
{
    [Theory]
    [MemberData(nameof(SearchResultFloodRoadData.Four), MemberType = typeof(SearchResultFloodRoadData))]
    public async Task GetFloodRoads_ReturnsOkay_WhenSuccessful(ICollection<FloodRoad> _, PagedResult<SearchResultFloodRoad> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodRoadService>();
        service.GetFloodRoads(expected.CurrentPage, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodRoadEndpoints.GetFloodRoads(service, expected.CurrentPage, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodRoad>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodRoadData.Four_WithReports), MemberType = typeof(SearchResultFloodRoadData))]
    public async Task GetFloodRoadsWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodRoad> _, PagedResult<SearchResultFloodRoad> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodRoadService>();
        service.GetFloodRoadsWithReports(expected.CurrentPage, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodRoadEndpoints.GetFloodRoadsWithReports(service, expected.CurrentPage, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodRoad>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }
}

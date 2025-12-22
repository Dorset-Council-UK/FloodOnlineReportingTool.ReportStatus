using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class SearchFloodPropertyEndpointsTests
{
    [Theory]
    [MemberData(nameof(SearchResultFloodPropertyData.Four), MemberType = typeof(SearchResultFloodPropertyData))]
    public async Task GetFloodProperties_ReturnsOkay_WhenSuccessful(ICollection<FloodProperty> _, PagedResult<SearchResultFloodProperty> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodPropertyService>();
        service.GetFloodProperties(expected.CurrentPage, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodPropertyEndpoints.GetFloodProperties(service, expected.CurrentPage, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodProperty>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [MemberData(nameof(SearchResultFloodPropertyData.Four_WithReports), MemberType = typeof(SearchResultFloodPropertyData))]
    public async Task GetFloodPropertiesWithReports_ReturnsOkay_WhenSuccessful(ICollection<FloodProperty> _, PagedResult<SearchResultFloodProperty> expected)
    {
        // Arrange
        var service = Substitute.For<ISearchFloodPropertyService>();
        service.GetFloodPropertiesWithReports(expected.CurrentPage, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await SearchFloodPropertyEndpoints.GetFloodPropertiesWithReports(service, expected.CurrentPage, expected.PageSize, TestContext.Current.CancellationToken);

        // Assert
        var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodProperty>>>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }
}

using FloodOnlineReportingTool.Contracts.Shared.Search;
using FloodOnlineReportingTool.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Endpoints;
using FloodOnlineReportingTool.ReportStatus.Models;
using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Microsoft.AspNetCore.Http.HttpResults;
using NetTopologySuite.Geometries;
using NSubstitute;
using Tests.Data;

namespace Tests.Endpoints;

public class SearchFloodReportEndpointsTests
{
    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReports_ReturnsOkay_WhenSuccessful(ICollection<FloodReportSource> _, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    service.GetFloodReports(1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await SearchFloodReportEndpoints.GetFloodReports(service, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodReport>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Fact]
    //public async Task SearchFloodReports_ReturnsValidationProblem_WhenNoSearchCriteria()
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    var request = new SearchFloodReportRequest();

    //    // Act
    //    var result = await SearchFloodReportEndpoints.SearchFloodReports(service, request, ct: TestContext.Current.CancellationToken);

    //    // Assert
    //    var validationResult = Assert.IsType<ValidationProblem>(result.Result);
    //    Assert.Contains(nameof(request), validationResult.ProblemDetails.Errors.Keys);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_WithUprn), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task SearchFloodReports_ReturnsOkay_WhenSuccessful(long uprn, ICollection<FloodReportSource> _, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    var request = new SearchFloodReportRequest { Uprn = uprn };
    //    service.SearchFloodReports(null, null, Arg.Any<ICollection<Guid>>(), uprn, null, "", null, 1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await SearchFloodReportEndpoints.SearchFloodReports(service, request, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodReport>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_Within50Metres), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsWithinMetres_ReturnsOkay_WhenSuccessful(double easting, double northing, double metres, ICollection<FloodReportSource> _, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    service.GetFloodReportsWithinMetres(easting, northing, metres, 1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await SearchFloodReportEndpoints.GetFloodReportsWithinMetres(service, easting, northing, metres, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodReport>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Theory]
    //[InlineData(-1)]
    //[InlineData(-100)]
    //[InlineData(101)]
    //[InlineData(1234)]
    //public async Task GetFloodReportsWithinMetres_ReturnsValidationProblem_WhenMetresOutsideRange(double metres)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    double easting = 368991.12;
    //    double northing = 90881.94;

    //    // Act
    //    var result = await SearchFloodReportEndpoints.GetFloodReportsWithinMetres(service, easting, northing, metres, ct: TestContext.Current.CancellationToken);

    //    // Assert
    //    var validationResult = Assert.IsType<ValidationProblem>(result.Result);
    //    Assert.Contains(nameof(metres), validationResult.ProblemDetails.Errors.Keys);
    //    await service.DidNotReceive().GetFloodReportsWithinMetres(Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_WithUprn), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsByUprn_ReturnsOkay_WhenSuccessful(long uprn, ICollection<FloodReportSource> _, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    service.GetFloodReportsByUprn(uprn, 1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await SearchFloodReportEndpoints.GetFloodReportsByUprn(service, uprn, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodReport>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}

    //[Theory]
    //[MemberData(nameof(SearchResultFloodReportData.Four_WithUsrn), MemberType = typeof(SearchResultFloodReportData))]
    //public async Task GetFloodReportsByUsrn_ReturnsOkay_WhenSuccessful(long usrn, ICollection<FloodReportSource> _, PagedResult<SearchResultFloodReport> expected)
    //{
    //    // Arrange
    //    var service = Substitute.For<ISearchFloodReportService>();
    //    service.GetFloodReportsByUsrn(usrn, 1, expected.PageSize, Arg.Any<CancellationToken>()).Returns(expected);

    //    // Act
    //    var result = await SearchFloodReportEndpoints.GetFloodReportsByUsrn(service, usrn, 1, expected.PageSize, TestContext.Current.CancellationToken);

    //    // Assert
    //    var okResult = Assert.IsType<Ok<PagedResult<SearchResultFloodReport>>>(result.Result);
    //    Assert.Equal(expected, okResult.Value);
    //}
}

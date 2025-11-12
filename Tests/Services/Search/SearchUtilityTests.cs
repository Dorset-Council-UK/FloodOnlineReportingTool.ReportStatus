using FloodOnlineReportingTool.ReportStatus.Services.Search;
using Tests.Data;
using MockQueryable;

namespace Tests.Services.Search;

public class SearchUtilityTests
{
    [Theory]
    [InlineData(1, 10, 20, 10, 0)]   // Normal case, page 1, pageSize 10
    [InlineData(2, 10, 20, 10, 10)]  // Page 2, pageSize 10
    [InlineData(0, 10, 20, 10, 0)]   // Page number < 1, should skip 0
    [InlineData(-1, 10, 20, 10, 0)]  // Negative page number, should skip 0
    [InlineData(1, 0, 20, 20, 0)]    // pageSize < 1, use defaultPageSize
    [InlineData(3, 0, 15, 15, 30)]   // pageSize < 1, use defaultPageSize, page 3
    [InlineData(1, -5, 25, 25, 0)]   // Negative pageSize, use defaultPageSize
    public void Pagination_ReturnsExpectedTakeAndSkip(int pageNumber, int pageSize, int defaultPageSize, int expectedTake, int expectedSkip)
    {
        var (take, skip) = SearchUtility.Pagination(pageNumber, pageSize, defaultPageSize);

        Assert.Equal(expectedTake, take);
        Assert.Equal(expectedSkip, skip);
    }
    
    [Theory]
    [MemberData(nameof(PagedResultData.PagingScenarios), MemberType = typeof(PagedResultData))]
    public async Task CreatePagedResult_ReturnsExpectedPagedResult(PagedResultTest pagedResultTest)
    {
        // Arrange
        var query = pagedResultTest.Data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pagedResultTest.PageNumber, pagedResultTest.PageSize, pagedResultTest.DefaultPageSize);

        // Assert
        Assert.Equal(pagedResultTest.Expected.Results.Count, pagedResult.Results.Count);
        Assert.Equal(pagedResultTest.Expected.Results, pagedResult.Results);
        Assert.Equal(pagedResultTest.Expected.TotalCount, pagedResult.TotalCount);
        Assert.Equal(pagedResultTest.Expected.PageSize, pagedResult.PageSize);
        Assert.Equal(pagedResultTest.Expected.CurrentPage, pagedResult.CurrentPage);
        Assert.Equal(pagedResultTest.Expected.TotalPages, pagedResult.TotalPages);
    }
}

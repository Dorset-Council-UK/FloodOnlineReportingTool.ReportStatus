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

    [Fact]
    [Trait("Category", "Empty data")]
    public async Task CreatePagedResult_ReturnPageResult_WhenNothingFound()
    {
        // Arrange
        var data = PagedResultData.EmptySimpleItems;
        var pageNumber = 1;
        var pageSize = 4;
        var defaultPageSize = 10;

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Empty(pagedResult.Results);
        Assert.Equal(0, pagedResult.TotalCount);
        Assert.Equal(pageSize, pagedResult.PageSize);
        Assert.Equal(pageNumber, pagedResult.CurrentPage);
        Assert.Equal(0, pagedResult.TotalPages);
    }

    [Theory]
    [Trait("Category", "PageNumber")]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task CreatePagedResult_ReturnsPageResult_WhenFound(int pageNumber)
    {
        // Arrange
        var data = PagedResultData.TenSimpleItems;
        var pageSize = 5;
        var defaultPageSize = 5;
        IReadOnlyCollection<SimpleReferenceType> expectedResults = [.. Enumerable.Range(1, 5).Select(i => new SimpleReferenceType(i))];

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResults.Count, pagedResult.Results.Count);
        Assert.Equal(expectedResults, pagedResult.Results);
        Assert.Equal(data.Count, pagedResult.TotalCount);
        Assert.Equal(pageSize, pagedResult.PageSize);
        Assert.Equal(1, pagedResult.CurrentPage);
        Assert.Equal(2, pagedResult.TotalPages);
    }

    [Theory]
    [Trait("Category", "PageNumber")]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task CreatePagedResult_UsesFirstPage_WhenPageNumberLessThanOne(int pageNumber)
    {
        // Arrange
        var data = PagedResultData.TenSimpleItems;
        var pageSize = 5;
        var defaultPageSize = 5;
        IReadOnlyCollection<SimpleReferenceType> expectedResults = [.. Enumerable.Range(1, 5).Select(i => new SimpleReferenceType(i))];

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResults.Count, pagedResult.Results.Count);
        Assert.Equal(expectedResults, pagedResult.Results);
        Assert.Equal(data.Count, pagedResult.TotalCount);
        Assert.Equal(pageSize, pagedResult.PageSize);
        Assert.Equal(1, pagedResult.CurrentPage);
        Assert.Equal(2, pagedResult.TotalPages);
    }

    [Theory]
    [Trait("Category", "PageNumber")]
    [InlineData(1, 1, 4)]
    [InlineData(2, 5, 4)]
    [InlineData(99, 0, 0)]
    public async Task CreatePagedResult_ReturnsCorrectPage_WhenPageNumberIsGreaterThanZero(int pageNumber, int expectedStart, int expectedCount)
    {
        // Arrange
        var data = PagedResultData.TenSimpleItems;
        var pageSize = 4;
        var defaultPageSize = 10;
        IReadOnlyCollection<SimpleReferenceType> expectedResults = [.. Enumerable.Range(expectedStart, expectedCount).Select(i => new SimpleReferenceType(i))];

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResults.Count, pagedResult.Results.Count);
        Assert.Equal(expectedResults, pagedResult.Results);
        Assert.Equal(data.Count, pagedResult.TotalCount);
        Assert.Equal(pageSize, pagedResult.PageSize);
        Assert.Equal(pageNumber, pagedResult.CurrentPage);
        Assert.Equal(3, pagedResult.TotalPages);
    }

    [Theory]
    [Trait("Category", "PageSize")]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task CreatePagedResult_UsesDefaultPageSize_WhenPageSizeLessThanOne(int pageSize)
    {
        // Arrange
        var data = PagedResultData.TenSimpleItems;
        var pageNumber = 2;
        var defaultPageSize = 6;
        IReadOnlyCollection<SimpleReferenceType> expectedResults = [.. Enumerable.Range(7, 4).Select(i => new SimpleReferenceType(i))];

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResults.Count, pagedResult.Results.Count);
        Assert.Equal(expectedResults, pagedResult.Results);
        Assert.Equal(data.Count, pagedResult.TotalCount);
        Assert.Equal(defaultPageSize, pagedResult.PageSize);
        Assert.Equal(pageNumber, pagedResult.CurrentPage);
        Assert.Equal(2, pagedResult.TotalPages);
    }

    [Theory]
    [Trait("Category", "PageSize")]
    [InlineData(1, 2, 1, 10)]
    [InlineData(2, 3, 2, 5)]
    [InlineData(99, 0, 0, 1)]
    public async Task CreatePagedResult_ReturnsCorrectPage_WhenPageSizeIsGreaterThanZero(int pageSize, int expectedStart, int expectedCount, int expectedTotalPages)
    {
        // Arrange
        var data = PagedResultData.TenSimpleItems;
        var pageNumber = 2;
        var defaultPageSize = 10;
        IReadOnlyCollection<SimpleReferenceType> expectedResults = [.. Enumerable.Range(expectedStart, expectedCount).Select(i => new SimpleReferenceType(i))];

        var query = data.BuildMock();

        // Act
        var pagedResult = await SearchUtility.CreatePagedResult(query, pageNumber, pageSize, defaultPageSize, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedResults.Count, pagedResult.Results.Count);
        Assert.Equal(expectedResults, pagedResult.Results);
        Assert.Equal(data.Count, pagedResult.TotalCount);
        Assert.Equal(pageSize, pagedResult.PageSize);
        Assert.Equal(pageNumber, pagedResult.CurrentPage);
        Assert.Equal(expectedTotalPages, pagedResult.TotalPages);
    }
}

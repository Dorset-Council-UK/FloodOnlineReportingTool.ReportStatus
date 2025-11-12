using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Models.Search;

namespace Tests.Data;

public record SimpleReferenceType(int Id);
public record PagedResultTest(ICollection<SimpleReferenceType> Data, int PageNumber, int PageSize, int DefaultPageSize, PagedResult<SimpleReferenceType> Expected);

public class PagedResultData
{
    public static readonly ICollection<SimpleReferenceType> Empty = [];
    public static readonly ICollection<SimpleReferenceType> OneItem = [new SimpleReferenceType(1)];
    public static readonly ICollection<SimpleReferenceType> FiveItems = [.. Enumerable.Range(1, 5).Select(i => new SimpleReferenceType(i))];
    public static readonly ICollection<SimpleReferenceType> TenItems = [.. Enumerable.Range(1, 10).Select(i => new SimpleReferenceType(i))];
    public static readonly ICollection<SimpleReferenceType> FiftyItems = [.. Enumerable.Range(1, 50).Select(i => new SimpleReferenceType(i))];

    public static IEnumerable<TheoryDataRow<PagedResultTest>> PagingScenarios => [
        // Empty data
        new(new PagedResultTest(
            Data: Empty,
            PageNumber: 1,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new(Results: [], TotalCount: 0, PageSize: 10, CurrentPage: 1, TotalPages: 0)
        )),
        // Single item, page 1
        new(new PagedResultTest(
            Data: OneItem,
            PageNumber: 1,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new(Results: [new SimpleReferenceType(1)], TotalCount: 1, PageSize: 10, CurrentPage: 1, TotalPages: 1)
        )),
        // Fewer items than page size
        new(new PagedResultTest(
            Data: FiveItems,
            PageNumber: 1,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new([.. Enumerable.Range(1, 5).Select(i => new SimpleReferenceType(i))], TotalCount: 5, PageSize: 10, CurrentPage: 1, TotalPages: 1)
        )),
        // Exact page size
        new(new PagedResultTest(
            Data: TenItems,
            PageNumber: 1,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new(Results: [.. Enumerable.Range(1, 10).Select(i => new SimpleReferenceType(i))], TotalCount: 10, PageSize: 10, CurrentPage: 1, TotalPages: 1)
        )),
        // Multiple pages, first page
        new(new PagedResultTest(
            Data: FiftyItems,
            PageNumber: 1,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new(Results: [.. Enumerable.Range(1, 10).Select(i => new SimpleReferenceType(i))], TotalCount: 50, PageSize: 10, CurrentPage: 1, TotalPages: 5)
        )),
        // Multiple pages, middle page
        new(new PagedResultTest(
            Data: FiftyItems,
            PageNumber: 3,
            PageSize: 10,
            DefaultPageSize: 10,
            Expected: new(Results: [.. Enumerable.Range(21, 10).Select(i => new SimpleReferenceType(i))], TotalCount: 50, PageSize: 10, CurrentPage: 3, TotalPages: 5)
        )),
        // Multiple pages, last page (partial)
        new(new PagedResultTest(
            Data: FiftyItems,
            PageNumber: 5,
            PageSize: 12,
            DefaultPageSize: 12,
            Expected: new(Results: [.. Enumerable.Range(49, 2).Select(i => new SimpleReferenceType(i))], TotalCount: 50, PageSize: 12, CurrentPage: 5, TotalPages: 5)
        )),
        // Page number out of range (too high)
        new(new PagedResultTest(
            Data: TenItems,
            PageNumber: 5,
            PageSize: 3,
            DefaultPageSize: 3,
            Expected: new(Results: [], TotalCount: 10, PageSize: 3, CurrentPage: 5, TotalPages: 4)
        )),
        // Page number less than 1
        new(new PagedResultTest(
            Data: TenItems,
            PageNumber: 0,
            PageSize: 5,
            DefaultPageSize: 5,
            Expected: new(Results: [.. Enumerable.Range(1, 5).Select(i => new SimpleReferenceType(i))], TotalCount: 10, PageSize: 5, CurrentPage: 0, TotalPages: 2)
        )),
        // Negative page size, should use default
        new(new PagedResultTest(
            Data: TenItems,
            PageNumber: 1,
            PageSize: -5,
            DefaultPageSize: 4,
            Expected: new(Results: [.. Enumerable.Range(1, 4).Select(i => new SimpleReferenceType(i))], TotalCount: 10, PageSize: 4, CurrentPage: 1, TotalPages: 3)
        )),
        // Page size zero, should use default
        new(new PagedResultTest(
            Data: TenItems,
            PageNumber: 2,
            PageSize: 0,
            DefaultPageSize: 6,
            Expected: new(Results: [.. Enumerable.Range(7, 4).Select(i => new SimpleReferenceType(i))], TotalCount: 10, PageSize: 6, CurrentPage: 2, TotalPages: 2)
        )),
    ];
}

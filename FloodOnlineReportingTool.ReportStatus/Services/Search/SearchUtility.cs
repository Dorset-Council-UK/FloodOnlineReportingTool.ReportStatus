using FloodOnlineReportingTool.Contracts.Shared.Search;
using Microsoft.EntityFrameworkCore;

namespace FloodOnlineReportingTool.ReportStatus.Services.Search;

internal static class SearchUtility
{
    internal static (int take, int skip) Pagination(int pageNumber, int pageSize, int defaultPageSize)
    {
        var take = pageSize < 1 ? defaultPageSize : pageSize;
        return (take, pageNumber < 1 ? 0 : (pageNumber - 1) * take);
    }

    internal static async Task<PagedResult<T>> CreatePagedResult<T>(IQueryable<T> query, int pageNumber, int pageSize, int defaultPageSize, CancellationToken ct = default)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = defaultPageSize;
        }

        var (take, skip) = Pagination(pageNumber, pageSize, defaultPageSize);

        var totalCount = await query.CountAsync(ct);
        var results = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);

        var totalPages = (totalCount + take - 1) / take;

        return new PagedResult<T>(results, totalCount, pageSize, pageNumber, totalPages);
    }
}

using FloodOnlineReportingTool.Contracts.Shared.Search;

namespace Tests.Data;

public static class SearchResultFloodPropertyFloodReportData
{
    public static readonly DateTimeOffset TestCreatedDate = new(2025, 12, 18, 14, 46, 0, TimeSpan.Zero);

    public static SearchResultFloodPropertyFloodReport Create(Guid? floodReportId = null, bool confirmed = false)
    {
        DateTimeOffset? confirmedDate = confirmed ? TestCreatedDate.AddHours(1) : null;
        string? confirmedUserName = confirmed ? "Test Confirmer" : null;

        return new(
            floodReportId ?? Guid.NewGuid(),
            TestCreatedDate,
            UpdatedUtc: null,
            confirmedDate,
            confirmedUserName
        );
    }
}

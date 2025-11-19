namespace Tests.Data;

public static class PagedResultData
{
    public static readonly ICollection<SimpleReferenceType> EmptySimpleItems = [];
    public static readonly ICollection<SimpleReferenceType> TenSimpleItems = [.. Enumerable.Range(1, 10).Select(i => new SimpleReferenceType(i))];
}

namespace MetricBoard.Common.Application.Pagination;

public sealed class PagedResult<TItem>
{
    public required IReadOnlyCollection<TItem> Items { get; init; }

    public required int TotalCount { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public static PagedResult<TItem> Create(IReadOnlyCollection<TItem> items, int totalCount, int page, int pageSize) =>
        new()
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        };
}

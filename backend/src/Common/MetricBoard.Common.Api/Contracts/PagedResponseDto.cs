using MetricBoard.Common.Application.Pagination;

namespace MetricBoard.Common.Api.Contracts;

public sealed class PagedResponseDto<TItem>
{
    public required IReadOnlyCollection<TItem> Items { get; init; }

    public required int TotalCount { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public static PagedResponseDto<TItem> From(PagedResult<TItem> result) => new()
    {
        Items = result.Items,
        TotalCount = result.TotalCount,
        Page = result.Page,
        PageSize = result.PageSize,
    };
}

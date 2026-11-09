using MetricBoard.Common.Application.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MetricBoard.Common.Api.Contracts;

/// <summary>
/// Bind with [AsParameters] on a GET action to pick up page/pageSize/sortBy/sortDirection
/// query params without repeating them on every query DTO.
/// </summary>
public sealed class PageQueryParameters
{
    [FromQuery(Name = "page")]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "pageSize")]
    public int PageSize { get; init; } = 20;

    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; init; }

    [FromQuery(Name = "sortDirection")]
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
}

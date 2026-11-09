using Dictionaries.Application.Dtos;
using Dictionaries.Domain;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Pagination;

namespace Dictionaries.Application.Queries.GetThemes;

/// <summary>
/// Backs the "Справочник тем" table: one optional contains-filter per visible column,
/// a single sortable column, and paging — exactly what DataTable sends as query params.
/// </summary>
public sealed record GetThemesQuery(
    string? IdContains,
    string? NameContains,
    string? AuthorContains,
    string? EditorContains,
    ThemeStatus? Status,
    string? SortBy,
    SortDirection SortDirection,
    int Page,
    int PageSize) : IQuery<PagedResult<ThemeDto>>;

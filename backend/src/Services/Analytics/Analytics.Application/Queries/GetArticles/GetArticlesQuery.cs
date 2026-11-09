using Analytics.Application.Dtos;
using Analytics.Domain;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Pagination;

namespace Analytics.Application.Queries.GetArticles;

/// <summary>Backs the "Статьи" list: free-text search plus the three filters shown in the sidebar.</summary>
public sealed record GetArticlesQuery(
    string? Search,
    Guid? ThemeId,
    Periodicity? Periodicity,
    string? AuthorContains,
    string? SortBy,
    SortDirection SortDirection,
    int Page,
    int PageSize) : IQuery<PagedResult<ArticleListItemDto>>;

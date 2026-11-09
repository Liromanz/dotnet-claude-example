using Analytics.Application.Abstractions;
using Analytics.Application.Dtos;
using Analytics.Domain;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Application.Queries.GetArticles;

public sealed class GetArticlesQueryHandler(
    IArticleReadRepository articleReadRepository,
    IThemeRefReadRepository themeRefReadRepository) : IQueryHandler<GetArticlesQuery, PagedResult<ArticleListItemDto>>
{
    public async Task<PagedResult<ArticleListItemDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var query = articleReadRepository.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(a =>
                EF.Functions.Like(a.Title, $"%{request.Search}%") ||
                EF.Functions.Like(a.Subtitle, $"%{request.Search}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.AuthorContains))
        {
            query = query.Where(a => EF.Functions.Like(a.Author, $"%{request.AuthorContains}%"));
        }

        if (request.Periodicity is not null)
        {
            query = query.Where(a => a.Periodicity == request.Periodicity);
        }

        if (request.ThemeId is { } themeId)
        {
            query = query.Where(a => EF.Functions.Like(EF.Property<string>(a, "ThemeIdsSerialized"), $"%\"{themeId}\"%"));
        }

        query = ApplySort(query, request.SortBy, request.SortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var articles = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var themeIds = articles.SelectMany(a => a.ThemeIds).Distinct().ToList();
        var themesById = await themeRefReadRepository.Query()
            .Where(t => themeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var items = articles
            .Select(a => new ArticleListItemDto
            {
                Id = a.Id,
                Title = a.Title,
                Subtitle = a.Subtitle,
                Author = a.Author,
                Periodicity = a.Periodicity.ToString(),
                ViewCount = a.ViewCount,
                PublishedAtUtc = a.PublishedAtUtc,
                Themes = a.ThemeIds
                    .Where(themesById.ContainsKey)
                    .Select(id => new ThemeTagDto { Id = id, Name = themesById[id].Name })
                    .ToList(),
            })
            .ToList();

        return PagedResult<ArticleListItemDto>.Create(items, totalCount, page, pageSize);
    }

    private static IQueryable<Article> ApplySort(IQueryable<Article> query, string? sortBy, SortDirection direction)
    {
        var descending = direction == SortDirection.Descending;

        return sortBy?.ToLowerInvariant() switch
        {
            "title" => descending ? query.OrderByDescending(a => a.Title) : query.OrderBy(a => a.Title),
            "author" => descending ? query.OrderByDescending(a => a.Author) : query.OrderBy(a => a.Author),
            "viewcount" => descending ? query.OrderByDescending(a => a.ViewCount) : query.OrderBy(a => a.ViewCount),
            "publishedatutc" => descending ? query.OrderByDescending(a => a.PublishedAtUtc) : query.OrderBy(a => a.PublishedAtUtc),
            _ => query.OrderByDescending(a => a.PublishedAtUtc),
        };
    }
}

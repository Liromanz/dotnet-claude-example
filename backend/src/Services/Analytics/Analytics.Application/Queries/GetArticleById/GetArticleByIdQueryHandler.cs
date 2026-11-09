using Analytics.Application.Abstractions;
using Analytics.Application.Dtos;
using MetricBoard.Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Application.Queries.GetArticleById;

public sealed class GetArticleByIdQueryHandler(
    IArticleReadRepository articleReadRepository,
    IThemeRefReadRepository themeRefReadRepository) : IQueryHandler<GetArticleByIdQuery, ArticleDetailDto?>
{
    public async Task<ArticleDetailDto?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await articleReadRepository.Query()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (article is null)
        {
            return null;
        }

        var themesById = await themeRefReadRepository.Query()
            .Where(t => article.ThemeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        return new ArticleDetailDto
        {
            Id = article.Id,
            Title = article.Title,
            Subtitle = article.Subtitle,
            Body = article.Body,
            Author = article.Author,
            Periodicity = article.Periodicity.ToString(),
            ViewCount = article.ViewCount,
            PublishedAtUtc = article.PublishedAtUtc,
            Themes = article.ThemeIds
                .Where(themesById.ContainsKey)
                .Select(id => new ThemeTagDto { Id = id, Name = themesById[id].Name })
                .ToList(),
        };
    }
}

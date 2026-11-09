namespace Analytics.Application.Dtos;

public sealed class ArticleListItemDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Subtitle { get; init; } = string.Empty;

    public string Author { get; init; } = string.Empty;

    public string Periodicity { get; init; } = string.Empty;

    public IReadOnlyList<ThemeTagDto> Themes { get; init; } = [];

    public int ViewCount { get; init; }

    public DateTimeOffset PublishedAtUtc { get; init; }
}

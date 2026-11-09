using System.Text.Json;
using MetricBoard.Common.Domain;

namespace Analytics.Domain;

/// <summary>
/// The thing Analytics actually tracks. ViewCount is the metric this whole app exists to
/// collect — it only ever moves through RecordView, called once per detail-page open.
/// ThemeIds point at Dictionaries' Theme aggregate but are never resolved by calling that
/// service directly; see ThemeRef for the local read-cache fed by integration events.
/// </summary>
public sealed class Article : AggregateRoot
{
    public const int MaxThemes = 3;

    private List<Guid> _themeIds = [];

    private Article(Guid id) : base(id)
    {
    }

#pragma warning disable CS8618 // materialized by EF Core
    private Article()
    {
    }
#pragma warning restore CS8618

    public string Title { get; private set; } = string.Empty;

    public string Subtitle { get; private set; } = string.Empty;

    public string Body { get; private set; } = string.Empty;

    public string Author { get; private set; } = string.Empty;

    public Periodicity Periodicity { get; private set; }

    public IReadOnlyList<Guid> ThemeIds => _themeIds.AsReadOnly();

    /// <summary>
    /// EF Core persistence only — mapped by property name in ArticleConfiguration since the
    /// public surface only ever exposes the read-only ThemeIds. Never touch from domain logic.
    /// </summary>
    private string ThemeIdsSerialized
    {
        get => JsonSerializer.Serialize(_themeIds);
        set => _themeIds = string.IsNullOrWhiteSpace(value) ? [] : JsonSerializer.Deserialize<List<Guid>>(value) ?? [];
    }

    public int ViewCount { get; private set; }

    public DateTimeOffset PublishedAtUtc { get; private set; }

    public static Article Create(
        string title,
        string subtitle,
        string body,
        string author,
        Periodicity periodicity,
        IEnumerable<Guid> themeIds)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Article title is required.");
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new DomainException("Article author is required.");
        }

        var themes = themeIds.Distinct().ToList();
        if (themes.Count > MaxThemes)
        {
            throw new DomainException($"An article can have at most {MaxThemes} themes.");
        }

        var article = new Article(Guid.NewGuid())
        {
            Title = title.Trim(),
            Subtitle = subtitle.Trim(),
            Body = body.Trim(),
            Author = author.Trim(),
            Periodicity = periodicity,
            PublishedAtUtc = DateTimeOffset.UtcNow,
            ViewCount = 0,
        };
        article._themeIds.AddRange(themes);

        return article;
    }

    public void RecordView() => ViewCount++;
}

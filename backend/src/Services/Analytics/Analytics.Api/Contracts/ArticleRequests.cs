using Analytics.Domain;

namespace Analytics.Api.Contracts;

public sealed record CreateArticleRequest(
    string Title,
    string Subtitle,
    string Body,
    string Author,
    Periodicity Periodicity,
    IReadOnlyList<Guid> ThemeIds);

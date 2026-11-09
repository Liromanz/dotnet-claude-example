using Analytics.Domain;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.CreateArticle;

public sealed record CreateArticleCommand(
    string Title,
    string Subtitle,
    string Body,
    string Author,
    Periodicity Periodicity,
    IReadOnlyList<Guid> ThemeIds) : ICommand<Guid>;

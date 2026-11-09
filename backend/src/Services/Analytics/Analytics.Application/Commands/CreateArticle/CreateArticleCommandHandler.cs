using Analytics.Application.Abstractions;
using Analytics.Domain;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.CreateArticle;

public sealed class CreateArticleCommandHandler(IArticleRepository repository)
    : ICommandHandler<CreateArticleCommand, Guid>
{
    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = Article.Create(
            request.Title,
            request.Subtitle,
            request.Body,
            request.Author,
            request.Periodicity,
            request.ThemeIds);

        await repository.AddAsync(article, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return article.Id;
    }
}

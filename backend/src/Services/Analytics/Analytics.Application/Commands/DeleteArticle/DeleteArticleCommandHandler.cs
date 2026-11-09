using Analytics.Application.Abstractions;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.DeleteArticle;

public sealed class DeleteArticleCommandHandler(IArticleRepository repository) : ICommandHandler<DeleteArticleCommand>
{
    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Article '{request.Id}' was not found.");

        repository.Remove(article);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

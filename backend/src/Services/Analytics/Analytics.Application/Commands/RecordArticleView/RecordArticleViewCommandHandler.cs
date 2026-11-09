using Analytics.Application.Abstractions;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.RecordArticleView;

public sealed class RecordArticleViewCommandHandler(IArticleRepository repository) : ICommandHandler<RecordArticleViewCommand>
{
    public async Task Handle(RecordArticleViewCommand request, CancellationToken cancellationToken)
    {
        var article = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Article '{request.Id}' was not found.");

        article.RecordView();

        repository.Update(article);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

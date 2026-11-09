using Dictionaries.Application.Abstractions;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;

namespace Dictionaries.Application.Commands.DeleteTheme;

public sealed class DeleteThemeCommandHandler(
    IThemeRepository repository,
    IIntegrationEventPublisher eventPublisher) : ICommandHandler<DeleteThemeCommand>
{
    public async Task Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
    {
        var theme = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Theme '{request.Id}' was not found.");

        repository.Remove(theme);
        await repository.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(
            ThemeTopics.Themes,
            new ThemeDeletedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, theme.Id),
            cancellationToken);
    }
}

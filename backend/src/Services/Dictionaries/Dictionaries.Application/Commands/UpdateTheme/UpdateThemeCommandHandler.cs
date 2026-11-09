using Dictionaries.Application.Abstractions;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;

namespace Dictionaries.Application.Commands.UpdateTheme;

public sealed class UpdateThemeCommandHandler(
    IThemeRepository repository,
    IIntegrationEventPublisher eventPublisher) : ICommandHandler<UpdateThemeCommand>
{
    public async Task Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
        var theme = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Theme '{request.Id}' was not found.");

        theme.Rename(request.Name, request.Editor);

        repository.Update(theme);
        await repository.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(
            ThemeTopics.Themes,
            new ThemeUpdatedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, theme.Id, theme.Name, theme.Status.ToString()),
            cancellationToken);
    }
}

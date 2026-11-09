using Dictionaries.Application.Abstractions;
using Dictionaries.Domain;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;

namespace Dictionaries.Application.Commands.CreateTheme;

public sealed class CreateThemeCommandHandler(
    IThemeRepository repository,
    IIntegrationEventPublisher eventPublisher) : ICommandHandler<CreateThemeCommand, Guid>
{
    public async Task<Guid> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
    {
        var theme = Theme.Create(request.Name, request.Author);

        await repository.AddAsync(theme, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(
            ThemeTopics.Themes,
            new ThemeCreatedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, theme.Id, theme.Name, theme.Status.ToString()),
            cancellationToken);

        return theme.Id;
    }
}

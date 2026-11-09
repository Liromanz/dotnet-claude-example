using Analytics.Application.Abstractions;
using Analytics.Domain;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;

namespace Analytics.Application.IntegrationEvents;

/// <summary>
/// These four handlers are the entire contract between the services: Analytics never reads
/// Dictionaries' database or calls its API — it only ever reacts to what arrives on the
/// "themes" topic and keeps its local ThemeRef cache eventually consistent.
/// </summary>
public sealed class ThemeCreatedIntegrationEventHandler(IThemeRefRepository repository)
    : IIntegrationEventHandler<ThemeCreatedIntegrationEvent>
{
    public async Task HandleAsync(ThemeCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await repository.UpsertAsync(ThemeRef.Create(@event.ThemeId, @event.Name, @event.Status), cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ThemeUpdatedIntegrationEventHandler(IThemeRefRepository repository)
    : IIntegrationEventHandler<ThemeUpdatedIntegrationEvent>
{
    public async Task HandleAsync(ThemeUpdatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var themeRef = await repository.GetByIdAsync(@event.ThemeId, cancellationToken)
            ?? ThemeRef.Create(@event.ThemeId, @event.Name, @event.Status);

        themeRef.Apply(@event.Name, @event.Status);

        await repository.UpsertAsync(themeRef, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ThemeApprovedIntegrationEventHandler(IThemeRefRepository repository)
    : IIntegrationEventHandler<ThemeApprovedIntegrationEvent>
{
    public async Task HandleAsync(ThemeApprovedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var themeRef = await repository.GetByIdAsync(@event.ThemeId, cancellationToken);
        if (themeRef is null)
        {
            return;
        }

        themeRef.Apply(themeRef.Name, @event.Status);

        await repository.UpsertAsync(themeRef, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ThemeDeletedIntegrationEventHandler(IThemeRefRepository repository)
    : IIntegrationEventHandler<ThemeDeletedIntegrationEvent>
{
    public async Task HandleAsync(ThemeDeletedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(@event.ThemeId, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}

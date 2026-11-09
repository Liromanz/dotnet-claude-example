using Dictionaries.Application.Abstractions;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;

namespace Dictionaries.Application.Commands.ApproveTheme;

public sealed class ApproveThemeCommandHandler(
    IThemeRepository repository,
    IIntegrationEventPublisher eventPublisher) : ICommandHandler<ApproveThemeCommand>
{
    public async Task Handle(ApproveThemeCommand request, CancellationToken cancellationToken)
    {
        var theme = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Theme '{request.Id}' was not found.");

        theme.Approve(request.Editor);

        repository.Update(theme);
        await repository.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(
            ThemeTopics.Themes,
            new ThemeApprovedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, theme.Id, theme.Status.ToString()),
            cancellationToken);
    }
}

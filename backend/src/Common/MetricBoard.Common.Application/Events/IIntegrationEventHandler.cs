using MetricBoard.Common.Domain.Events;

namespace MetricBoard.Common.Application.Events;

/// <summary>
/// Consumer-side handler for one integration event type. A service registers as many of
/// these as it cares about; the dispatcher resolves them by the concrete event type.
/// </summary>
public interface IIntegrationEventHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

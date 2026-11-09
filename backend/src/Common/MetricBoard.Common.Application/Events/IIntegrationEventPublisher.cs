using MetricBoard.Common.Domain.Events;

namespace MetricBoard.Common.Application.Events;

/// <summary>
/// Publishes an integration event onto the shared topic (Broker.Api stands in for Kafka).
/// Application-layer code depends only on this abstraction, never on the HTTP transport.
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(string topic, TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent;
}

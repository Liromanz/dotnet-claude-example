using MetricBoard.Common.Domain.Events;

namespace MetricBoard.Common.Application.Events;

/// <summary>
/// Maps the "eventType" string carried on the wire back to the CLR type a consuming
/// service should deserialize the payload into. Each service registers only the event
/// types it actually subscribes to (the "единый топик" principle: everyone sees every
/// message, only interested services know how to decode it).
/// </summary>
public sealed class IntegrationEventTypeCatalog
{
    private readonly Dictionary<string, Type> _eventTypesByName = new(StringComparer.Ordinal);

    public IntegrationEventTypeCatalog Register<TEvent>(string eventType) where TEvent : IIntegrationEvent
    {
        _eventTypesByName[eventType] = typeof(TEvent);
        return this;
    }

    public bool TryResolve(string eventType, out Type clrType) =>
        _eventTypesByName.TryGetValue(eventType, out clrType!);
}

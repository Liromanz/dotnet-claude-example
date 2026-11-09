namespace MetricBoard.Common.Domain.Events;

/// <summary>
/// Marker for a contract that crosses service boundaries over the shared event topic
/// (simulated Kafka). Implementations are plain records shared via the Common libraries
/// so both the publishing and the consuming service compile against the same shape.
/// </summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }

    DateTimeOffset OccurredAtUtc { get; }
}

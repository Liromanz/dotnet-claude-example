namespace Broker.Api;

/// <summary>
/// One entry in the single shared topic log. Broker.Api stands in for Kafka: every service
/// publishes and polls through this same shape, identified only by topic name.
/// </summary>
public sealed class EventEnvelope
{
    public required long Sequence { get; init; }

    public required string Topic { get; init; }

    public required string EventType { get; init; }

    public required string PayloadJson { get; init; }

    public required DateTimeOffset OccurredAtUtc { get; init; }
}

public sealed record PublishEventRequest(string EventType, string PayloadJson, DateTimeOffset OccurredAtUtc);

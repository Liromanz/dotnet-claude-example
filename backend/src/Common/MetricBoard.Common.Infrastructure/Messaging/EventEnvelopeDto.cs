namespace MetricBoard.Common.Infrastructure.Messaging;

/// <summary>
/// Wire shape used to talk to Broker.Api. Mirrors Broker.Api's own EventEnvelope so the two
/// processes agree on the JSON contract without sharing an assembly.
/// </summary>
public sealed class EventEnvelopeDto
{
    public long Sequence { get; set; }

    public required string Topic { get; set; }

    public required string EventType { get; set; }

    public required string PayloadJson { get; set; }

    public DateTimeOffset OccurredAtUtc { get; set; }
}

public sealed class PublishEventRequest
{
    public required string EventType { get; set; }

    public required string PayloadJson { get; set; }

    public DateTimeOffset OccurredAtUtc { get; set; }
}

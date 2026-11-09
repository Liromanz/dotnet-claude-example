using System.Collections.Concurrent;

namespace Broker.Api;

/// <summary>
/// The "единый топик" itself: an append-only, per-topic log kept in memory for the
/// lifetime of this process. Consumers never get a push — they ask for everything after
/// the sequence number they last saw, which is exactly what a Kafka consumer offset does.
/// </summary>
public sealed class InMemoryTopicStore
{
    private readonly ConcurrentDictionary<string, TopicLog> _topics = new(StringComparer.Ordinal);

    public EventEnvelope Append(string topic, string eventType, string payloadJson, DateTimeOffset occurredAtUtc) =>
        _topics.GetOrAdd(topic, static _ => new TopicLog()).Append(topic, eventType, payloadJson, occurredAtUtc);

    public IReadOnlyList<EventEnvelope> GetAfter(string topic, long after) =>
        _topics.TryGetValue(topic, out var log) ? log.GetAfter(after) : [];

    private sealed class TopicLog
    {
        private readonly object _gate = new();
        private readonly List<EventEnvelope> _events = [];
        private long _sequence;

        public EventEnvelope Append(string topic, string eventType, string payloadJson, DateTimeOffset occurredAtUtc)
        {
            lock (_gate)
            {
                var envelope = new EventEnvelope
                {
                    Sequence = ++_sequence,
                    Topic = topic,
                    EventType = eventType,
                    PayloadJson = payloadJson,
                    OccurredAtUtc = occurredAtUtc,
                };
                _events.Add(envelope);
                return envelope;
            }
        }

        public IReadOnlyList<EventEnvelope> GetAfter(long after)
        {
            lock (_gate)
            {
                return _events.Where(e => e.Sequence > after).ToList();
            }
        }
    }
}

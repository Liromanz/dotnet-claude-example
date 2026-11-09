using System.Net.Http.Json;
using System.Text.Json;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Domain.Events;

namespace MetricBoard.Common.Infrastructure.Messaging;

/// <summary>
/// IIntegrationEventPublisher implementation that POSTs to Broker.Api. From the
/// application layer's point of view this is just "the bus" — it has no idea the
/// transport underneath is a tiny HTTP service standing in for a Kafka topic.
/// </summary>
public sealed class BrokerEventPublisher(IHttpClientFactory httpClientFactory) : IIntegrationEventPublisher
{
    public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task PublishAsync<TEvent>(string topic, TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        var client = httpClientFactory.CreateClient(BrokerHttpClientNames.Broker);

        var request = new PublishEventRequest
        {
            EventType = EventTypeNaming.For<TEvent>(),
            PayloadJson = JsonSerializer.Serialize(@event, SerializerOptions),
            OccurredAtUtc = @event.OccurredAtUtc,
        };

        var response = await client.PostAsJsonAsync($"topics/{topic}/events", request, SerializerOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}

public static class BrokerHttpClientNames
{
    public const string Broker = "Broker";
}

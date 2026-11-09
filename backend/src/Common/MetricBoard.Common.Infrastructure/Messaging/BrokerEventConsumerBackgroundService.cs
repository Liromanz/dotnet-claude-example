using System.Net.Http.Json;
using System.Text.Json;
using MetricBoard.Common.Application.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetricBoard.Common.Infrastructure.Messaging;

/// <summary>
/// Long-polls Broker.Api for one topic/consumer-group pair and dispatches each event to
/// every registered IIntegrationEventHandler&lt;TEvent&gt; for its concrete type. The
/// topic and consumer group are fixed per service (e.g. Analytics always polls "themes"
/// as the "analytics" group) — only services that actually subscribe pay for this.
/// </summary>
public sealed class BrokerEventConsumerBackgroundService(
    string topic,
    string consumerGroup,
    IHttpClientFactory httpClientFactory,
    IntegrationEventTypeCatalog catalog,
    IServiceScopeFactory scopeFactory,
    IOptions<BrokerClientOptions> options,
    ILogger<BrokerEventConsumerBackgroundService> logger) : BackgroundService
{
    private long _cursor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = httpClientFactory.CreateClient(BrokerHttpClientNames.Broker);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var envelopes = await client.GetFromJsonAsync<List<EventEnvelopeDto>>(
                    $"topics/{topic}/events?after={_cursor}&group={consumerGroup}",
                    BrokerEventPublisher.SerializerOptions,
                    stoppingToken);

                if (envelopes is { Count: > 0 })
                {
                    foreach (var envelope in envelopes.OrderBy(e => e.Sequence))
                    {
                        await DispatchAsync(envelope, stoppingToken);
                        _cursor = envelope.Sequence;
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Failed to poll topic {Topic} for group {Group}", topic, consumerGroup);
            }

            try
            {
                await Task.Delay(options.Value.PollingIntervalMs, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // shutting down
            }
        }
    }

    private async Task DispatchAsync(EventEnvelopeDto envelope, CancellationToken cancellationToken)
    {
        if (!catalog.TryResolve(envelope.EventType, out var clrType))
        {
            return;
        }

        var @event = JsonSerializer.Deserialize(envelope.PayloadJson, clrType, BrokerEventPublisher.SerializerOptions);
        if (@event is null)
        {
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(clrType);
        var handleMethod = handlerType.GetMethod("HandleAsync")!;

        foreach (var handler in scope.ServiceProvider.GetServices(handlerType))
        {
            var task = (Task)handleMethod.Invoke(handler, [@event, cancellationToken])!;
            await task;
        }
    }
}

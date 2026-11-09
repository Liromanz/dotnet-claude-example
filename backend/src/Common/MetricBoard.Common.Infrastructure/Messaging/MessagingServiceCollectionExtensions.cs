using MetricBoard.Common.Application.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MetricBoard.Common.Infrastructure.Messaging;

public static class MessagingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Broker.Api-backed publisher plus the HttpClient it rides on. Call once
    /// per service, regardless of whether that service also consumes events.
    /// </summary>
    public static IServiceCollection AddBrokerMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BrokerClientOptions>(configuration.GetSection(BrokerClientOptions.SectionName));

        services.AddHttpClient(BrokerHttpClientNames.Broker, (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<BrokerClientOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
        });

        services.AddSingleton<IIntegrationEventPublisher, BrokerEventPublisher>();

        return services;
    }

    /// <summary>
    /// Registers the catalog a consumer uses to turn a wire "eventType" string back into a
    /// CLR type. Only call this from a service that actually consumes events — a pure
    /// publisher (like Dictionaries) has no use for it.
    /// </summary>
    public static IServiceCollection AddIntegrationEventCatalog(
        this IServiceCollection services,
        Action<IntegrationEventTypeCatalog> configure)
    {
        var catalog = new IntegrationEventTypeCatalog();
        configure(catalog);
        services.AddSingleton(catalog);
        return services;
    }

    /// <summary>
    /// Starts a background poller for one topic/consumer-group pair. Requires
    /// AddIntegrationEventCatalog to have been called first so handlers can be resolved by
    /// concrete event type.
    /// </summary>
    public static IServiceCollection AddIntegrationEventConsumer(
        this IServiceCollection services,
        string topic,
        string consumerGroup)
    {
        services.AddHostedService(serviceProvider => new BrokerEventConsumerBackgroundService(
            topic,
            consumerGroup,
            serviceProvider.GetRequiredService<IHttpClientFactory>(),
            serviceProvider.GetRequiredService<IntegrationEventTypeCatalog>(),
            serviceProvider.GetRequiredService<IServiceScopeFactory>(),
            serviceProvider.GetRequiredService<IOptions<BrokerClientOptions>>(),
            serviceProvider.GetRequiredService<ILogger<BrokerEventConsumerBackgroundService>>()));

        return services;
    }
}

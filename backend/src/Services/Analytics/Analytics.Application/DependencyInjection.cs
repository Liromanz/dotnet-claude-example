using Analytics.Application.IntegrationEvents;
using Mapster;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAnalyticsApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        TypeAdapterConfig.GlobalSettings.Scan(typeof(DependencyInjection).Assembly);

        services.AddScoped<IIntegrationEventHandler<ThemeCreatedIntegrationEvent>, ThemeCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ThemeUpdatedIntegrationEvent>, ThemeUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ThemeApprovedIntegrationEvent>, ThemeApprovedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ThemeDeletedIntegrationEvent>, ThemeDeletedIntegrationEventHandler>();

        return services;
    }
}

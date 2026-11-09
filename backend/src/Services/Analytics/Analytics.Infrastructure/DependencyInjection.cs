using Analytics.Application.Abstractions;
using Analytics.Infrastructure.Persistence;
using Analytics.Infrastructure.Repositories;
using MetricBoard.Common.Application.IntegrationEvents.Themes;
using MetricBoard.Common.Infrastructure.Messaging;
using MetricBoard.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Analytics.Infrastructure;

public static class DependencyInjection
{
    private const string ThemesTopic = ThemeTopics.Themes;
    private const string ConsumerGroup = "analytics";

    public static IServiceCollection AddAnalyticsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Analytics")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Analytics.");

        services.AddDbContext<AnalyticsDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticleReadRepository, ArticleReadRepository>();
        services.AddScoped<IThemeRefRepository, ThemeRefRepository>();
        services.AddScoped<IThemeRefReadRepository, ThemeRefReadRepository>();

        services.AddBrokerMessaging(configuration);

        services.AddIntegrationEventCatalog(catalog => catalog
            .Register<ThemeCreatedIntegrationEvent>("ThemeCreated")
            .Register<ThemeUpdatedIntegrationEvent>("ThemeUpdated")
            .Register<ThemeApprovedIntegrationEvent>("ThemeApproved")
            .Register<ThemeDeletedIntegrationEvent>("ThemeDeleted"));

        services.AddIntegrationEventConsumer(ThemesTopic, ConsumerGroup);

        return services;
    }

    public static async Task EnsureSchemaReadyAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
        await SqliteSchemaGuard.EnsureTablesExistAsync(dbContext, AnalyticsDbContext.ExpectedTables, cancellationToken);
    }
}

using Dictionaries.Application.Abstractions;
using Dictionaries.Infrastructure.Persistence;
using Dictionaries.Infrastructure.Repositories;
using MetricBoard.Common.Infrastructure.Messaging;
using MetricBoard.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionaries.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionariesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Dictionaries")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:Dictionaries.");

        services.AddDbContext<DictionariesDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IThemeRepository, ThemeRepository>();
        services.AddScoped<IThemeReadRepository, ThemeReadRepository>();

        services.AddBrokerMessaging(configuration);

        return services;
    }

    public static async Task EnsureSchemaReadyAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DictionariesDbContext>();
        await SqliteSchemaGuard.EnsureTablesExistAsync(dbContext, DictionariesDbContext.ExpectedTables, cancellationToken);
    }
}

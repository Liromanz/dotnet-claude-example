using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionaries.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDictionariesApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        TypeAdapterConfig.GlobalSettings.Scan(typeof(DependencyInjection).Assembly);

        return services;
    }
}

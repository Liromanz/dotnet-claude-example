using Microsoft.Extensions.DependencyInjection;

namespace MetricBoard.Common.Api;

public static class FrontendCorsServiceCollectionExtensions
{
    public const string FrontendPolicy = "Frontend";

    /// <summary>
    /// The React dev server (Vite, localhost:5173) talks to each API directly in local dev —
    /// there's no gateway in front of these services, so every API opens CORS to it.
    /// </summary>
    public static IServiceCollection AddFrontendCors(this IServiceCollection services, params string[] allowedOrigins) =>
        services.AddCors(options => options.AddPolicy(FrontendPolicy, policy =>
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()));
}

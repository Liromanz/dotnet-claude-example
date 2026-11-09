using System.Text.Json.Serialization;
using Dictionaries.Application;
using Dictionaries.Infrastructure;
using MetricBoard.Common.Api;
using MetricBoard.Common.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddDictionariesApplication();
builder.Services.AddDictionariesInfrastructure(builder.Configuration);

builder.Services.AddFrontendCors(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? []);

var app = builder.Build();

await app.Services.EnsureSchemaReadyAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseProblemDetailsExceptionHandling();
app.UseCors(FrontendCorsServiceCollectionExtensions.FrontendPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();

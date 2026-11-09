using Analytics.Application.Abstractions;
using Analytics.Application.IntegrationEvents;
using Analytics.Domain;
using MetricBoard.Common.Application.IntegrationEvents.Themes;
using NSubstitute;
using Xunit;

namespace Analytics.Application.Tests.IntegrationEvents;

public sealed class ThemeCreatedIntegrationEventHandlerTests
{
    [Fact]
    public async Task HandleAsync_UpsertsLocalThemeRefCache()
    {
        var repository = Substitute.For<IThemeRefRepository>();
        var handler = new ThemeCreatedIntegrationEventHandler(repository);
        var themeId = Guid.NewGuid();

        await handler.HandleAsync(
            new ThemeCreatedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, themeId, "Инфляция", "Draft"),
            CancellationToken.None);

        await repository.Received(1).UpsertAsync(
            Arg.Is<ThemeRef>(t => t.Id == themeId && t.Name == "Инфляция" && t.Status == "Draft"),
            Arg.Any<CancellationToken>());
        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}

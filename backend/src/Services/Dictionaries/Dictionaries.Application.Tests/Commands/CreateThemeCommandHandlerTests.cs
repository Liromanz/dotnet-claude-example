using Dictionaries.Application.Abstractions;
using Dictionaries.Application.Commands.CreateTheme;
using Dictionaries.Domain;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;
using NSubstitute;
using Xunit;

namespace Dictionaries.Application.Tests.Commands;

public sealed class CreateThemeCommandHandlerTests
{
    [Fact]
    public async Task Handle_PersistsThemeAsDraft_AndPublishesCreatedEvent()
    {
        var repository = Substitute.For<IThemeRepository>();
        var eventPublisher = Substitute.For<IIntegrationEventPublisher>();
        var handler = new CreateThemeCommandHandler(repository, eventPublisher);

        var themeId = await handler.Handle(new CreateThemeCommand("Инфляция", "ЦМИ"), CancellationToken.None);

        await repository.Received(1).AddAsync(
            Arg.Is<Theme>(t => t.Id == themeId && t.Name == "Инфляция" && t.Status == ThemeStatus.Draft),
            Arg.Any<CancellationToken>());
        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        await eventPublisher.Received(1).PublishAsync(
            "themes",
            Arg.Any<ThemeCreatedIntegrationEvent>(),
            Arg.Any<CancellationToken>());
    }
}

using Dictionaries.Application.Abstractions;
using Dictionaries.Application.Commands.ApproveTheme;
using Dictionaries.Domain;
using MetricBoard.Common.Application;
using MetricBoard.Common.Application.Events;
using MetricBoard.Common.Application.IntegrationEvents.Themes;
using NSubstitute;
using Xunit;

namespace Dictionaries.Application.Tests.Commands;

public sealed class ApproveThemeCommandHandlerTests
{
    [Fact]
    public async Task Handle_ApprovesDraftTheme_AndPublishesApprovedEvent()
    {
        var theme = Theme.Create("Бюджетная политика", "ЦМИ");
        var repository = Substitute.For<IThemeRepository>();
        repository.GetByIdAsync(theme.Id, Arg.Any<CancellationToken>()).Returns(theme);
        var eventPublisher = Substitute.For<IIntegrationEventPublisher>();
        var handler = new ApproveThemeCommandHandler(repository, eventPublisher);

        await handler.Handle(new ApproveThemeCommand(theme.Id, "Редактор"), CancellationToken.None);

        Assert.Equal(ThemeStatus.Approved, theme.Status);
        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await eventPublisher.Received(1).PublishAsync(
            "themes",
            Arg.Any<ThemeApprovedIntegrationEvent>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ThemeAlreadyApproved_ThrowsDomainException()
    {
        var theme = Theme.Create("Инвестиции", "ЦМИ");
        theme.Approve("Редактор");
        var repository = Substitute.For<IThemeRepository>();
        repository.GetByIdAsync(theme.Id, Arg.Any<CancellationToken>()).Returns(theme);
        var handler = new ApproveThemeCommandHandler(repository, Substitute.For<IIntegrationEventPublisher>());

        await Assert.ThrowsAsync<MetricBoard.Common.Domain.DomainException>(() =>
            handler.Handle(new ApproveThemeCommand(theme.Id, "Редактор 2"), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UnknownTheme_ThrowsNotFoundException()
    {
        var repository = Substitute.For<IThemeRepository>();
        repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Theme?)null);
        var handler = new ApproveThemeCommandHandler(repository, Substitute.For<IIntegrationEventPublisher>());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new ApproveThemeCommand(Guid.NewGuid(), "Редактор"), CancellationToken.None));
    }
}

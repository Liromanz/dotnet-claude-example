using Analytics.Application.Abstractions;
using Analytics.Application.Commands.CreateArticle;
using Analytics.Domain;
using MetricBoard.Common.Domain;
using NSubstitute;
using Xunit;

namespace Analytics.Application.Tests.Commands;

public sealed class CreateArticleCommandHandlerTests
{
    [Fact]
    public async Task Handle_PersistsArticle_WithGivenThemes()
    {
        var repository = Substitute.For<IArticleRepository>();
        var handler = new CreateArticleCommandHandler(repository);
        var themeIds = new[] { Guid.NewGuid(), Guid.NewGuid() };

        var articleId = await handler.Handle(
            new CreateArticleCommand("Заголовок", "Подзаголовок", "Текст", "ЦМИ", Periodicity.Weekly, themeIds),
            CancellationToken.None);

        await repository.Received(1).AddAsync(
            Arg.Is<Article>(a => a.Id == articleId && a.Title == "Заголовок" && a.ThemeIds.SequenceEqual(themeIds)),
            Arg.Any<CancellationToken>());
        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MoreThanMaxThemes_ThrowsDomainException()
    {
        var repository = Substitute.For<IArticleRepository>();
        var handler = new CreateArticleCommandHandler(repository);
        var themeIds = Enumerable.Range(0, Article.MaxThemes + 1).Select(_ => Guid.NewGuid()).ToArray();

        await Assert.ThrowsAsync<DomainException>(() =>
            handler.Handle(
                new CreateArticleCommand("Заголовок", "Подзаголовок", "Текст", "ЦМИ", Periodicity.Weekly, themeIds),
                CancellationToken.None));
    }
}

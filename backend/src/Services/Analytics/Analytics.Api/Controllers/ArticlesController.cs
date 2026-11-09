using Analytics.Api.Contracts;
using Analytics.Application.Commands.CreateArticle;
using Analytics.Application.Commands.DeleteArticle;
using Analytics.Application.Commands.RecordArticleView;
using Analytics.Application.Dtos;
using Analytics.Application.Queries.GetArticleById;
using Analytics.Application.Queries.GetArticles;
using MediatR;
using MetricBoard.Common.Api;
using MetricBoard.Common.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.Api.Controllers;

[Route("api/articles")]
public sealed class ArticlesController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ArticleListItemDto>>> GetArticles(
        [FromQuery] GetArticlesRequest filter,
        [AsParameters] PageQueryParameters paging,
        CancellationToken cancellationToken)
    {
        var query = new GetArticlesQuery(
            filter.Search,
            filter.ThemeId,
            filter.Periodicity,
            filter.Author,
            paging.SortBy,
            paging.SortDirection,
            paging.Page,
            paging.PageSize);

        var result = await Mediator.Send(query, cancellationToken);
        return Ok(PagedResponseDto<ArticleListItemDto>.From(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        // Command first (records the metric), then the query — the client never computes
        // the view count itself, it just asks for the article again afterwards.
        await Mediator.Send(new RecordArticleViewCommand(id), cancellationToken);
        var article = await Mediator.Send(new GetArticleByIdQuery(id), cancellationToken);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateArticleCommand(
            request.Title,
            request.Subtitle,
            request.Body,
            request.Author,
            request.Periodicity,
            request.ThemeIds);

        var id = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteArticleCommand(id), cancellationToken);
        return NoContent();
    }
}

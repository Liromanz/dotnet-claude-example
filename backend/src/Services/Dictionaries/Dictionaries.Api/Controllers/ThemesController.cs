using Dictionaries.Api.Contracts;
using Dictionaries.Application.Commands.ApproveTheme;
using Dictionaries.Application.Commands.CreateTheme;
using Dictionaries.Application.Commands.DeleteTheme;
using Dictionaries.Application.Commands.UpdateTheme;
using Dictionaries.Application.Queries.GetThemeById;
using Dictionaries.Application.Queries.GetThemes;
using Mapster;
using MediatR;
using MetricBoard.Common.Api;
using MetricBoard.Common.Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Dictionaries.Api.Controllers;

[Route("api/themes")]
public sealed class ThemesController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<Dictionaries.Application.Dtos.ThemeDto>>> GetThemes(
        [FromQuery] GetThemesRequest filter,
        [AsParameters] PageQueryParameters paging,
        CancellationToken cancellationToken)
    {
        var query = new GetThemesQuery(
            filter.Id,
            filter.Name,
            filter.Author,
            filter.Editor,
            filter.Status,
            paging.SortBy,
            paging.SortDirection,
            paging.Page,
            paging.PageSize);

        var result = await Mediator.Send(query, cancellationToken);
        return Ok(PagedResponseDto<Dictionaries.Application.Dtos.ThemeDto>.From(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var theme = await Mediator.Send(new GetThemeByIdQuery(id), cancellationToken);
        return theme is null ? NotFound() : Ok(theme);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateThemeRequest request, CancellationToken cancellationToken)
    {
        var id = await Mediator.Send(request.Adapt<CreateThemeCommand>(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateThemeRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateThemeCommand(id, request.Name, request.Editor), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveThemeRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ApproveThemeCommand(id, request.Editor), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteThemeCommand(id), cancellationToken);
        return NoContent();
    }
}

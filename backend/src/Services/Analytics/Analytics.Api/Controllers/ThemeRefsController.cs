using Analytics.Application.Dtos;
using Analytics.Application.Queries.GetThemeRefs;
using MediatR;
using MetricBoard.Common.Api;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.Api.Controllers;

[Route("api/theme-refs")]
public sealed class ThemeRefsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ThemeRefDto>>> GetThemeRefs(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetThemeRefsQuery(search), cancellationToken);
        return Ok(result);
    }
}

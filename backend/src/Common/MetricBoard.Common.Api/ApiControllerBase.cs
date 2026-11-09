using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MetricBoard.Common.Api;

/// <summary>
/// Every controller in every service inherits from this: it only ever talks to MediatR,
/// never to a repository or DbContext directly — those live behind command/query handlers.
/// </summary>
[ApiController]
public abstract class ApiControllerBase(ISender mediator) : ControllerBase
{
    protected ISender Mediator { get; } = mediator;
}

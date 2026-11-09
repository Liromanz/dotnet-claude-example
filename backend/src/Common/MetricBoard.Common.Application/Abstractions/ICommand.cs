using MediatR;

namespace MetricBoard.Common.Application.Abstractions;

/// <summary>
/// A write operation dispatched through MediatR. Handlers call into a write repository.
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// A write operation that returns a value (e.g. the id of the created aggregate).
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

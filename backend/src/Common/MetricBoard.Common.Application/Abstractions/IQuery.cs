using MediatR;

namespace MetricBoard.Common.Application.Abstractions;

/// <summary>
/// A read operation dispatched through MediatR. Handlers call into a readonly repository.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}

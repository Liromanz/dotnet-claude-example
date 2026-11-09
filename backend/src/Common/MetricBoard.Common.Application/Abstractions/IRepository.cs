using MetricBoard.Common.Domain;

namespace MetricBoard.Common.Application.Abstractions;

/// <summary>
/// Write-side repository used by command handlers. One per aggregate root.
/// </summary>
public interface IRepository<TAggregate> where TAggregate : AggregateRoot
{
    Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    void Update(TAggregate aggregate);

    void Remove(TAggregate aggregate);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

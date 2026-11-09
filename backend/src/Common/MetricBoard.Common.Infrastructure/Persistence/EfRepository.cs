using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace MetricBoard.Common.Infrastructure.Persistence;

/// <summary>
/// Default write-repository implementation shared by every service: tracked CRUD against
/// one DbSet. Service-specific repositories inherit from this and only add the handful of
/// query shapes the generic surface can't express (e.g. "find by name").
/// </summary>
public class EfRepository<TAggregate>(DbContext dbContext) : IRepository<TAggregate>
    where TAggregate : AggregateRoot
{
    protected DbContext DbContext { get; } = dbContext;

    public virtual async Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TAggregate>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public virtual async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TAggregate>().AddAsync(aggregate, cancellationToken);

    public virtual void Update(TAggregate aggregate) => DbContext.Set<TAggregate>().Update(aggregate);

    public virtual void Remove(TAggregate aggregate) => DbContext.Set<TAggregate>().Remove(aggregate);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        DbContext.SaveChangesAsync(cancellationToken);
}

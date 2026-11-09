using MetricBoard.Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MetricBoard.Common.Infrastructure.Persistence;

/// <summary>
/// Default read-repository implementation: an AsNoTracking IQueryable that query handlers
/// filter/sort/page against. Never shares a unit of work with the write side.
/// </summary>
public class EfReadRepository<TEntity>(DbContext dbContext) : IReadRepository<TEntity>
    where TEntity : class
{
    public IQueryable<TEntity> Query() => dbContext.Set<TEntity>().AsNoTracking();
}

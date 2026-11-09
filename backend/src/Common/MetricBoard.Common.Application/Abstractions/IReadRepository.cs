namespace MetricBoard.Common.Application.Abstractions;

/// <summary>
/// Read-side repository used by query handlers. Backed by a no-tracking EF Core query,
/// so query handlers can compose filtering/sorting/paging without touching the write model.
/// </summary>
public interface IReadRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query();
}

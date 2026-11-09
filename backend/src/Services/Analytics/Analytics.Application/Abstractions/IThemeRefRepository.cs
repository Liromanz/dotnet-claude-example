using Analytics.Domain;

namespace Analytics.Application.Abstractions;

/// <summary>
/// Write side of the local theme read-cache. Deliberately not IRepository&lt;T&gt;
/// (ThemeRef isn't an aggregate root of this service) — just upsert/delete driven by
/// integration events.
/// </summary>
public interface IThemeRefRepository
{
    Task<ThemeRef?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpsertAsync(ThemeRef themeRef, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

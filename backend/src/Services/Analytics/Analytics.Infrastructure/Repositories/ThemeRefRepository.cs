using Analytics.Application.Abstractions;
using Analytics.Domain;
using Analytics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Infrastructure.Repositories;

public sealed class ThemeRefRepository(AnalyticsDbContext dbContext) : IThemeRefRepository
{
    public Task<ThemeRef?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.ThemeRefs.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task UpsertAsync(ThemeRef themeRef, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.ThemeRefs.FirstOrDefaultAsync(t => t.Id == themeRef.Id, cancellationToken);
        if (existing is null)
        {
            await dbContext.ThemeRefs.AddAsync(themeRef, cancellationToken);
        }
        else if (!ReferenceEquals(existing, themeRef))
        {
            dbContext.Entry(existing).CurrentValues.SetValues(themeRef);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.ThemeRefs.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (existing is not null)
        {
            dbContext.ThemeRefs.Remove(existing);
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}

using Analytics.Domain;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Infrastructure.Persistence;

public sealed class AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles => Set<Article>();

    public DbSet<ThemeRef> ThemeRefs => Set<ThemeRef>();

    public static readonly IReadOnlyCollection<string> ExpectedTables = ["articles", "theme_refs"];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnalyticsDbContext).Assembly);
    }
}

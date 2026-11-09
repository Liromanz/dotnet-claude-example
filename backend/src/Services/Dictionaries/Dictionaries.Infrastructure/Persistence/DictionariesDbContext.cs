using Dictionaries.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dictionaries.Infrastructure.Persistence;

public sealed class DictionariesDbContext(DbContextOptions<DictionariesDbContext> options) : DbContext(options)
{
    public DbSet<Theme> Themes => Set<Theme>();

    public static readonly IReadOnlyCollection<string> ExpectedTables = ["themes"];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DictionariesDbContext).Assembly);
    }
}

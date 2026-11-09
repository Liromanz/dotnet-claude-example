using Dictionaries.Domain;
using MetricBoard.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionaries.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps onto the `themes` table created by Liquibase (see
/// backend/db/dictionaries/changelog) — column names here must match the changelog exactly,
/// since EF Core never creates or alters this schema itself.
/// </summary>
public sealed class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.ToTable("themes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Status).HasColumnName("status").HasConversion<string>().IsRequired();
        builder.Property(t => t.Author).HasColumnName("author").IsRequired();
        builder.Property(t => t.Editor).HasColumnName("editor");
        builder.Property(t => t.CreatedAtUtc).HasColumnName("created_at_utc").HasConversion<SortableDateTimeOffsetConverter>().IsRequired();
        builder.Property(t => t.UpdatedAtUtc).HasColumnName("updated_at_utc").HasConversion<SortableDateTimeOffsetConverter>().IsRequired();
    }
}

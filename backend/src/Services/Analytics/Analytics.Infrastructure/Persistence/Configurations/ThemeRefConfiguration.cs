using Analytics.Domain;
using MetricBoard.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Analytics.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps onto the `theme_refs` table from Liquibase — the local read-cache populated solely
/// by consuming the "themes" topic (see ThemeIntegrationEventHandlers).
/// </summary>
public sealed class ThemeRefConfiguration : IEntityTypeConfiguration<ThemeRef>
{
    public void Configure(EntityTypeBuilder<ThemeRef> builder)
    {
        builder.ToTable("theme_refs");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.Name).HasColumnName("name").IsRequired();
        builder.Property(t => t.Status).HasColumnName("status").IsRequired();
        builder.Property(t => t.UpdatedAtUtc).HasColumnName("updated_at_utc").HasConversion<SortableDateTimeOffsetConverter>().IsRequired();
    }
}

using Analytics.Domain;
using MetricBoard.Common.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Analytics.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps onto the `articles` table from Liquibase (see backend/db/analytics/changelog).
/// ThemeIdsSerialized is a private property on Article (JSON-encoded list of theme ids) —
/// configuring it by name is the standard EF Core way to map a non-public member.
/// </summary>
public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Title).HasColumnName("title").IsRequired();
        builder.Property(a => a.Subtitle).HasColumnName("subtitle");
        builder.Property(a => a.Body).HasColumnName("body");
        builder.Property(a => a.Author).HasColumnName("author").IsRequired();
        builder.Property(a => a.Periodicity).HasColumnName("periodicity").HasConversion<string>().IsRequired();
        builder.Property(a => a.ViewCount).HasColumnName("view_count");
        builder.Property(a => a.PublishedAtUtc).HasColumnName("published_at_utc").HasConversion<SortableDateTimeOffsetConverter>().IsRequired();

        builder.Property("ThemeIdsSerialized")
            .HasColumnName("theme_ids_json")
            .IsRequired();

        builder.Ignore(a => a.ThemeIds);
    }
}

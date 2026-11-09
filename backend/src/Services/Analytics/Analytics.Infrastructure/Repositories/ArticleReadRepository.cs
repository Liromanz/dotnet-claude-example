using Analytics.Application.Abstractions;
using Analytics.Domain;
using Analytics.Infrastructure.Persistence;
using MetricBoard.Common.Infrastructure.Persistence;

namespace Analytics.Infrastructure.Repositories;

public sealed class ArticleReadRepository(AnalyticsDbContext dbContext) : EfReadRepository<Article>(dbContext), IArticleReadRepository
{
}

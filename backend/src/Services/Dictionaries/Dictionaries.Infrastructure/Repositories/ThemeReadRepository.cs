using Dictionaries.Application.Abstractions;
using Dictionaries.Domain;
using Dictionaries.Infrastructure.Persistence;
using MetricBoard.Common.Infrastructure.Persistence;

namespace Dictionaries.Infrastructure.Repositories;

public sealed class ThemeReadRepository(DictionariesDbContext dbContext) : EfReadRepository<Theme>(dbContext), IThemeReadRepository
{
}

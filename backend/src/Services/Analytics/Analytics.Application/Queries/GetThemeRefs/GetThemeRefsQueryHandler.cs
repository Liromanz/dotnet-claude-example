using Analytics.Application.Abstractions;
using Analytics.Application.Dtos;
using Mapster;
using MetricBoard.Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Application.Queries.GetThemeRefs;

public sealed class GetThemeRefsQueryHandler(IThemeRefReadRepository readRepository)
    : IQueryHandler<GetThemeRefsQuery, IReadOnlyList<ThemeRefDto>>
{
    public async Task<IReadOnlyList<ThemeRefDto>> Handle(GetThemeRefsQuery request, CancellationToken cancellationToken)
    {
        var query = readRepository.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(t => EF.Functions.Like(t.Name, $"%{request.Search}%"));
        }

        return await query
            .OrderBy(t => t.Name)
            .ProjectToType<ThemeRefDto>()
            .ToListAsync(cancellationToken);
    }
}

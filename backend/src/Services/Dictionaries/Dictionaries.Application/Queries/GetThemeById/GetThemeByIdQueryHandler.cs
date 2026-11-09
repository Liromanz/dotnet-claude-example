using Dictionaries.Application.Abstractions;
using Dictionaries.Application.Dtos;
using Mapster;
using MetricBoard.Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Dictionaries.Application.Queries.GetThemeById;

public sealed class GetThemeByIdQueryHandler(IThemeReadRepository readRepository)
    : IQueryHandler<GetThemeByIdQuery, ThemeDto?>
{
    public async Task<ThemeDto?> Handle(GetThemeByIdQuery request, CancellationToken cancellationToken) =>
        await readRepository.Query()
            .Where(t => t.Id == request.Id)
            .ProjectToType<ThemeDto>()
            .FirstOrDefaultAsync(cancellationToken);
}

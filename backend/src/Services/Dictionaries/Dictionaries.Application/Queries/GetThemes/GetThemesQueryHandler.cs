using Dictionaries.Application.Abstractions;
using Dictionaries.Application.Dtos;
using Dictionaries.Domain;
using Mapster;
using MetricBoard.Common.Application.Abstractions;
using MetricBoard.Common.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dictionaries.Application.Queries.GetThemes;

public sealed class GetThemesQueryHandler(IThemeReadRepository readRepository)
    : IQueryHandler<GetThemesQuery, PagedResult<ThemeDto>>
{
    public async Task<PagedResult<ThemeDto>> Handle(GetThemesQuery request, CancellationToken cancellationToken)
    {
        var query = readRepository.Query();

        if (!string.IsNullOrWhiteSpace(request.IdContains))
        {
            query = query.Where(t => EF.Functions.Like(t.Id.ToString(), $"%{request.IdContains}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.NameContains))
        {
            query = query.Where(t => EF.Functions.Like(t.Name, $"%{request.NameContains}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.AuthorContains))
        {
            query = query.Where(t => EF.Functions.Like(t.Author, $"%{request.AuthorContains}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.EditorContains))
        {
            query = query.Where(t => t.Editor != null && EF.Functions.Like(t.Editor, $"%{request.EditorContains}%"));
        }

        if (request.Status is not null)
        {
            query = query.Where(t => t.Status == request.Status);
        }

        query = ApplySort(query, request.SortBy, request.SortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectToType<ThemeDto>()
            .ToListAsync(cancellationToken);

        return PagedResult<ThemeDto>.Create(items, totalCount, page, pageSize);
    }

    private static IQueryable<Theme> ApplySort(IQueryable<Theme> query, string? sortBy, SortDirection direction)
    {
        var descending = direction == SortDirection.Descending;

        return sortBy?.ToLowerInvariant() switch
        {
            "name" => descending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
            "author" => descending ? query.OrderByDescending(t => t.Author) : query.OrderBy(t => t.Author),
            "editor" => descending ? query.OrderByDescending(t => t.Editor) : query.OrderBy(t => t.Editor),
            "status" => descending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            "updatedatutc" => descending ? query.OrderByDescending(t => t.UpdatedAtUtc) : query.OrderBy(t => t.UpdatedAtUtc),
            "createdatutc" or _ => descending ? query.OrderByDescending(t => t.CreatedAtUtc) : query.OrderBy(t => t.CreatedAtUtc),
        };
    }
}

using Analytics.Application.Dtos;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Queries.GetThemeRefs;

/// <summary>Feeds the theme multi-select in the "Новая статья" modal.</summary>
public sealed record GetThemeRefsQuery(string? Search) : IQuery<IReadOnlyList<ThemeRefDto>>;

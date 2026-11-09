using Dictionaries.Application.Dtos;
using MetricBoard.Common.Application.Abstractions;

namespace Dictionaries.Application.Queries.GetThemeById;

public sealed record GetThemeByIdQuery(Guid Id) : IQuery<ThemeDto?>;

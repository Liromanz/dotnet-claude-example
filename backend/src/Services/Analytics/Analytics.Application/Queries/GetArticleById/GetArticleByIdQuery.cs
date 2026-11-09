using Analytics.Application.Dtos;
using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Queries.GetArticleById;

public sealed record GetArticleByIdQuery(Guid Id) : IQuery<ArticleDetailDto?>;

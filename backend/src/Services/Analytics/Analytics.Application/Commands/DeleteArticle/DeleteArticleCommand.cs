using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.DeleteArticle;

public sealed record DeleteArticleCommand(Guid Id) : ICommand;

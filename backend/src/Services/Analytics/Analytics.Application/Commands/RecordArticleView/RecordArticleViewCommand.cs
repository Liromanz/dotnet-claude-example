using MetricBoard.Common.Application.Abstractions;

namespace Analytics.Application.Commands.RecordArticleView;

/// <summary>The metric this whole app collects: one more view on one article.</summary>
public sealed record RecordArticleViewCommand(Guid Id) : ICommand;

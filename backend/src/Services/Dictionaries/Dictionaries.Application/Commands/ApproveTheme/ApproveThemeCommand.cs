using MetricBoard.Common.Application.Abstractions;

namespace Dictionaries.Application.Commands.ApproveTheme;

public sealed record ApproveThemeCommand(Guid Id, string Editor) : ICommand;

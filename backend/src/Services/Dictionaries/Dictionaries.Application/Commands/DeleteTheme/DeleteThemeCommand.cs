using MetricBoard.Common.Application.Abstractions;

namespace Dictionaries.Application.Commands.DeleteTheme;

public sealed record DeleteThemeCommand(Guid Id) : ICommand;

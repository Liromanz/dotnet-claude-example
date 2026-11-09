using MetricBoard.Common.Application.Abstractions;

namespace Dictionaries.Application.Commands.UpdateTheme;

public sealed record UpdateThemeCommand(Guid Id, string Name, string Editor) : ICommand;

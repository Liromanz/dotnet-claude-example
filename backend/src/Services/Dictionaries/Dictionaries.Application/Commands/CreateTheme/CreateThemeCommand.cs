using MetricBoard.Common.Application.Abstractions;

namespace Dictionaries.Application.Commands.CreateTheme;

public sealed record CreateThemeCommand(string Name, string Author) : ICommand<Guid>;

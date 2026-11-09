namespace Dictionaries.Application.Dtos;

public sealed class ThemeDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string Author { get; init; } = string.Empty;

    public string? Editor { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}

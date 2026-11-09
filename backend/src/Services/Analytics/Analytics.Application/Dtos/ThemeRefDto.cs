namespace Analytics.Application.Dtos;

public sealed class ThemeRefDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;
}

namespace Dictionaries.Api.Contracts;

public sealed record CreateThemeRequest(string Name, string Author);

public sealed record UpdateThemeRequest(string Name, string Editor);

public sealed record ApproveThemeRequest(string Editor);

using Dictionaries.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Dictionaries.Api.Contracts;

public sealed class GetThemesRequest
{
    [FromQuery(Name = "id")]
    public string? Id { get; init; }

    [FromQuery(Name = "name")]
    public string? Name { get; init; }

    [FromQuery(Name = "author")]
    public string? Author { get; init; }

    [FromQuery(Name = "editor")]
    public string? Editor { get; init; }

    [FromQuery(Name = "status")]
    public ThemeStatus? Status { get; init; }
}

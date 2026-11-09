using Analytics.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.Api.Contracts;

public sealed class GetArticlesRequest
{
    [FromQuery(Name = "search")]
    public string? Search { get; init; }

    [FromQuery(Name = "themeId")]
    public Guid? ThemeId { get; init; }

    [FromQuery(Name = "periodicity")]
    public Periodicity? Periodicity { get; init; }

    [FromQuery(Name = "author")]
    public string? Author { get; init; }
}

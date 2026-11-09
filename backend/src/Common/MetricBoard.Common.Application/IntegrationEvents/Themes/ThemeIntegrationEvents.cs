using MetricBoard.Common.Domain.Events;

namespace MetricBoard.Common.Application.IntegrationEvents.Themes;

/// <summary>
/// Shared wire contracts for the "themes" topic. Dictionaries.Application publishes these,
/// Analytics.Application consumes them to keep its local ThemeRef read-cache in sync.
/// Status is carried as a plain string so Analytics never needs to reference
/// Dictionaries.Domain's ThemeStatus enum.
/// </summary>
public static class ThemeTopics
{
    public const string Themes = "themes";
}

public sealed record ThemeCreatedIntegrationEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid ThemeId,
    string Name,
    string Status) : IIntegrationEvent;

public sealed record ThemeUpdatedIntegrationEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid ThemeId,
    string Name,
    string Status) : IIntegrationEvent;

public sealed record ThemeApprovedIntegrationEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid ThemeId,
    string Status) : IIntegrationEvent;

public sealed record ThemeDeletedIntegrationEvent(
    Guid EventId,
    DateTimeOffset OccurredAtUtc,
    Guid ThemeId) : IIntegrationEvent;

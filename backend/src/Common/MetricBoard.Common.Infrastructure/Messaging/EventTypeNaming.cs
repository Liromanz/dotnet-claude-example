namespace MetricBoard.Common.Infrastructure.Messaging;

/// <summary>
/// "ThemeCreatedIntegrationEvent" on the wire becomes "ThemeCreated" — short, stable,
/// and independent of the CLR namespace so publisher and consumer only need to agree on
/// this one convention plus the Common.Application contract type.
/// </summary>
public static class EventTypeNaming
{
    private const string Suffix = "IntegrationEvent";

    public static string For<TEvent>() => For(typeof(TEvent));

    public static string For(Type eventType)
    {
        var name = eventType.Name;
        return name.EndsWith(Suffix, StringComparison.Ordinal)
            ? name[..^Suffix.Length]
            : name;
    }
}

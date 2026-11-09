using MetricBoard.Common.Domain;

namespace Analytics.Domain;

/// <summary>
/// Analytics never queries Dictionaries' database or API for theme names — it keeps this
/// local, disposable read-cache instead, kept in sync purely by consuming
/// ThemeCreated/Updated/Approved/Deleted off the shared topic. Losing this table only means
/// a replay of the topic is needed; it is not a source of truth for anything.
/// </summary>
public sealed class ThemeRef : Entity
{
    private ThemeRef(Guid id, string name, string status) : base(id)
    {
        Name = name;
        Status = status;
    }

#pragma warning disable CS8618 // materialized by EF Core
    private ThemeRef()
    {
    }
#pragma warning restore CS8618

    public string Name { get; private set; } = string.Empty;

    public string Status { get; private set; } = string.Empty;

    public DateTimeOffset UpdatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    public static ThemeRef Create(Guid id, string name, string status) => new(id, name, status);

    public void Apply(string name, string status)
    {
        Name = name;
        Status = status;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}

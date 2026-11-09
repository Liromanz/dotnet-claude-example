using MetricBoard.Common.Domain;

namespace Dictionaries.Domain;

/// <summary>
/// The one dictionary this service fully owns, matching the "Справочник тем" screen:
/// a draft/approved workflow with author/editor tracking. EF Core needs the parameterless
/// constructor; everything else goes through the named factory/behaviour methods so the
/// aggregate can enforce its own rules.
/// </summary>
public sealed class Theme : AggregateRoot
{
    private Theme(Guid id) : base(id)
    {
    }

#pragma warning disable CS8618 // materialized by EF Core
    private Theme()
    {
    }
#pragma warning restore CS8618

    public string Name { get; private set; } = string.Empty;

    public ThemeStatus Status { get; private set; }

    public string Author { get; private set; } = string.Empty;

    public string? Editor { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static Theme Create(string name, string author)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Theme name is required.");
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new DomainException("Theme author is required.");
        }

        var now = DateTimeOffset.UtcNow;
        return new Theme(Guid.NewGuid())
        {
            Name = name.Trim(),
            Author = author.Trim(),
            Status = ThemeStatus.Draft,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        };
    }

    public void Rename(string name, string editor)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Theme name is required.");
        }

        Name = name.Trim();
        Editor = editor.Trim();
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Approve(string editor)
    {
        if (Status == ThemeStatus.Approved)
        {
            throw new DomainException($"Theme '{Name}' is already approved.");
        }

        Status = ThemeStatus.Approved;
        Editor = editor.Trim();
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}

namespace MetricBoard.Common.Domain;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>For EF Core materialization only — never call from application code.</summary>
    protected Entity()
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other || other.GetType() != GetType())
        {
            return false;
        }

        return Id == other.Id;
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}

namespace MetricBoard.Common.Domain;

public abstract class AggregateRoot : Entity
{
    private readonly List<object> _domainEvents = [];

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    /// <summary>For EF Core materialization only — never call from application code.</summary>
    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(object domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

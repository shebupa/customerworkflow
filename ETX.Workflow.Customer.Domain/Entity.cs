namespace ETX.Workflow.Customer.Domain;

[ExcludeFromCodeCoverage]
public abstract class Entity<TId>
{
    protected void Apply(object @event)
    {
        EnsureValidState();
        When(@event);
    }

    protected abstract void When(object @event);

    protected abstract void EnsureValidState();
}
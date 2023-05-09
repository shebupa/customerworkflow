namespace ETX.Workflow.Customer.Domain;

public class Workflow : Entity<Workflow>
{
    private readonly Dictionary<WorkflowEventType, Action<Events.WorkflowEvents>> _workflowEventTypes =
            new Dictionary<WorkflowEventType, Action<Events.WorkflowEvents>>();

    [JsonIgnore]
    private Events.WorkflowEvents _workflowEvent = default;

    public Registration Registration { get; set; }
    public Deposit Deposit { get; set; }
    public Trade Trade { get; set; }

    public Workflow(
       Events.WorkflowEvents workflowEvent)
    {
        PopulateWorkFlowEvents();
        _workflowEvent = workflowEvent;
        Apply(_workflowEvent);
    }

    private void PopulateWorkFlowEvents()
    {
        //Against rule of OCP, this is only place, similar to registration in the startup.cs
        _workflowEventTypes.Add(WorkflowEventType.Demo, (workflowEvent)
            => FormRegistrationStatus(workflowEvent));
        _workflowEventTypes.Add(WorkflowEventType.Join, (workflowEvent)
            => FormRegistrationStatus(workflowEvent));
        _workflowEventTypes.Add(WorkflowEventType.Completed, (workflowEvent)
            => FormRegistrationStatus(workflowEvent));
        _workflowEventTypes.Add(WorkflowEventType.Authorised, (workflowEvent)
            => FormRegistrationStatus(workflowEvent));
        _workflowEventTypes.Add(WorkflowEventType.FirstDeposit, (workflowEvent)
            => FormDeposit(workflowEvent));
        _workflowEventTypes.Add(WorkflowEventType.FirstTrade, (workflowEvent)
        => FormTrade(workflowEvent));
    }

    private void FormRegistrationStatus(Events.WorkflowEvents workflowEvent)
    {
        var registrationStatus = new WorkflowRegistrationStatus(workflowEvent);
        Registration = registrationStatus.Registration;
    }

    private void FormDeposit(Events.WorkflowEvents workflowEvent)
    {
        var firstDeposit = new WorkflowFirstDeposit(workflowEvent);
        Deposit = firstDeposit.Deposit;
    }

    private void FormTrade(Events.WorkflowEvents workflowEvent)
    {
        var firstTrade = new WorkflowFirstTrade(workflowEvent);
        Trade = firstTrade.Trade;
    }

    protected override void When(object @event)
    {
        var workflowEvent = (Events.WorkflowEvents)@event;
        _workflowEventTypes[workflowEvent.EventTypeId](workflowEvent);
    }

    protected override void EnsureValidState()
    {
        var valid = ((_workflowEvent.CustomerWorkflowId != null)
                   && (_workflowEvent.CustomerId != null)
                   && (_workflowEvent.EventDetails != null));

        if (!valid)
            throw new DomainExceptions.InvalidEntityState(
                this, $"Post-checks failed in state {valid}"
            );
    }
}
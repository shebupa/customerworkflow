namespace ETX.Workflow.Customer.Domain;

public class WorkflowFirstDeposit
{
    private Events.WorkflowEvents _workFlowEvents { get; set; }
    public Deposit Deposit { get; set; } = new Deposit();

    public WorkflowFirstDeposit(Events.WorkflowEvents workflowEvents)
    {
        _workFlowEvents = workflowEvents;
        FormFirstDeposit();
    }

    private Deposit FormFirstDeposit()
    {
        var firstDeposit = new FirstDeposit();
        var eventDetails = (JObject)JsonConvert.DeserializeObject(_workFlowEvents.EventDetails);
        var values = eventDetails.Values();
        var depositAmount = values.Select(x => x.Value<string>("DepositAmount"))?.Single();
        var depositCurrency = values.Select(x => x.Value<string>("DepositCurrency"))?.Single();

        firstDeposit = new FirstDeposit
        {
            Amount = Double.Parse(depositAmount),
            Currency = depositCurrency,
            EventDateTime = _workFlowEvents.EventDateTime
        };

        Deposit.CustomerWorkflowId = _workFlowEvents.CustomerWorkflowId;
        Deposit.CustomerId = _workFlowEvents.CustomerId;
        Deposit.FirstDeposit = firstDeposit;

        return Deposit;
    }
}
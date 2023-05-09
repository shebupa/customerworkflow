namespace ETX.Workflow.Customer.Domain;

public class WorkflowRegistrationStatus
{
    private Events.WorkflowEvents _workFlowEvents { get; set; }
    public Registration Registration { get; set; } = new Registration();

    public WorkflowRegistrationStatus(Events.WorkflowEvents workflowEvents)
    {
        _workFlowEvents = workflowEvents;
        FormRegistrationStatus();
    }

    private Registration FormRegistrationStatus()
    {
        var registrationStatus = new RegistrationStatus();

        var eventDetails = (JObject)JsonConvert.DeserializeObject(_workFlowEvents.EventDetails);
        var values = eventDetails.Values();
        var statusId = _workFlowEvents.EventTypeId;
        var statusDescription = Enumerations
                                .GetEnumDescription((WorkflowEventType)_workFlowEvents.EventTypeId);

        registrationStatus = new RegistrationStatus
        {
            StatusId = statusId,
            StatusDescription = statusDescription,
            EventDateTime = _workFlowEvents.EventDateTime
        };

        Registration.CustomerWorkflowId = _workFlowEvents.CustomerWorkflowId;
        Registration.CustomerId = _workFlowEvents.CustomerId;
        Registration.RegistrationStatus = registrationStatus;

        return Registration;
    }
}
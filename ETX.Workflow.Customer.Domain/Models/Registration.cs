namespace ETX.Workflow.Customer.Domain.Models;

public class Registration
{
    public string CustomerWorkflowId { get; set; }
    public string CustomerId { get; set; }
    public RegistrationStatus RegistrationStatus { get; set; }
}

public class RegistrationStatus
{
    public WorkflowEventType StatusId { get; set; }
    public string StatusDescription { get; set; }
    public string EventDateTime { get; set; }
}
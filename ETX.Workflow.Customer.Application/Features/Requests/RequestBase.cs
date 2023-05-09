namespace ETX.Workflow.Customer.Application.Features.Requests;

public class RequestBase
{
    public string CustomerWorkflowId { get; set; }
    public string CustomerId { get; set; }
    public WorkflowEventType EventTypeId { get; set; }
    public string EventDateTime { get; set; }
    public string EventDetails { get; set; }
}
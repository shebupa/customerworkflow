namespace ETX.Workflow.Customer.Application.Features;

public class CreateWorkflowRequest
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public int EventTypeId { get; set; }
    public string EventDateTime { get; set; }
    public string EventDetails { get; set; }
}
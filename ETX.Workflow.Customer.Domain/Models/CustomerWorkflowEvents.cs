namespace ETX.Workflow.Customer.Domain.Models;

[ExcludeFromCodeCoverage]
public class CustomerWorkflowEvents
{
    public string Id { get; set; }
    public string CustomerId { get; set; }
    public int EventTypeId { get; set; }
    public DateTime EventDateTime { get; set; }
    public string EventDetails { get; set; }
    public string Status { get; set; }
}
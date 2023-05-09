namespace ETX.Workflow.Customer.Application.Features.Responses;

public class ResponseBase
{
    public string CustomerWorkflowId { get; set; }
    public string CustomerId { get; set; }
    public DateTime EventDateTime { get; set; }
}
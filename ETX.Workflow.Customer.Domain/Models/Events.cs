namespace ETX.Workflow.Customer.Domain.Models;

public static class Events
{
    public class WorkflowEvents
    {
        public string CustomerWorkflowId { get; set; }
        public WorkflowEventType EventTypeId { get; set; }
        public string CustomerId { get; set; }
        public string EventDateTime { get; set; }
        public string EventDetails { get; set; }
    }
}
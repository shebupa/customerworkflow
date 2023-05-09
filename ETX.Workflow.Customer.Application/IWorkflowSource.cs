namespace ETX.Workflow.Customer.Application;

public interface IWorkflowSource
{
    Task<List<CustomerWorkflowEvents>> GetWorkflowEventsAsync();
}
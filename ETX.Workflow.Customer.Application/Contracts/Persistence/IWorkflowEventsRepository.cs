namespace ETX.Workflow.Customer.Application.Contracts.Persistence;

public interface IWorkflowEventsRepository
{
    Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync();

    Task<int> UpdateWorkflowEventsProcessingStatusAsync(QueryCondition queryCondition);
}
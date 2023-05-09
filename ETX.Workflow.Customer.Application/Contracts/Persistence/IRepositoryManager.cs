namespace ETX.Workflow.Customer.Application.Contracts.Persistence;

public interface IRepositoryManager
{
    IWorkflowEventsRepository WorkflowEventsRepository { get; }
}
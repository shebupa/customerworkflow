namespace ETX.Workflow.Customer.Application;

public interface IRequestHandler
{
    public Task HandleCommandsAsync(WorkflowEventSource customerWorkflowEventSource);
}
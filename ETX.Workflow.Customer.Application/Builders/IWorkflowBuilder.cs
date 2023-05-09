namespace ETX.Workflow.Customer.Application.Builders;

public interface IWorkflowBuilder
{
    object BuildCommand(CreateWorkflowRequest request);
}
namespace ETX.Workflow.Customer.Application.Factories;

public interface IWorkflowDbSourceFactory
{
    void Register(
       DataSource dataSource
       , IWorkflowSource _customerWorkflowSource);

    IWorkflowSource Create(
        DataSource dataSource);
}
namespace ETX.Workflow.Customer.Application.Factories;

public class WorkflowDbSourceFactory : IWorkflowDbSourceFactory
{
    private readonly Dictionary<DataSource, IWorkflowSource> _customerWorkflowDbSource
        = new Dictionary<DataSource, IWorkflowSource>();

    public void Register(
        DataSource dataSource
        , IWorkflowSource _customerWorkflowSource)
    {
        _customerWorkflowDbSource.Add(dataSource, _customerWorkflowSource);
    }

    public IWorkflowSource Create(
        DataSource dataSource)
    {
        return _customerWorkflowDbSource[dataSource];
    }
}
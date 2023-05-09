namespace ETX.Workflow.Customer.Persistence.Repositories;

public class RepositoryManagerItdb : IRepositoryManager
{
    private readonly DbContext _customerWorkflowItdbContext = default;
    private readonly IWorkflowEventsRepository _customerWorkflowEventsRepository = default;

    public RepositoryManagerItdb(
           DbContext customerWorkflowItdbContext
           , IWorkflowEventsRepository customerWorkflowEventsRepository
           )
    {
        _customerWorkflowItdbContext = customerWorkflowItdbContext;
        _customerWorkflowEventsRepository = customerWorkflowEventsRepository;
    }

    public IWorkflowEventsRepository WorkflowEventsRepository
    {
        get
        {
            return _customerWorkflowEventsRepository;
        }
    }
}
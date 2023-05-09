namespace ETX.Workflow.Customer.Persistence.Repositories;

public class RepositoryManagerTitan : IRepositoryManager
{
    private readonly DbContext _customerWorkflowTitanContext = default;
    private readonly IWorkflowEventsRepository _customerWorkflowEventsRepository = default;

    public RepositoryManagerTitan(
           DbContext customerWorkflowTitanContext
           , IWorkflowEventsRepository customerWorkflowEventsRepository
           )
    {
        _customerWorkflowTitanContext = customerWorkflowTitanContext;
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
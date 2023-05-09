namespace ETX.Workflow.Customer.Application;

public class WorkflowItdbDbSource :
    WorkflowWithProcessingStatus
    , IWorkflowSource
{
    private readonly IRepositoryManagerFactory _repositoryManagerFactory = default;

    public WorkflowItdbDbSource(
          IRepositoryManagerFactory repositoryManagerFactory) : base(repositoryManagerFactory)
    {
        _repositoryManagerFactory = repositoryManagerFactory;
    }

    public async Task<List<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        var respositoryManager = _repositoryManagerFactory.Create(DataSource.Itdb);
        var customerWorkflowEventsRepository = respositoryManager.WorkflowEventsRepository;
        var customerWorkflowEvents = await customerWorkflowEventsRepository.GetWorkflowEventsAsync();

        var countOfUpdates = await customerWorkflowEventsRepository
                                   .UpdateWorkflowEventsProcessingStatusAsync(
                                    BuildQueryCondition(QueryConditionConstants.INPROGRESS,
                                    customerWorkflowEvents.Select(id => id.Id).ToList()));
        return customerWorkflowEvents.ToList();
    }

    public override async Task<int> UpdateWorkflowEventsAsync(
       string status
       , List<ResponseBase> responseList)
    {
        var respositoryManager = _repositoryManagerFactory.Create(DataSource.Itdb);
        var customerWorkflowEventsRepository = respositoryManager.WorkflowEventsRepository;
        var countOfUpdates = await customerWorkflowEventsRepository
                                   .UpdateWorkflowEventsProcessingStatusAsync(
                                    BuildQueryCondition(status,
                                    responseList.Select(id => ((ResponseBase)id).CustomerWorkflowId).ToList()));
        return countOfUpdates;
    }
}
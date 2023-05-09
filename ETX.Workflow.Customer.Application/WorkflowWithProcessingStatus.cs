namespace ETX.Workflow.Customer.Application;

public abstract class WorkflowWithProcessingStatus
{
    private readonly IRepositoryManagerFactory _repositoryManagerFactory = default;

    public WorkflowWithProcessingStatus(IRepositoryManagerFactory repositoryManagerFactory)
    {
        _repositoryManagerFactory = repositoryManagerFactory;
    }

    public abstract Task<int> UpdateWorkflowEventsAsync(
       string status
       , List<ResponseBase> responseList);

    protected QueryCondition BuildQueryCondition(
         string status
         , List<string> whereIns)
    {
        var queryCondition = new QueryCondition();
        var set = new List<string>();
        set.Add(status);
        queryCondition.ConditionTagsForWhereIns.Add(
                QueryConditionConstants.WHEREINS
                , whereIns);
        queryCondition.ConditionTagsForWhereIns.Add(
            QueryConditionConstants.STATUS
            , set);
        return queryCondition;
    }
}
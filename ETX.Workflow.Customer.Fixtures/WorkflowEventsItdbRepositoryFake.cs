namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class WorkflowEventsItdbRepositoryFake :
    RepositoryBase<DbContext, CustomerWorkflowEvents>
    , IWorkflowEventsRepository
{
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;

    public WorkflowEventsItdbRepositoryFake(
        DbContext context
        , DataSourceItdbSettings dataSourceItdbSettings) : base(context)
    {
        _dataSourceItdbSettings = dataSourceItdbSettings;
    }

    public async Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        var ret = await FindByConditionAsync(
                        QueryConstants.QUERY_SELECT_CUSTOMERWORKFLOWEVENTS
                        , default);
        return ret;
    }

    public async Task<int> UpdateWorkflowEventsProcessingStatusAsync(QueryCondition queryCondition)
    {
        var ret = await UpdateByConditionAsync(QueryConstants.QUERY_UPDATE_WITH_WHERE
                                               , queryCondition
                                               , null);
        return ret;
    }
}
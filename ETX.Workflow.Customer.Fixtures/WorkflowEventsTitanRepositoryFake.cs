namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class WorkflowEventsTitanRepositoryFake :
    RepositoryBase<DbContext, CustomerWorkflowEvents>
    , IWorkflowEventsRepository
{
    private readonly DataSourceTitanSettings _dataSourceTianSettings = default;

    public WorkflowEventsTitanRepositoryFake(
        DbContext context
        , DataSourceTitanSettings dataSourceTianSettings) : base(context)
    {
        _dataSourceTianSettings = dataSourceTianSettings;
    }

    public async Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        string sqlQuery = @"SELECT
                            *
                           FROM CUSTOMERWORKFLOWEVENTS";
        var ret = await FindByConditionAsync(sqlQuery, null);
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
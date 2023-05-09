namespace ETX.Workflow.Customer.Persistence.Repositories;

public class WorkflowEventsTitanRepository :
    RepositoryBase<DbContext, CustomerWorkflowEvents>
    , IWorkflowEventsRepository
{
    private readonly DataSourceTitanSettings _dataSourceTitanSettings = default;

    public WorkflowEventsTitanRepository(
        DbContext context
        , DataSourceTitanSettings dataSourceTitanSettings) : base(context)
    {
        _dataSourceTitanSettings = dataSourceTitanSettings;
    }

    public async Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        string sqlQuery = @"SELECT
                             CONVERT(NVARCHAR(50), ID) AS ID
                            ,CUSTOMERID
                            ,EVENTTYPEID
                            ,EVENTDATETIME
                            ,EVENTDETAILS
                            ,STATUS
					        FROM CUSTOMERWORKFLOWEVENTS
                            WHERE EVENTTYPEID <> 0 AND STATUS IS NULL
								   ORDER BY EVENTDATETIME
							OFFSET 0 ROWS FETCH FIRST" + $" { _dataSourceTitanSettings.TitanLimitRows} " + "ROWS ONLY ";
        var ret = await FindByConditionAsync(sqlQuery, null);
        return ret;
    }

    public async Task<int> UpdateWorkflowEventsProcessingStatusAsync(QueryCondition queryCondition)
    {
        string sqlQuery = @"UPDATE CUSTOMERWORKFLOWEVENTS SET STATUS =@STATUS
                            WHERE ID IN @" + $"{QueryConditionConstants.WHEREINS}";

        var ret = await UpdateByConditionAsync(sqlQuery, queryCondition);
        return ret;
    }
}
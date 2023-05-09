namespace ETX.Workflow.Customer.Persistence.Repositories;

public class WorkflowEventsItdbRepository :
    RepositoryBase<DbContext, CustomerWorkflowEvents>
    , IWorkflowEventsRepository
{
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;

    public WorkflowEventsItdbRepository(
        DbContext context
        , DataSourceItdbSettings dataSourceItdbSettings) : base(context)

    {
        _dataSourceItdbSettings = dataSourceItdbSettings;
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
                            AND EVENTDATETIME <= dateadd(MINUTE," + $" { _dataSourceItdbSettings.DelayInMinutes} " + ", getUTCdate())" +
								   "ORDER BY EVENTDATETIME " +
                                   "OFFSET 0 ROWS FETCH FIRST" + $" { _dataSourceItdbSettings.ItdbLimitRows} " + "ROWS ONLY ";
        var ret = await FindByConditionAsync(sqlQuery, null);
        return ret;
    }

    public async Task<int> UpdateWorkflowEventsProcessingStatusAsync(
        QueryCondition queryCondition)
    {
        string sqlQuery = @"UPDATE CUSTOMERWORKFLOWEVENTS SET STATUS =@STATUS
                            WHERE ID IN @" + $"{QueryConditionConstants.WHEREINS}";

        var ret = await UpdateByConditionAsync(sqlQuery, queryCondition);
        return ret;
    }
}
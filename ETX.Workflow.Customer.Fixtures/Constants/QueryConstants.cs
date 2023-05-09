namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class QueryConstants
{
    public const string QUERY_UPDATE_STATUS_INPROGRESS =
        $"UPDATE CUSTOMERWORKFLOWEVENTS SET STATUS = 'InProgress' WHERE ID IN @{QueryConditionConstants.WHEREINS}";

    public const string QUERY_UPDATE_STATUS_COMPLETED =
       $"UPDATE CUSTOMERWORKFLOWEVENTS SET STATUS = 'Completed' WHERE ID IN @{QueryConditionConstants.WHEREINS}";

    public const string QUERY_UPDATE_WITH_WHERE =
       @"UPDATE CUSTOMERWORKFLOWEVENTS SET STATUS =@STATUS
                            WHERE ID IN @" + $"{QueryConditionConstants.WHEREINS}";

    public const string QUERY_SELECT_CUSTOMERWORKFLOWEVENTS =
       $"SELECT * FROM CUSTOMERWORKFLOWEVENTS where EVENTDATETIME < DATETIME('2022-03-16 11:45:05.137', '-10 minute')";
}
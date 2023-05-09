using ETX.Workflow.Customer.Application.Features.Requests;

namespace ETX.Workflow.Customer.Fixtures.Configurations;

[ExcludeFromCodeCoverage]
public class Util
{
    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }

    public static QueryCondition BuildQueryCondition(
       string id)
    {
        var queryCondition = new QueryCondition();
        var whereIns = new List<string>();
        whereIns.Add(id);
        queryCondition.ConditionTagsForWhereIns.Add(
                QueryConditionConstants.WHEREINS
                , whereIns);
        return queryCondition;
    }

    public static QueryCondition BuildQueryCondition(
      string status
      , string id)
    {
        var queryCondition = new QueryCondition();
        var whereIns = new List<string>();
        whereIns.Add(id);
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

    public static Events.WorkflowEvents CreateWorkflowEvent<T>(
        T commadRequest) where T : RequestBase
    {
        return new Events.WorkflowEvents
        {
            CustomerWorkflowId = commadRequest.CustomerWorkflowId,
            CustomerId = commadRequest.CustomerId,
            EventTypeId = commadRequest.EventTypeId,
            EventDetails = commadRequest.EventDetails
        };
    }
}
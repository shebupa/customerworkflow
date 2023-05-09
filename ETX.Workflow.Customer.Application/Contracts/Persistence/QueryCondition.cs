namespace ETX.Workflow.Customer.Application.Contracts;

[ExcludeFromCodeCoverage]
public class QueryCondition
{
    public Dictionary<string, string> ConditionTags { get; set; }
        = new Dictionary<string, string>();

    public Dictionary<string, List<string>> ConditionTagsForWhereIns { get; set; }
        = new Dictionary<string, List<string>>();
}
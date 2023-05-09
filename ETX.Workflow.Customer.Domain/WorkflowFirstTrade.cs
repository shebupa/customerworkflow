namespace ETX.Workflow.Customer.Domain;

public class WorkflowFirstTrade
{
    private Events.WorkflowEvents _workFlowEvents { get; set; }
    public Trade Trade { get; set; } = new Trade();

    public WorkflowFirstTrade(Events.WorkflowEvents workflowEvents)
    {
        _workFlowEvents = workflowEvents;
        FormFirstTrade();
    }

    private Trade FormFirstTrade()
    {
        FirstTrade firstTrade = default;
        var eventDetails = (JObject)JsonConvert.DeserializeObject(_workFlowEvents.EventDetails);
        var values = eventDetails.Values();
        var clientId = values.Select(x => x.Value<string>("ClientId"))?.Single();
        var tradeCurrency = values.Select(x => x.Value<string>("TradeCurrency"))?.Single();
        var marketName = values.Select(x => x.Value<string>("MarketName"))?.Single();
        var tradeDate = values.Select(x => x.Value<string>("TradeDate"))?.Single();

        firstTrade = new FirstTrade
        {
            ClientId = clientId,
            MarketName = marketName,
            TradeDate = tradeDate,
            Currency = tradeCurrency
        };

        Trade.CustomerWorkflowId = _workFlowEvents.CustomerWorkflowId;
        Trade.CustomerId = _workFlowEvents.CustomerId;
        Trade.FirstTrade = firstTrade;
        Trade.FirstTrade.EventDateTime = _workFlowEvents.EventDateTime;

        return Trade;
    }
}
namespace ETX.Workflow.Customer.Application.Features.Responses;

using ETX.Workflow.Customer.Domain;

[ExcludeFromCodeCoverage]
public class CreateFirstTradeCommandResponse
    : ResponseBase
{
    public string EventName { get; set; } = Enumerations.GetEnumDescription(WorkflowEventType.FirstTrade);
    public FirstTradeEvent EventValue { get; set; }
}

public class FirstTradeEvent
{
    public string ClientId { get; set; }
    public string TradeDate { get; set; }
    public string MarketName { get; set; }
    public string Currency { get; set; }
    public double Amount { get; set; }
}
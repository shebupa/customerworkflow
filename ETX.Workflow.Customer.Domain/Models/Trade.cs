namespace ETX.Workflow.Customer.Domain.Models;

public class Trade
{
    public string CustomerWorkflowId { get; set; }
    public string CustomerId { get; set; }
    public FirstTrade FirstTrade { get; set; }
}

public class FirstTrade
{
    public string ClientId { get; set; }
    public string TradeDate { get; set; }
    public string MarketName { get; set; }
    public string Currency { get; set; }
    public double Amount { get; set; }
    public string EventDateTime { get; set; }
}
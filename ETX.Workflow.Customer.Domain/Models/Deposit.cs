namespace ETX.Workflow.Customer.Domain.Models;

public class Deposit
{
    public string CustomerWorkflowId { get; set; }
    public string CustomerId { get; set; }
    public FirstDeposit FirstDeposit { get; set; }
}

public class FirstDeposit
{
    public string Currency { get; set; }
    public double Amount { get; set; }
    public string EventDateTime { get; set; }
}
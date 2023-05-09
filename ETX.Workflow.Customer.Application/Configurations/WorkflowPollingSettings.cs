namespace ETX.Workflow.Customer.Application.Configurations;

[ExcludeFromCodeCoverage]
public class WorkflowPollingSettings
{
    public string RegistrationPollingInterval { get; init; }
    public string FirstDepositPollingInterval { get; init; }
    public string FirstTradePollingInterval { get; init; }
}
namespace ETX.Workflow.Customer.Application.Configurations;

[ExcludeFromCodeCoverage]
public class EventBusSettings : BrokerSettings
{
    public string RegistrationStatusTopicName { get; set; }
    public string FirstDepositTopicName { get; set; }
    public string FirstTradeTopicName { get; set; }
}
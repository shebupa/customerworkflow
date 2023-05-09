namespace ETX.Workflow.Customer.Application.Configurations;

[ExcludeFromCodeCoverage]
public class CacheSettings : BrokerSettings
{
    public string TimeToLiveTemplate { get; init; }
}
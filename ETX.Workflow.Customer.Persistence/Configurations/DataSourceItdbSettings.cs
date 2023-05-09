namespace ETX.Workflow.Customer.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class DataSourceItdbSettings
{
    public string SqlConnectionItdb { get; init; }
    public string RegistrationPollingInterval { get; init; }
    public string FirstDepositPollingInterval { get; init; }
    public string ItdbLimitRows { get; init; }
    public string DelayInMinutes { get; init; }
}
namespace ETX.Workflow.Customer.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class DataSourceTitanSettings
{
    public string SqlConnectionTitan { get; init; }
    public string FirstTradePollingInterval { get; init; }
    public string TitanLimitRows { get; init; }
}
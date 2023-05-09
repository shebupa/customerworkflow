namespace ETX.Workflow.Customer.Application.Configurations;

[ExcludeFromCodeCoverage]
public class BrokerSettings
{
    public string HostName { get; init; }
    public string Port { get; init; }
    public string User { get; init; }
    public string Password { get; init; }
    public string ClusterName { get; init; }
    public string RetryAttempt { get; init; }
    public string ExceptionsAllowedBeforeBreaking { get; init; }
    public string DurationOfCicruitBreak { get; init; }
    public string SleepDurationAfterRetry { get; init; }
}
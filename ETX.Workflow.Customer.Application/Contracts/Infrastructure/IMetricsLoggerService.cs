namespace ETX.Workflow.Customer.Application.Contracts.Infrastructure;

public interface IMetricsLoggerService
{
    void Flush();

    void LogDebugMessage(
        string msg,
        Dictionary<string, object> props = null);

    void LogInfoMessage(
        string msg,
        Dictionary<string, object> props = null);

    void LogWarnMessage(
        string msg,
        Dictionary<string, object> props = null);

    void LogErrorMessage(
        string msg,
        Dictionary<string, object> props = null);

    void LogFatalMessage(
        string msg,
        Dictionary<string, object> props = null);
}
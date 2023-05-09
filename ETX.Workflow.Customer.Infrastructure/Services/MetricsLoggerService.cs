namespace ETX.Workflow.Customer.Infrastructure.Services;

public class MetricsLoggerService : IMetricsLoggerService
{
    private readonly IMetricsLogger _metricsLogger = default;

    public MetricsLoggerService(IMetricsLogger metricsLogger)
    {
        _metricsLogger = metricsLogger;
    }

    public void Flush()
    {
        _metricsLogger.Flush();
    }

    public void LogDebugMessage(
            string msg,
            Dictionary<string, object> props = null)
    {
        _metricsLogger.LogDebugMessage(msg, props);
    }

    public void LogInfoMessage(
           string msg,
           Dictionary<string, object> props = null)
    {
        _metricsLogger.LogInfoMessage(msg, props);
    }

    public void LogWarnMessage(
        string msg,
        Dictionary<string, object> props = null)
    {
        _metricsLogger.LogWarnMessage(msg, props);
    }

    public void LogErrorMessage(
        string msg,
        Dictionary<string, object> props = null)
    {
        _metricsLogger.LogErrorMessage(msg, props);
    }

    public void LogFatalMessage(
        string msg,
        Dictionary<string, object> props = null)
    {
        _metricsLogger.LogFatalMessage(msg, props);
    }
}
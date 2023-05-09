namespace ETX.Workflow.Customer.Infrastructure.Services.Policies
{
    [ExcludeFromCodeCoverage]
    public abstract class PolicyBase
    {
        private readonly IMetricsLoggerService _logger = default;

        public PolicyBase(
            IMetricsLoggerService logger)
        {
            _logger = logger;
        }

        protected abstract void Initialise();

        protected void OnBreak(
           Exception responseMessage,
           TimeSpan timeSpan)
        {
            _logger.LogInfoMessage($"Error in Connecting Wait and Try again," +
                                    $"breaking timeSpan={timeSpan} Seconds for exception=\"{responseMessage.Message}\"");
        }

        protected void OnReset()
        {
            _logger.LogInfoMessage($"Reseting and Try Connection");
        }

        protected void OnHalfOpen()
        {
            _logger.LogInfoMessage($"Open Once, Connect and Try Connecting");
        }
    }
}
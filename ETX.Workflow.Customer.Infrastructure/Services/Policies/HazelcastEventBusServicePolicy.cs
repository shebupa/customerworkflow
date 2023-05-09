namespace ETX.Workflow.Customer.Infrastructure.Services.Policies;

public class HazelcastEventBusServicePolicy
    : PolicyBase
      , IEventBusBrokerService
{
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IMetricsLoggerService _logger = default;
    private static IEventBusServicePolicyFacade _eventBusServicePolicyFacade = default;
    private static AsyncRetryPolicy _retryPolicy = default;
    private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy = default;

    public HazelcastEventBusServicePolicy(
          EventBusSettings eventBusSettings
          , IMetricsLoggerService logger
          , IEventBusServicePolicyFacade eventBusServicePolicyFacade) : base(logger)
    {
        _eventBusSettings = eventBusSettings;
        _logger = logger;
        _eventBusServicePolicyFacade = eventBusServicePolicyFacade;

        Initialise();
    }

    public async Task<bool> ConnectToServerAsync()
    {
        var isConnected = await _eventBusServicePolicyFacade.ConnectToServerAsync(
                                _retryPolicy
                                , _circuitBreakerPolicy);
        return isConnected;
    }

    public async Task WriteAsync(
        string topicName
        , object message)
    {
        await _eventBusServicePolicyFacade.WriteAsync(
                    _retryPolicy
                    , _circuitBreakerPolicy
                    , topicName
                    , message);
    }

    protected override void Initialise()
    {
        var policies = _eventBusServicePolicyFacade.Initialise(
                                    OnBreak
                                    , OnReset
                                    , OnHalfOpen);

        _retryPolicy = policies.Item1;
        _circuitBreakerPolicy = policies.Item2;
    }
}
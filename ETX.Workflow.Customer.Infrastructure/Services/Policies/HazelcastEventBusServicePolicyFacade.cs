using ETX.Workflow.Customer.Application.Features.Responses;

namespace ETX.Workflow.Customer.Infrastructure.Services.Policies;

public class HazelcastEventBusServicePolicyFacade
    : IEventBusServicePolicyFacade
{
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IMetricsLoggerService _metricsLogger = default;
    private IHazelcastClient _hazelcastClient = default;

    public HazelcastEventBusServicePolicyFacade(
         EventBusSettings eventBusSettings
         , IHazelcastClient hazelcastClient
         , IMetricsLoggerService logger
         )
    {
        _metricsLogger = logger;
        _eventBusSettings = eventBusSettings;
        _hazelcastClient = hazelcastClient;
    }

    public async Task<bool> ConnectToServerAsync(
         AsyncRetryPolicy _retryPolicy
        , AsyncCircuitBreakerPolicy _circuitBreakerPolicy)
    {
        var isConnected = await ExecuteWithRetryAndCircuitBreaker<bool>(
                                _retryPolicy
                                , _circuitBreakerPolicy
                                , ConnectToServer);

        return true;
    }

    public async Task WriteAsync(
        AsyncRetryPolicy _retryPolicy
        , AsyncCircuitBreakerPolicy _circuitBreakerPolicy
        , string topicName
        , object message)

    {
        await ExecuteWithRetryAndCircuitBreaker(
                    _retryPolicy
                    , _circuitBreakerPolicy
                    , topicName
                    , message
                    , async (topicName, messageToWrite) => await (Write(topicName, messageToWrite)));
    }

    private async Task<bool> ConnectToServer()
    {
        bool isConnected = true;
        if (_hazelcastClient.IsConnected)
        {
            isConnected = await Task.FromResult<bool>(true);
        }
        else
        {
            var util = new Util();
            _hazelcastClient = util.ConnectToHazelcast(
                              new HazelcastSerialiser(_metricsLogger)
                              , _eventBusSettings
                              , _metricsLogger);
            isConnected = await Task.FromResult<bool>(_hazelcastClient.IsConnected);
        }

        if (!isConnected)
        {
            throw new EventBusConnectionException();
        }

        return isConnected;
    }

    private async Task<bool> Write(
          string topicName
          , object message)
    {
        try
        {
            await using var topic = await _hazelcastClient.GetTopicAsync<object>(topicName);

            _metricsLogger.LogInfoMessage($"Event Published successfully to Topic={topicName} " +
                                          $"trackerId={((ResponseBase)message).CustomerWorkflowId} " +
                                          $"address={_eventBusSettings.HostName} " +
                                          $"port={_eventBusSettings.Port} " +
                                          $"queueName={topicName} " +
                                          $"clusterName={_eventBusSettings.ClusterName}");

            await topic.PublishAsync(message);
            return true;
        }
        catch (Exception exception)
        {
            _metricsLogger.LogErrorMessage($"Error in publishing event, trackerId={((ResponseBase)message).CustomerWorkflowId}" +
                                    $"exception=\"{exception.Message}\" " +
                                    $"innerException=\"{exception.InnerException?.Message}\"");
            throw new EventBusConnectionException(exception.Message);
        }
    }

    private async Task<TOutput> ExecuteWithRetryAndCircuitBreaker<TOutput>(
            AsyncRetryPolicy _retryPolicy
            , AsyncCircuitBreakerPolicy _circuitBreakerPolicy
            , Func<Task<TOutput>> connectAndSend)
    {
        var response = await _retryPolicy
                             .WrapAsync(_circuitBreakerPolicy)
                             .ExecuteAsync(async () => await connectAndSend());

        return response;
    }

    private async Task<TOutput> ExecuteWithRetryAndCircuitBreaker<TOutput>(
            AsyncRetryPolicy _retryPolicy
            , AsyncCircuitBreakerPolicy _circuitBreakerPolicy
            , string topicName
            , object input
            , Func<string, object, Task<TOutput>> connectAndSend)
    {
        var response = await _retryPolicy
                       .WrapAsync(_circuitBreakerPolicy)
                       .ExecuteAsync(async () => await connectAndSend(topicName, input));

        return response;
    }

    public (AsyncRetryPolicy, AsyncCircuitBreakerPolicy) Initialise(
            Action<Exception, TimeSpan> OnBreak
            , Action OnReset
            , Action OnHalfOpen)
    {
        AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy
                                 .Handle<EventBusConnectionException>()
                                 .Or<OperationCanceledException>()
                                 .CircuitBreakerAsync(int.Parse(_eventBusSettings.ExceptionsAllowedBeforeBreaking)
                                                      , TimeSpan.FromSeconds(int.Parse(_eventBusSettings.DurationOfCicruitBreak))
                                                      , OnBreak
                                                      , OnReset
                                                      , OnHalfOpen);

        AsyncRetryPolicy retryPolicy = Policy
                       .Handle<EventBusConnectionException>()
                       .WaitAndRetryAsync(
                            int.Parse(_eventBusSettings.RetryAttempt), (retryAttempt) =>
                            {
                                _metricsLogger.LogInfoMessage($"Error in Connecting EventBus Broker Service Retry, " +
                                                              $"retryAttempt={retryAttempt} " +
                                                              $"count={int.Parse(_eventBusSettings.RetryAttempt)} " +
                                                              $"Wait for sleepDuration={int.Parse(_eventBusSettings.SleepDurationAfterRetry)}");
                                return TimeSpan.FromSeconds(int.Parse(_eventBusSettings.SleepDurationAfterRetry));
                            });

        return (retryPolicy, circuitBreakerPolicy);
    }
}
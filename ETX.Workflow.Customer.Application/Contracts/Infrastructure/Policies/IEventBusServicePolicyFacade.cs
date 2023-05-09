namespace ETX.Workflow.Customer.Application.Contracts.Infrastructure.Policies;

public interface IEventBusServicePolicyFacade
{
    public (AsyncRetryPolicy, AsyncCircuitBreakerPolicy) Initialise(
            Action<Exception, TimeSpan> OnBreak
            , Action OnReset
            , Action OnHalfOpen);

    Task<bool> ConnectToServerAsync(
            AsyncRetryPolicy _retryPolicy
            , AsyncCircuitBreakerPolicy _circuitBreakerPolicy);

    Task WriteAsync(
            AsyncRetryPolicy _retryPolicy
            , AsyncCircuitBreakerPolicy _circuitBreakerPolicy
            , string topicName
            , object message);
}
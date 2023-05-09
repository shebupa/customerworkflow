namespace ETX.Workflow.Customer.Infrastructure.Services;

public class HazelcastEventBusBrokerService : IEventBusBrokerService
{
    private IHazelcastClient _hazelcastClient = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IMetricsLoggerService _metricsLogger = default;

    public HazelcastEventBusBrokerService(
            IHazelcastClient hazelcastClient
            , EventBusSettings eventBusSettings
            , IMetricsLoggerService logger)
    {
        _hazelcastClient = hazelcastClient;
        _eventBusSettings = eventBusSettings;
        _metricsLogger = logger;
    }

    public async Task<bool> ConnectToServerAsync()
    {
        if (_hazelcastClient.IsConnected)
        {
            return await Task.FromResult<bool>(true);
        }
        else
        {
            var util = new Util();
            _hazelcastClient = util.ConnectToHazelcast(
                              new HazelcastSerialiser(_metricsLogger)
                              , _eventBusSettings
                              , _metricsLogger);
            return await Task.FromResult<bool>(_hazelcastClient.IsConnected);
        }
    }

    public async Task WriteAsync(
        string topicName
        , object message)
    {
        await using var topic = await _hazelcastClient.GetTopicAsync<object>(topicName);

        _metricsLogger.LogDebugMessage($"Event={message} " +
                                       $"address={_eventBusSettings.HostName} " +
                                       $"port={_eventBusSettings.Port} " +
                                       $"queueName={topicName} " +
                                       $"clusterName={_eventBusSettings.ClusterName}");

        await topic.PublishAsync(message);
    }
}
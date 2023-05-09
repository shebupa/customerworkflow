namespace ETX.Workflow.Customer.Infrastructure.Services;

public class EventPublisherService : IPublisherService
{
    private readonly IEventBusBrokerService _eventBusBrokerService = default;

    public EventPublisherService(
        IEventBusBrokerService eventBusBrokerService)
    {
        _eventBusBrokerService = eventBusBrokerService;
    }

    public async Task ConnectToServerAsync()
    {
        await _eventBusBrokerService.ConnectToServerAsync();
    }

    public async Task PublishEventAsync(
        string topicName
        , object message)
    {
        await _eventBusBrokerService.WriteAsync(
                                     topicName
                                     , message);
    }
}
namespace ETX.Workflow.Customer.Application.Contracts.Infrastructure;

public interface IPublisherService
{
    Task ConnectToServerAsync();

    Task PublishEventAsync(
            string topicName
            , object message);
}
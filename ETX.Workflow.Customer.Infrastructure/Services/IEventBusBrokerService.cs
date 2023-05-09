namespace ETX.Workflow.Customer.Infrastructure.Services;

public interface IEventBusBrokerService
{
    Task<bool> ConnectToServerAsync();

    Task WriteAsync(
            string topicName
            , object message);
}
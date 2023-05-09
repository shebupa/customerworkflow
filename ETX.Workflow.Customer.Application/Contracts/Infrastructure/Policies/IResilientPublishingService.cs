namespace ETX.Workflow.Customer.Application.Contracts.Infrastructure.Policies;

public interface IResilientPublishingService
{
    Task<bool> ConnectToServerAsync();

    Task WriteAsync(
            string topicName
            , object message);
}
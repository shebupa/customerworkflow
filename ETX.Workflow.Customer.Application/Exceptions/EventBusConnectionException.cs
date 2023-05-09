namespace ETX.Workflow.Customer.Application.Exceptions;

public sealed class EventBusConnectionException
     : Exception
{
    public EventBusConnectionException()
    {
    }

    public EventBusConnectionException(
        string message)
        : base(message)
    {
    }
}
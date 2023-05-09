namespace ETX.Workflow.Customer.Application.Exceptions;

public sealed class DBServiceConnectionException
       : Exception
{
    public DBServiceConnectionException()
    {
    }

    public DBServiceConnectionException(
        string message)
        : base(message)
    {
    }
}
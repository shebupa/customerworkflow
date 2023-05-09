namespace ETX.Workflow.Customer.Application.Tests.Exceptions;

[ExcludeFromCodeCoverage]
public class DBServiceConnectionExceptionTests
{
    public DBServiceConnectionExceptionTests()
    { }

    [Fact]
    public void IsInheritsFromException()
    {
        //Arrange
        var exception = new EventBusConnectionException();

        //Assert
        exception.ShouldBeAssignableTo<Exception>();
    }

    [Fact]
    public void Is_Exist_Construct_With_Argument_Of_Type_String()
    {
        //Act
        //Arrange
        var message = "Error in Connecting to Database";
        var exception = new EventBusConnectionException(message);

        //Assert
        exception.ShouldBeAssignableTo<Exception>();
    }
}
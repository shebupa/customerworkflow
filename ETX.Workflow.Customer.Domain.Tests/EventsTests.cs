namespace ETX.Workflow.Customer.Domain.Tests;

public class EventsTests
{
    public EventsTests()
    {
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public void IsExist_RegistrationStatus(
       CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        //Act
        var registrationStatus = new Events.WorkflowEvents
        {
            EventTypeId = createRegistrationStatusCommandRequest.EventTypeId,
            CustomerId = createRegistrationStatusCommandRequest.CustomerId,
            EventDetails = createRegistrationStatusCommandRequest.EventDetails
        };
    }
}
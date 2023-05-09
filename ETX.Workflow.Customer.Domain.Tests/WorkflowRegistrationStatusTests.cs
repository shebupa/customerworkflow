namespace ETX.Workflow.Customer.Domain.Tests;

[ExcludeFromCodeCoverage]
public class WorkflowRegistrationStatusTests
{
    public WorkflowRegistrationStatusTests()
    {
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildRegistrationStatusWorkflow")]
    public void IsExist(
        CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createRegistrationStatusCommandRequest);

        var customerWorkflow = new WorkflowRegistrationStatus(workFlowEvents);
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildRegistrationStatusWorkflow")]
    public void WorkflowRegistrationStatus_Should_Return_RegistrationStatus_Event(
         CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createRegistrationStatusCommandRequest);

        //Act
        var registrationStatus = new WorkflowRegistrationStatus(workFlowEvents);

        //Assert
        registrationStatus.ShouldNotBeNull();
        registrationStatus.Registration.CustomerWorkflowId
                    .ShouldBeEquivalentTo(createRegistrationStatusCommandRequest.CustomerWorkflowId.ToString());
        registrationStatus.Registration.CustomerId
                    .ShouldBeEquivalentTo(createRegistrationStatusCommandRequest.CustomerId.ToString());
    }
}
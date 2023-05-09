namespace ETX.Workflow.Customer.Domain.Tests;

[ExcludeFromCodeCoverage]
public class WorkflowFirstDepositTests
{
    public WorkflowFirstDepositTests()
    {
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstDepositWorkflow")]
    public void IsExist(
        CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createFirstDepositCommandRequest);

        //Act
        var customerWorkflow = new WorkflowFirstDeposit(workFlowEvents);
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstDepositWorkflow")]
    public void WorkflowFirstDeposit_Should_Return_FirstDeposit_Event(
         CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createFirstDepositCommandRequest);

        //Act
        var firstDeposit = new WorkflowFirstDeposit(workFlowEvents);

        //Assert
        firstDeposit.ShouldNotBeNull();
        firstDeposit.Deposit.CustomerWorkflowId
                    .ShouldBeEquivalentTo(createFirstDepositCommandRequest.CustomerWorkflowId.ToString());
        firstDeposit.Deposit.CustomerId
                    .ShouldBeEquivalentTo(createFirstDepositCommandRequest.CustomerId.ToString());
    }
}
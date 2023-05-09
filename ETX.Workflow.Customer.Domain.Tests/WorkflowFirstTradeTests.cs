namespace ETX.Workflow.Customer.Domain.Tests;

[ExcludeFromCodeCoverage]
public class WorkflowFirstTradeTests
{
    public WorkflowFirstTradeTests()
    {
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstTradeWorkflow")]
    public void IsExist(
        CreateFirstTradeCommandRequest createFirstTradeCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createFirstTradeCommandRequest);

        //Act
        var customerWorkflow = new WorkflowFirstTrade(workFlowEvents);
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstTradeWorkflow")]
    public void WorkflowFirstDeposit_Should_Return_FirstDeposit_Event(
         CreateFirstTradeCommandRequest createFirstTradeCommandRequest)
    {
        //Arrange
        var workFlowEvents = new Events.WorkflowEvents();
        workFlowEvents = Util.CreateWorkflowEvent(createFirstTradeCommandRequest);

        //Act
        var firstTrade = new WorkflowFirstTrade(workFlowEvents);

        //Assert
        firstTrade.ShouldNotBeNull();
        firstTrade.Trade.CustomerId
                    .ShouldBeEquivalentTo(createFirstTradeCommandRequest.CustomerId.ToString());
        firstTrade.Trade.CustomerWorkflowId
                       .ShouldBeEquivalentTo(createFirstTradeCommandRequest.CustomerWorkflowId);
        firstTrade.Trade.FirstTrade
                      .MarketName.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        firstTrade.Trade.FirstTrade
                      .Amount.ToString().ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        firstTrade.Trade.FirstTrade
                      .ClientId.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        firstTrade.Trade.FirstTrade
                      .Currency.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
    }
}
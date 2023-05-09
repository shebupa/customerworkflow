using ETX.Workflow.Customer.Domain.Exceptions;

namespace ETX.Workflow.Customer.Domain.Tests;

[ExcludeFromCodeCoverage]
public class WorkflowTests
{
    public WorkflowTests()
    {
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstDepositWorkflow")]
    public void IsExist(
        CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        var workflow = new Workflow(
                                  new Events.WorkflowEvents
                                  {
                                      CustomerWorkflowId = createFirstDepositCommandRequest.CustomerWorkflowId,
                                      CustomerId = createFirstDepositCommandRequest.CustomerId,
                                      EventTypeId = createFirstDepositCommandRequest.EventTypeId,
                                      EventDetails = createFirstDepositCommandRequest.EventDetails,
                                  });
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstDepositWorkflow")]
    public void Workflow_Should_Form_FirstDeposit_Event(
         CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        var workflow = new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createFirstDepositCommandRequest.CustomerWorkflowId,
                                     CustomerId = createFirstDepositCommandRequest.CustomerId,
                                     EventTypeId = createFirstDepositCommandRequest.EventTypeId,
                                     EventDetails = createFirstDepositCommandRequest.EventDetails
                                 });

        //Act
        var generatedEvent = JsonConvert.SerializeObject(workflow);

        //Assert
        generatedEvent.ShouldBeAssignableTo<object>();
        generatedEvent.ShouldNotBeNull();
        workflow.Deposit.FirstDeposit
            .Currency.ShouldBeSubsetOf(createFirstDepositCommandRequest.EventDetails);
        workflow.Deposit.FirstDeposit
            .Amount.ToString().ShouldBeSubsetOf(createFirstDepositCommandRequest.EventDetails);
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildRegistrationStatusWorkflow")]
    public void Workflow_Should_Form_RegistrationStatus_Event(
        CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        var workflow = new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createRegistrationStatusCommandRequest.CustomerWorkflowId,
                                     CustomerId = createRegistrationStatusCommandRequest.CustomerId,
                                     EventTypeId = createRegistrationStatusCommandRequest.EventTypeId,
                                     EventDetails = createRegistrationStatusCommandRequest.EventDetails
                                 });

        //Act
        var generatedEvent = JsonConvert.SerializeObject(workflow);

        //Assert
        generatedEvent.ShouldBeAssignableTo<object>();
        generatedEvent.ShouldNotBeNull();
        workflow.Registration.CustomerId
                        .ShouldBeEquivalentTo(createRegistrationStatusCommandRequest.CustomerId);
        workflow.Registration.CustomerWorkflowId
                        .ShouldBeEquivalentTo(createRegistrationStatusCommandRequest.CustomerWorkflowId);
        workflow.Registration.RegistrationStatus
                        .StatusDescription.ShouldBeSubsetOf("Join");
    }

    [Theory]
    [LoadData("WorkflowEvents_BuildFirstTradeWorkflow")]
    public void Workflow_Should_Form_FirstTrade_Event(
       CreateRegistrationStatusCommandRequest createFirstTradeCommandRequest)
    {
        //Arrange
        var workflow = new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createFirstTradeCommandRequest.CustomerWorkflowId,
                                     CustomerId = createFirstTradeCommandRequest.CustomerId,
                                     EventTypeId = createFirstTradeCommandRequest.EventTypeId,
                                     EventDetails = createFirstTradeCommandRequest.EventDetails
                                 });

        //Act
        var generatedEvent = JsonConvert.SerializeObject(workflow);

        //Assert
        generatedEvent.ShouldBeAssignableTo<object>();
        generatedEvent.ShouldNotBeNull();
        workflow.Trade.CustomerId
                        .ShouldBeEquivalentTo(createFirstTradeCommandRequest.CustomerId);
        workflow.Trade.CustomerWorkflowId
                        .ShouldBeEquivalentTo(createFirstTradeCommandRequest.CustomerWorkflowId);
        workflow.Trade.FirstTrade
                      .MarketName.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        workflow.Trade.FirstTrade
                      .Amount.ToString().ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        workflow.Trade.FirstTrade
                      .ClientId.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
        workflow.Trade.FirstTrade
                      .Currency.ShouldBeSubsetOf(createFirstTradeCommandRequest.EventDetails);
    }

    [Theory]
    [LoadData("CreateFirstDepositCommandRequest_BuildFirstDepositWorkflowEvent_Invalid")]
    public void Workflow_FirstDeposit_Should_Throw_Exception(
         CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        //Act
        //Assert
        Should.Throw<DomainExceptions.InvalidEntityState>(() => new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createFirstDepositCommandRequest.CustomerWorkflowId,
                                     CustomerId = createFirstDepositCommandRequest.CustomerId,
                                     EventTypeId = createFirstDepositCommandRequest.EventTypeId,
                                     EventDetails = createFirstDepositCommandRequest.EventDetails
                                 }))
              .Message.Contains("Post-checks failed in state");
    }

    [Theory]
    [LoadData("CreateFirstTradeCommandRequest_BuildFirstTradeWorkflowEvent_Invalid")]
    public void Workflow_FirstTrade_Should_Throw_Exception(
         CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        //Act
        //Assert
        Should.Throw<DomainExceptions.InvalidEntityState>(() => new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createRegistrationStatusCommandRequest.CustomerWorkflowId,
                                     CustomerId = createRegistrationStatusCommandRequest.CustomerId,
                                     EventTypeId = createRegistrationStatusCommandRequest.EventTypeId,
                                     EventDetails = createRegistrationStatusCommandRequest.EventDetails
                                 }))
              .Message.Contains("Post-checks failed in state");
    }

    [Theory]
    [LoadData("CreateRegistrationStatusCommandRequest_BuildRegistrationStatusWorkflowEvent_Invalid")]
    public void Workflow_RegistrationStatus_Should_Throw_Exception(
         CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        //Act
        //Assert
        Should.Throw<DomainExceptions.InvalidEntityState>(() => new Workflow(
                                 new Events.WorkflowEvents
                                 {
                                     CustomerWorkflowId = createRegistrationStatusCommandRequest.CustomerWorkflowId,
                                     CustomerId = createRegistrationStatusCommandRequest.CustomerId,
                                     EventTypeId = createRegistrationStatusCommandRequest.EventTypeId,
                                     EventDetails = createRegistrationStatusCommandRequest.EventDetails
                                 }))
              .Message.Contains("Post-checks failed in state");
    }
}
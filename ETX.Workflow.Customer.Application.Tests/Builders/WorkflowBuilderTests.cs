namespace ETX.Workflow.Customer.Application.Tests.Builders;

[ExcludeFromCodeCoverage]
public class WorkflowBuilderTests
{
    private readonly Mock<IMediator> _mediatorMock = default;
    private readonly Mock<IMapper> _mapperMock = default;
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;
    private readonly IWorkflowBuilder _workflowBuilder = default;

    public WorkflowBuilderTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _metricsloggerMock = new Mock<IMetricsLoggerService>();
        _workflowBuilder = new WorkflowBuilder(
                                      _metricsloggerMock.Object
                                      , _mapperMock.Object);
    }

    [Fact]
    public void WorkflowBuilder_Should_Be_Of_Type_ICustomerWorkflowBuilder()
    {
        //Assert
        _workflowBuilder.ShouldBeAssignableTo<IWorkflowBuilder>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Join")]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Completed")]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Authorised")]
    public void BuildCommand_Should_Return_NotNull_And_Return_CreateRegistrationStatusCommandRequest_Demo(
        CreateWorkflowRequest request)
    {
        //Arrange
        var registrationStatusCommandRequest = new CreateRegistrationStatusCommandRequest();
        registrationStatusCommandRequest.CustomerId = request.CustomerId;

        _mapperMock.Setup(x => x.Map<CreateRegistrationStatusCommandRequest>(request))
                   .Returns(registrationStatusCommandRequest).Verifiable();

        //Act
        var workFlowCommand = (CreateRegistrationStatusCommandRequest)_workflowBuilder
                              .BuildCommand(request);

        //Assert
        workFlowCommand.ShouldNotBeNull();
        workFlowCommand.ShouldBeAssignableTo<CreateRegistrationStatusCommandRequest>();
        _mapperMock.Verify(x => x.Map<CreateRegistrationStatusCommandRequest>(request), Times.Once());
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstDepositWorkflow")]
    public void BuildCommand_Should_Return_NotNull_And_Return_CreateFirstDepositCommandRequest(
       CreateWorkflowRequest request)
    {
        //Arrange
        var firstDepositCommandRequest = new CreateFirstDepositCommandRequest();
        firstDepositCommandRequest.CustomerId = request.CustomerId;
        _mapperMock.Setup(x => x.Map<CreateFirstDepositCommandRequest>(request))
                   .Returns(firstDepositCommandRequest).Verifiable();

        //Act
        var workFlowCommand = (CreateFirstDepositCommandRequest)_workflowBuilder
                              .BuildCommand(request);

        //Assert
        workFlowCommand.ShouldNotBeNull();
        workFlowCommand.ShouldBeAssignableTo<CreateFirstDepositCommandRequest>();
        workFlowCommand.CustomerId.ShouldBeEquivalentTo(request.CustomerId);
        _mapperMock.Verify(x => x.Map<CreateFirstDepositCommandRequest>(request), Times.Once());
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTradeWorkflow")]
    public void BuildCommand_Should_Return_NotNull_And_Return_CreateFirstTradeCommandRequest(
       CreateWorkflowRequest request)
    {
        //Arrange
        var firstTradeCommandRequest = new CreateFirstTradeCommandRequest();
        firstTradeCommandRequest.CustomerId = request.CustomerId;
        _mapperMock.Setup(x => x.Map<CreateFirstTradeCommandRequest>(request))
                .Returns(firstTradeCommandRequest).Verifiable();

        //Act
        var workFlowCommand = (CreateFirstTradeCommandRequest)_workflowBuilder
                              .BuildCommand(request);

        //Assert
        workFlowCommand.ShouldNotBeNull();
        workFlowCommand.ShouldBeAssignableTo<CreateFirstTradeCommandRequest>();
        workFlowCommand.CustomerId.ShouldBeEquivalentTo(request.CustomerId);
        _mapperMock.Verify(x => x.Map<CreateFirstTradeCommandRequest>(request), Times.Once());
    }
}
namespace ETX.Workflow.Customer.Application.Tests;

[ExcludeFromCodeCoverage]
public class RequestHanderTests
{
    private readonly Mock<IMediator> _mediatorMock = default;
    private readonly Mock<IMapper> _mapperMock = default;
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;
    private readonly Mock<IWorkflowBuilder> _workflowBuilderMock = default;
    private IRequestHandler _requestHandler = default;
    private readonly Mock<IWorkflowSource> _workflowSourceMock = default;

    private Func<WorkflowEventSource, IWorkflowSource>
       _customerWorkflowEventSourceDelegate = default;

    public RequestHanderTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _metricsloggerMock = new Mock<IMetricsLoggerService>();
        _workflowBuilderMock = new Mock<IWorkflowBuilder>();
        _workflowSourceMock = new Mock<IWorkflowSource>();

        _customerWorkflowEventSourceDelegate = (x) => _workflowSourceMock.Object;
        _requestHandler = new RequestHandler(
                                 _metricsloggerMock.Object
                                 , _mediatorMock.Object
                                 , _mapperMock.Object
                                 , _workflowBuilderMock.Object
                                 , _customerWorkflowEventSourceDelegate);
    }

    [Fact]
    public void RequestHandler_Should_Be_Of_Type_IRequestHandler()
    {
        //Assert
        _requestHandler.ShouldBeAssignableTo<IRequestHandler>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task HandleCommands_Should_Accept_EventSource_And_Should_Call_BuildCommand_With_RegistrationStatus(
        CustomerWorkflowEvents request)
    {
        //Arrange
        var responseBase = new ResponseBase();
        var registrationStatusCommandRequest = new CreateRegistrationStatusCommandRequest();
        var workflowEvents = new List<CustomerWorkflowEvents>();
        var createWorkflowRequest = new CreateWorkflowRequest();

        workflowEvents.Add(request);
        _mediatorMock.Setup(x => x.Send(registrationStatusCommandRequest, CancellationToken.None))
                     .Verifiable();

        _mapperMock.Setup(x => x.Map<CreateWorkflowRequest>(request))
                   .Returns(createWorkflowRequest);
        _workflowBuilderMock.Setup(x => x.BuildCommand(createWorkflowRequest))
                                    .Returns(registrationStatusCommandRequest);

        _workflowSourceMock.Setup(x => x.GetWorkflowEventsAsync())
                                   .ReturnsAsync(workflowEvents);

        //Act
        await _requestHandler.HandleCommandsAsync(WorkflowEventSource.DbItdb);

        //Assert
        _workflowBuilderMock.Verify(x => x.BuildCommand(createWorkflowRequest), Times.Once());
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstDepositWorkflow")]
    public async Task HandleCommands_Should_Accept_EventSource_And_Should_Call_BuildCommand_With_FirstDeposit(
       CustomerWorkflowEvents request)
    {
        //Arrange
        var responseBase = new ResponseBase();
        var firstDepositCommandRequest = new CreateFirstDepositCommandRequest();
        var workflowEvents = new List<CustomerWorkflowEvents>();
        var createWorkflowRequest = new CreateWorkflowRequest();

        workflowEvents.Add(request);
        _mediatorMock.Setup(x => x.Send(firstDepositCommandRequest, CancellationToken.None))
                     .Verifiable();

        _mapperMock.Setup(x => x.Map<CreateWorkflowRequest>(request))
                   .Returns(createWorkflowRequest);
        _workflowBuilderMock.Setup(x => x.BuildCommand(createWorkflowRequest))
                                    .Returns(firstDepositCommandRequest);

        _workflowSourceMock.Setup(x => x.GetWorkflowEventsAsync())
                                   .ReturnsAsync(workflowEvents);

        //Act
        await _requestHandler.HandleCommandsAsync(WorkflowEventSource.DbItdb);

        //Assert
        _workflowBuilderMock.Verify(x => x.BuildCommand(createWorkflowRequest), Times.Once());
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstDepositWorkflow")]
    public void HandleCommands_Should_Raise_Exception_And_Should_Not_Process(
       CustomerWorkflowEvents request)
    {
        //Arrange
        var responseBase = new ResponseBase();
        var firstDepositCommandRequest = new CreateFirstDepositCommandRequest();
        var workflowEvents = new List<CustomerWorkflowEvents>();
        var createWorkflowRequest = new CreateWorkflowRequest();

        _customerWorkflowEventSourceDelegate = (x) => null;
        _requestHandler = new RequestHandler(
                                        _metricsloggerMock.Object
                                        , _mediatorMock.Object
                                        , _mapperMock.Object
                                        , _workflowBuilderMock.Object
                                        , _customerWorkflowEventSourceDelegate);

        //Assert
        Should.NotThrow(async () => await _requestHandler.HandleCommandsAsync(
                                                          0));
    }
}
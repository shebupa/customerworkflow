namespace ETX.Workflow.Customer.Application.Tests.Features.CustomerWorkflow.Handlers.Commands;

[ExcludeFromCodeCoverage]
public class CreateDepositCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = default;
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;
    private readonly Mock<IPublisherService> _publisherServiceMock = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly Mock<IWorkflowResponseValidator> _workflowResponseValidator = default;

    public CreateDepositCommandHandlerTests()
    {
        _eventBusSettings = new EventBusSettings();
        _mapperMock = new Mock<IMapper>();
        _metricsloggerMock = new Mock<IMetricsLoggerService>();
        _publisherServiceMock = new Mock<IPublisherService>();
        _workflowResponseValidator = new Mock<IWorkflowResponseValidator>();

        _publisherServiceMock.Setup(x => x.ConnectToServerAsync())
                             .Returns(Task.FromResult<bool>(true));

        //Config Read
        var config = ETX.Workflow.Customer.Fixtures.Configurations.Util.InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);
    }

    [Fact]
    public void CreateDepositCommandHandler_Is_Of_Type_IRequestHandler()
    {
        //Arrange
        //Act
        var createDepositCommandHandler = new CreateDepositCommandHandler(
                                              _eventBusSettings
                                              , _mapperMock.Object
                                              , _metricsloggerMock.Object
                                              , _publisherServiceMock.Object
                                              , _workflowResponseValidator.Object);

        //Assert
        createDepositCommandHandler
            .ShouldBeAssignableTo<MediatR.IRequestHandler<CreateFirstDepositCommandRequest, CreateFirstDepositCommandResponse>>();
    }

    [Theory]
    [LoadData("CreateFirstDepositCommandRequest_BuildFirstDepositWorkflowEvent")]
    public async Task CreateDepositCommandHandler_Should_Return_CreateFirstDepositCommandResponse_And_Call_PublishEventAsync(
         CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //deposit
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createFirstDepositCommandRequest);

        _publisherServiceMock.Setup(x =>
                              x.PublishEventAsync(_eventBusSettings.FirstDepositTopicName, createFirstDepositCommandResponse))
                              .Verifiable();
        _mapperMock.Setup(x => x.Map<Events.WorkflowEvents>(createFirstDepositCommandRequest))
                   .Returns(workflowEvents).Verifiable();
        _mapperMock.Setup(x => x.Map<CreateFirstDepositCommandResponse>(It.IsAny<Deposit>()))
                   .Returns(createFirstDepositCommandResponse).Verifiable();
        _workflowResponseValidator.Setup(x => x.IsValidResponseToSend(createFirstDepositCommandResponse))
                               .Returns(true);

        var createDepositCommandHandler = new CreateDepositCommandHandler(
                                             _eventBusSettings
                                             , _mapperMock.Object
                                             , _metricsloggerMock.Object
                                             , _publisherServiceMock.Object
                                             , _workflowResponseValidator.Object);

        //Act
        var response = await createDepositCommandHandler.Handle(
                             createFirstDepositCommandRequest
                             , CancellationToken.None);

        //Assert
        response.ShouldBeAssignableTo<CreateFirstDepositCommandResponse>();
        _mapperMock.Verify(x => x.Map<Events.WorkflowEvents>(createFirstDepositCommandRequest), Times.Once());
        _publisherServiceMock.Verify(x =>
                                     x.PublishEventAsync(_eventBusSettings.FirstDepositTopicName
                                     , createFirstDepositCommandResponse), Times.Once());
    }

    [Theory]
    [LoadData("CreateFirstDepositCommandRequest_BuildFirstDepositWorkflowEvent_Invalid")]
    public void CreateDepositCommandHandler_Should_NotThrow_Exception_And_Return_Null_Response_And_Not_Call_PublishEventAsync(
         CreateFirstDepositCommandRequest createFirstDepositCommandRequest)
    {
        //Arrange
        var deposit = new Deposit();
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createFirstDepositCommandRequest);
        _mapperMock.Setup(x => x.Map<Events.WorkflowEvents>(createFirstDepositCommandRequest))
                   .Returns(workflowEvents).Verifiable();

        var createDepositCommandHandler = new CreateDepositCommandHandler(
                                                         _eventBusSettings
                                                         , _mapperMock.Object
                                                         , _metricsloggerMock.Object
                                                         , _publisherServiceMock.Object
                                                         , _workflowResponseValidator.Object
                                                         );

        //Assert
        Should.NotThrow<CreateFirstDepositCommandResponse>(async () => await createDepositCommandHandler.Handle(
                                                                                   createFirstDepositCommandRequest
                                                                                   , CancellationToken.None)).ShouldBeNull();
    }
}
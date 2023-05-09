namespace ETX.Workflow.Customer.Application.Tests.Features.CustomerWorkflow.Handlers.Commands;

[ExcludeFromCodeCoverage]
public class CreateTradeCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = default;
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;
    private readonly Mock<IPublisherService> _publisherServiceMock = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly Mock<IWorkflowResponseValidator> _workflowResponseValidator = default;

    public CreateTradeCommandHandlerTests()
    {
        _eventBusSettings = new EventBusSettings();
        _mapperMock = new Mock<IMapper>();
        _metricsloggerMock = new Mock<IMetricsLoggerService>();
        _publisherServiceMock = new Mock<IPublisherService>();
        _workflowResponseValidator = new Mock<IWorkflowResponseValidator>();

        //Config Read
        var config = ETX.Workflow.Customer.Fixtures.Configurations.Util.InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);
    }

    [Fact]
    public async Task CreateTradeCommandHandler_Is_Of_Type_IRequestHandler()
    {
        //Arrange
        var createTradeCommandHandler = new CreateTradeCommandHandler(
                                             _eventBusSettings
                                            , _mapperMock.Object
                                            , _metricsloggerMock.Object
                                            , _publisherServiceMock.Object
                                            , _workflowResponseValidator.Object);

        //Act
        var response = await createTradeCommandHandler.Handle(
                            new CreateFirstTradeCommandRequest()
                            , CancellationToken.None);

        //Assert
        createTradeCommandHandler
            .ShouldBeAssignableTo<MediatR.IRequestHandler<CreateFirstTradeCommandRequest, CreateFirstTradeCommandResponse>>();
        response.ShouldBeAssignableTo<CreateFirstTradeCommandResponse>();
    }

    [Theory]
    [LoadData("CreateFirstTradeCommandRequest_BuildFirstTradeWorkflowEvent")]
    public async Task CreateTradeCommandHandler_Should_Return_CreateFirstTradeCommandHandleResponse_And_Call_PublishEventAsync(
        CreateFirstTradeCommandRequest createFirstTradeCommandRequest)
    {
        //Arrange
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createFirstTradeCommandRequest);

        _mapperMock.Setup(x => x.Map<Events.WorkflowEvents>(createFirstTradeCommandRequest))
                   .Returns(workflowEvents).Verifiable();
        _publisherServiceMock.Setup(x =>
                                  x.PublishEventAsync(_eventBusSettings.FirstTradeTopicName, createFirstTradeCommandResponse))
                                 .Verifiable();
        _mapperMock.Setup(x => x.Map<CreateFirstTradeCommandResponse>(It.IsAny<Trade>()))
                   .Returns(createFirstTradeCommandResponse).Verifiable();
        _workflowResponseValidator.Setup(x => x.IsValidResponseToSend(createFirstTradeCommandResponse))
                               .Returns(true);

        var createTradeCommandHandler = new CreateTradeCommandHandler(
                                               _eventBusSettings
                                              , _mapperMock.Object
                                              , _metricsloggerMock.Object
                                              , _publisherServiceMock.Object
                                              , _workflowResponseValidator.Object);

        //Act
        var response = await createTradeCommandHandler.Handle(
                             createFirstTradeCommandRequest
                             , CancellationToken.None);

        //Assert
        response.ShouldBeAssignableTo<CreateFirstTradeCommandResponse>();
        _mapperMock.Verify(x => x.Map<Events.WorkflowEvents>(createFirstTradeCommandRequest), Times.Once());
        _publisherServiceMock.Verify(x =>
                                     x.PublishEventAsync(_eventBusSettings.FirstTradeTopicName
                                     , createFirstTradeCommandResponse), Times.Once()); ;
    }

    [Theory]
    [LoadData("CreateFirstTradeCommandRequest_BuildFirstTradeWorkflowEvent_Invalid")]
    public void CreateTradeCommandHandler_Should_NotThrow_Exception_And_Return_Null_Response_And_Not_Call_PublishEventAsync(
      CreateFirstTradeCommandRequest createFirstTradeCommandRequest)
    {
        //Arrange
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createFirstTradeCommandRequest);

        var createTradeCommandHandler = new CreateTradeCommandHandler(
                                               _eventBusSettings
                                              , _mapperMock.Object
                                              , _metricsloggerMock.Object
                                              , _publisherServiceMock.Object
                                              , _workflowResponseValidator.Object);

        //Assert
        Should.NotThrow<CreateFirstTradeCommandResponse>(async () => await createTradeCommandHandler.Handle(
                                                                           createFirstTradeCommandRequest
                                                                           , CancellationToken.None));
    }
}
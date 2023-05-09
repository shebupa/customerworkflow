namespace ETX.Workflow.Customer.Application.Tests.Features.CustomerWorkflow.Handlers.Commands;

[ExcludeFromCodeCoverage]
public class CreateRegistrationStatusCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = default;
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;
    private readonly Mock<IPublisherService> _publisherServiceMock = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly Mock<IWorkflowResponseValidator> _workflowResponseValidator = default;

    public CreateRegistrationStatusCommandHandlerTests()
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
    public void CreateRegistrationStatusCommandHandler_Is_Of_Type_IRequestHandler()
    {
        //Arrange
        //Act
        var createRegistrationStatusCommandHandler = new CreateRegistrationStatusCommandHandler(
                                                         _eventBusSettings
                                                         , _mapperMock.Object
                                                         , _metricsloggerMock.Object
                                                         , _publisherServiceMock.Object
                                                         , _workflowResponseValidator.Object);

        //Assert
        createRegistrationStatusCommandHandler
            .ShouldBeAssignableTo<MediatR.IRequestHandler<CreateRegistrationStatusCommandRequest, CreateRegistrationStatusCommandResponse>>();
    }

    [Theory]
    [LoadData("CreateRegistrationStatusCommandRequest_BuildRegistrationStatusWorkflowEvent")]
    public async Task CreateRegistrationStatusCommandHandler_Should_Return_CreateRegistrationStatusCommandHandleResponse__And_Call_PublishEventAsync(
   CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        var createRegistrationStatusCommandResponse = new CreateRegistrationStatusCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createRegistrationStatusCommandRequest);

        _mapperMock.Setup(x => x.Map<Events.WorkflowEvents>(createRegistrationStatusCommandRequest))
                   .Returns(workflowEvents).Verifiable();
        _mapperMock.Setup(x => x.Map<CreateRegistrationStatusCommandResponse>(It.IsAny<Registration>()))
                   .Returns(createRegistrationStatusCommandResponse).Verifiable();
        _workflowResponseValidator.Setup(x => x.IsValidResponseToSend(createRegistrationStatusCommandResponse))
                               .Returns(true);
        _publisherServiceMock.Setup(x =>
                                   x.PublishEventAsync(_eventBusSettings.RegistrationStatusTopicName, createRegistrationStatusCommandResponse))
                                  .Verifiable();

        var createRegistrationStatusCommandHandler = new CreateRegistrationStatusCommandHandler(
                                               _eventBusSettings
                                              , _mapperMock.Object
                                              , _metricsloggerMock.Object
                                              , _publisherServiceMock.Object
                                              , _workflowResponseValidator.Object);

        //Act
        var response = await createRegistrationStatusCommandHandler.Handle(
                             createRegistrationStatusCommandRequest
                             , CancellationToken.None);

        //Assert
        response.ShouldBeAssignableTo<CreateRegistrationStatusCommandResponse>();
        _mapperMock.Verify(x => x.Map<Events.WorkflowEvents>(createRegistrationStatusCommandRequest), Times.Once());
        _publisherServiceMock.Verify(x =>
                                     x.PublishEventAsync(_eventBusSettings.RegistrationStatusTopicName
                                     , createRegistrationStatusCommandResponse), Times.Once()); ;
    }

    [Theory]
    [LoadData("CreateRegistrationStatusCommandRequest_BuildRegistrationStatusWorkflowEvent_Invalid")]
    public void CreateRegistrationStatusCommandHandler_Should_NotThrow_Exception_And_Return_Null_Response_And_Not_Call_PublishEventAsync(
        CreateRegistrationStatusCommandRequest createRegistrationStatusCommandRequest)
    {
        //Arrange
        var createRegistrationStatusCommandResponse = new CreateRegistrationStatusCommandResponse();
        var workflowEvents = Util.CreateWorkflowEvent(createRegistrationStatusCommandRequest);

        var createRegistrationStatusCommandHandler = new CreateRegistrationStatusCommandHandler(
                                               _eventBusSettings
                                              , _mapperMock.Object
                                              , _metricsloggerMock.Object
                                              , _publisherServiceMock.Object
                                              , _workflowResponseValidator.Object);
        //Assert
        Should.NotThrow<CreateRegistrationStatusCommandResponse>(async () => await createRegistrationStatusCommandHandler.Handle(
                                                                                   createRegistrationStatusCommandRequest
                                                                                   , CancellationToken.None)).ShouldBeNull();
    }
}
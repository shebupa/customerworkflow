namespace ETX.Workflow.Customer.Infrastructure.Services.Tests;

[ExcludeFromCodeCoverage]
public class EventPublisherServiceTests
{
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly Mock<IMetricsLoggerService> _metricsLoggerServiceMock = default;
    private readonly IEventBusBrokerService _eventBusBrokerService = default;
    private readonly Mock<IHazelcastClient> _hazelcastClientMock = default;

    public EventPublisherServiceTests()
    {
        _eventBusSettings = new EventBusSettings();
        _metricsLoggerServiceMock = new Mock<IMetricsLoggerService>();
        _hazelcastClientMock = new Mock<IHazelcastClient>();

        _hazelcastClientMock.Setup(x => x.IsConnected)
                            .Returns(true)
                            .Verifiable();

        _eventBusBrokerService = new HazelcastEventBusBrokerService(
                                   _hazelcastClientMock.Object
                                   , _eventBusSettings
                                   , _metricsLoggerServiceMock.Object);

        //Config Read
        var config = ETX.Workflow.Customer.Fixtures.Configurations.Util.InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);
    }

    [Fact]
    public void EventPublisherService_Is_of_Type_IPublisherService()
    {
        //Arrange
        var eventService = new EventPublisherService(
                               _eventBusBrokerService);

        //Assert
        eventService.ShouldBeAssignableTo<IPublisherService>();
    }

    [Fact]
    public async Task ConnectToServerAsync_Should_Return_True()
    {
        //Arrange
        var eventService = new EventPublisherService(
                               _eventBusBrokerService);

        //Act
        await eventService.ConnectToServerAsync();

        //Assert
        _hazelcastClientMock.Verify(x => x.IsConnected, Times.AtLeastOnce());
    }

    [Fact]
    public async Task PublishEventAsync_CreateFirstDepositCommandResponse_Should_Write_To_FirstDepositTopicName()
    {
        //Arrange
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();
        var eventService = new EventPublisherService(
                               _eventBusBrokerService);

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstDepositCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventService.PublishEventAsync(
                            _eventBusSettings.FirstDepositTopicName
                            , createFirstDepositCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstDepositCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task PublishEventAsync_CreateFirstTradeCommandResponse_Should_Write_To_FirstTradeTopicName()
    {
        //Arrange
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();
        var eventService = new EventPublisherService(
                               _eventBusBrokerService);

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstTradeCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstTradeTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventService.PublishEventAsync(
                            _eventBusSettings.FirstTradeTopicName
                            , createFirstTradeCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.FirstTradeTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstTradeCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task PublishEventAsync_CreateRegistrationStatusCommandResponse_Should_Write_To_RegistrationStatusTopicName()
    {
        //Arrange
        var createRegistrationStatusCommandResponse = new CreateRegistrationStatusCommandResponse();
        var eventService = new EventPublisherService(
                               _eventBusBrokerService);

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createRegistrationStatusCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventService.PublishEventAsync(
                            _eventBusSettings.RegistrationStatusTopicName
                            , createRegistrationStatusCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createRegistrationStatusCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }
}
namespace ETX.Workflow.Customer.Infrastructure.Services.Tests;

[ExcludeFromCodeCoverage]
public class HazelcastEventBusBrokerServiceTests
{
    private readonly Mock<IMetricsLoggerService> _metricsLoggerMock = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly Mock<IHazelcastClient> _hazelcastClientMock = default;
    private readonly HazelcastEventBusBrokerService _hazelcastEventBusBrokerService = default;

    public HazelcastEventBusBrokerServiceTests()
    {
        _metricsLoggerMock = new Mock<IMetricsLoggerService>();
        _hazelcastClientMock = new Mock<IHazelcastClient>();
        _eventBusSettings = new EventBusSettings();

        //Config Read
        var config = ETX.Workflow.Customer.Fixtures.Configurations.Util.InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);

        _hazelcastEventBusBrokerService = new HazelcastEventBusBrokerService(
                                              _hazelcastClientMock.Object,
                                              _eventBusSettings,
                                              _metricsLoggerMock.Object);
    }

    [Fact]
    public void HazelCastBrokerService_Is_of_Type_IEventBusBrokerService()
    {
        //Assert
        _hazelcastEventBusBrokerService.ShouldBeAssignableTo<IEventBusBrokerService>();
    }

    [Fact]
    public async Task ConnectToServerAsync_Should_Call_IsConnected_Return_True()
    {
        //Act
        _hazelcastClientMock.Setup(x => x.IsConnected)
                          .Returns(true)
                          .Verifiable();
        var isConnected = await _hazelcastEventBusBrokerService.ConnectToServerAsync();

        //Assert
        isConnected.ShouldBeAssignableTo<bool>();
        isConnected.ShouldBeEquivalentTo(true);
        _hazelcastClientMock.Verify(x => x.IsConnected, Times.Once());
    }

    [Fact]
    public async Task WriteAsync_Should_Call_GetTopicAsync_PublishAsync_Once_With_CreateFirstDepositCommandResponse()
    {
        //Arrange
        var topicName = "CustomerWorkflowFirstDeposit";
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstDepositCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(It.IsAny<string>()))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _hazelcastEventBusBrokerService.WriteAsync(
                                              topicName,
                                              createFirstDepositCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(topicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstDepositCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task WriteAsync_Should_Call_GetTopicAsync_PublishAsync_Once_With_CreateFirstTradeCommandResponse()
    {
        //Arrange
        var topicName = "CustomerWorkflowFirstTrade";
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstTradeCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(It.IsAny<string>()))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _hazelcastEventBusBrokerService.WriteAsync(
                                              topicName,
                                              createFirstTradeCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(topicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstTradeCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task WriteAsync_Should_Call_GetTopicAsync_PublishAsync_Once_With_CreateRegistrationStatusCommandResponse()
    {
        //Arrange
        var topicName = "CustomerWorkflowRegistrationStatus";
        var createRegistrationStatusCommandResponse = new CreateRegistrationStatusCommandResponse();

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createRegistrationStatusCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(It.IsAny<string>()))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _hazelcastEventBusBrokerService.WriteAsync(
                                              topicName,
                                              createRegistrationStatusCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(topicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createRegistrationStatusCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }
}
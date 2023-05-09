namespace ETX.Workflow.Customer.Infrastructure.Tests.Services.Policies;

[ExcludeFromCodeCoverage]
public class HazelcastEventBusServicePolicyTests
{
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IEventBusServicePolicyFacade _eventBusServicePolicyFacade = default;
    private readonly Mock<IMetricsLoggerService> _metricsLoggerServiceMock = default;
    private readonly Mock<IHazelcastClient> _hazelcastClientMock = default;

    public HazelcastEventBusServicePolicyTests()
    {
        _eventBusSettings = new EventBusSettings();
        _metricsLoggerServiceMock = new Mock<IMetricsLoggerService>();
        _hazelcastClientMock = new Mock<IHazelcastClient>();

        _hazelcastClientMock.Setup(x => x.IsConnected)
                      .Returns(true)
                      .Verifiable();

        _eventBusServicePolicyFacade = new HazelcastEventBusServicePolicyFacade(
                                        _eventBusSettings
                                        , _hazelcastClientMock.Object
                                        , _metricsLoggerServiceMock.Object
                                        );

        //Config Read
        var config = InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);
    }

    [Fact]
    public void Should_Implement_IResilientPublishingService()
    {
        //Arrange
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                        _eventBusSettings
                                        , _metricsLoggerServiceMock.Object
                                        , _eventBusServicePolicyFacade);

        //Assert
        eventBusServicePolicy.ShouldBeAssignableTo<IEventBusBrokerService>();
    }

    [Fact]
    public void Should_Inherit_PolicyBase()
    {
        //Arrange
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                        _eventBusSettings
                                        , _metricsLoggerServiceMock.Object
                                        , _eventBusServicePolicyFacade);

        //Assert
        eventBusServicePolicy.ShouldBeAssignableTo<PolicyBase>();
    }

    [Fact]
    public async Task ConnectToServerAsync_Should_Return_True()
    {
        //Arrange
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                        _eventBusSettings
                                        , _metricsLoggerServiceMock.Object
                                        , _eventBusServicePolicyFacade);

        //Act
        var response = await eventBusServicePolicy.ConnectToServerAsync();

        //Assert
        response.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task WriteAsync_CreateFirstDepositCommandResponse_Should_Write_To_FirstDepositTopicName()
    {
        //Arrange
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                  _eventBusSettings
                                 , _metricsLoggerServiceMock.Object
                                 , _eventBusServicePolicyFacade);
        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstDepositCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventBusServicePolicy.WriteAsync(
                             _eventBusSettings.FirstDepositTopicName
                             , createFirstDepositCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstDepositCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task WriteAsync_CreateRegistrationStatusCommandResponse_Should_Write_To_FirstTradeTopicName()
    {
        //Arrange
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                  _eventBusSettings
                                 , _metricsLoggerServiceMock.Object
                                 , _eventBusServicePolicyFacade);
        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstTradeCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstTradeTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventBusServicePolicy.WriteAsync(
                             _eventBusSettings.FirstTradeTopicName
                             , createFirstTradeCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.FirstTradeTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstTradeCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task WriteAsync_CreateRegistrationStatusCommandResponse_Should_Write_To_RegistrationStatusTopicName()
    {
        //Arrange
        var createRegistrationStatusCommandResponse = new CreateRegistrationStatusCommandResponse();
        var eventBusServicePolicy = new HazelcastEventBusServicePolicy(
                                  _eventBusSettings
                                 , _metricsLoggerServiceMock.Object
                                 , _eventBusServicePolicyFacade);
        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createRegistrationStatusCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await eventBusServicePolicy.WriteAsync(
                             _eventBusSettings.RegistrationStatusTopicName
                             , createRegistrationStatusCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createRegistrationStatusCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    private IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }

    private (AsyncRetryPolicy, AsyncCircuitBreakerPolicy) Initialise()
    {
        var logMessage = "TestLog";

        return _eventBusServicePolicyFacade.Initialise(
                          (HttpResponseMessage, TimeSpan) => { _metricsLoggerServiceMock.Object.LogInfoMessage(logMessage); }
                          , () => { _metricsLoggerServiceMock.Object.LogInfoMessage(logMessage); }
                          , () => { _metricsLoggerServiceMock.Object.LogInfoMessage(logMessage); });
    }
}
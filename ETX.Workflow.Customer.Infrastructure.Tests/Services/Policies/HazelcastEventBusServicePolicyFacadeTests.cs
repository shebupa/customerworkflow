namespace ETX.Workflow.Customer.Infrastructure.Tests.Services.Policies;

[ExcludeFromCodeCoverage]
public class HazelcastEventBusServicePolicyFacadeTests
{
    private AsyncRetryPolicy _retryPolicy = default;
    private AsyncCircuitBreakerPolicy _circuitBreakerPolicy = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IEventBusServicePolicyFacade _eventBusServicePolicyFacade = default;
    private readonly Mock<IMetricsLoggerService> _metricsLoggerServiceMock = default;
    private readonly Mock<IHazelcastClient> _hazelcastClientMock = default;

    public HazelcastEventBusServicePolicyFacadeTests()
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
    public void Should_Implement_IEventBusServicePolicyFacade()
    {
        //Assert
        _eventBusServicePolicyFacade.ShouldBeAssignableTo<IEventBusServicePolicyFacade>();
    }

    [Fact]
    public async Task ConnectToServerAsync_Should_Return_True()
    {
        //Arrange
        var pollyConfig = Initialise();

        _retryPolicy = pollyConfig.Item1;
        _circuitBreakerPolicy = pollyConfig.Item2;

        //Act
        var response = await _eventBusServicePolicyFacade.ConnectToServerAsync(
                            _retryPolicy
                            , _circuitBreakerPolicy);

        //Assert
        response.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task WriteAsync_CreateFirstDepositCommandResponse_Should_Write_To_FirstDepositTopicName()
    {
        //Arrange
        var createFirstDepositCommandResponse = new CreateFirstDepositCommandResponse();
        var pollyConfig = Initialise();

        _retryPolicy = pollyConfig.Item1;
        _circuitBreakerPolicy = pollyConfig.Item2;

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstDepositCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _eventBusServicePolicyFacade.WriteAsync(
                             _retryPolicy
                             , _circuitBreakerPolicy
                             , _eventBusSettings.FirstDepositTopicName
                             , createFirstDepositCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.FirstDepositTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createFirstDepositCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task WriteAsync_CreateFirstTradeCommandResponse_Should_Write_To_FirstTradeTopicName()
    {
        //Arrange
        var createFirstTradeCommandResponse = new CreateFirstTradeCommandResponse();
        var pollyConfig = Initialise();

        _retryPolicy = pollyConfig.Item1;
        _circuitBreakerPolicy = pollyConfig.Item2;

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createFirstTradeCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.FirstTradeTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _eventBusServicePolicyFacade.WriteAsync(
                             _retryPolicy
                             , _circuitBreakerPolicy
                             , _eventBusSettings.FirstTradeTopicName
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
        var pollyConfig = Initialise();

        _retryPolicy = pollyConfig.Item1;
        _circuitBreakerPolicy = pollyConfig.Item2;

        var topicMock = new Mock<IHTopic<object>>();
        topicMock.Setup(x => x.PublishAsync(createRegistrationStatusCommandResponse)).Verifiable();
        topicMock.Setup(x => x.DisposeAsync()).Verifiable();

        _hazelcastClientMock.Setup(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName))
                            .ReturnsAsync(topicMock.Object)
                            .Verifiable();

        //Act
        await _eventBusServicePolicyFacade.WriteAsync(
                             _retryPolicy
                             , _circuitBreakerPolicy
                             , _eventBusSettings.RegistrationStatusTopicName
                             , createRegistrationStatusCommandResponse);

        //Assert
        _hazelcastClientMock.Verify(x => x.GetTopicAsync<object>(_eventBusSettings.RegistrationStatusTopicName), Times.Once());
        topicMock.Verify(x => x.PublishAsync(createRegistrationStatusCommandResponse), Times.Once());
        topicMock.Verify(x => x.DisposeAsync(), Times.Once());
    }

    [Fact]
    public async Task Initialise_Should_Accept_OnBreak_OnReset_OnHalfOpen_Policy_Functions()
    {
        //Arrange
        //Act
        var response = Initialise();

        //Assert
        response.ShouldBeAssignableTo<(AsyncRetryPolicy, AsyncCircuitBreakerPolicy)>();
        response.Item1.ShouldNotBeNull();
        response.Item2.ShouldNotBeNull();
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
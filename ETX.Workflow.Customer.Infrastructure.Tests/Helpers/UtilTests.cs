namespace ETX.Workflow.Customer.Infrastructure.Tests.Helpers;

public class UtilTests
{
    public EventBusSettings _eventBusSettings = new EventBusSettings();
    public Mock<IMetricsLoggerService> _mettricsLoggerServiceMock;

    public UtilTests()
    {
        _mettricsLoggerServiceMock = new Mock<IMetricsLoggerService>();

        //Config Read
        var config = ETX.Workflow.Customer.Fixtures.Configurations.Util.InitConfiguration();
        config.Bind("CustomerWorkflowEventServiceBus", _eventBusSettings);
    }

    [Fact]
    public void ConfigureHazelcastNetwork_Should_Return_NotNull()
    {
        //Arrange
        var util = new Util();

        //Act
        var networkDetails = util.ConfigureHazelcastNetwork(
                            _eventBusSettings.HostName,
                            _eventBusSettings.Port);

        //Assert
        networkDetails.ShouldNotBeNull();
        networkDetails.First().ShouldContain(EventBusConstants.HAZELCAST_NETWORK_ADDRESS);
    }

    public void ConnectToHazelcast_Should_Return_NotNull()
    {
        //Arrange
        var hazelcastSerialiser = new HazelcastSerialiser(_mettricsLoggerServiceMock.Object);
        var hazecastClient = new Mock<IHazelcastClient>();
        var util = new Util();
        //var client = util.ConnectToHazelcast(hazelcastSerialiser, _eventBusSettings, _mettricsLoggerServiceMock.Object);
        //var util = new Mock<Util>();
        /*util.Setup(x => x.ConnectToHazelcast(
                                hazelcastSerialiser,
                                _eventBusSettings,
                                _mettricsLoggerServiceMock.Object))
             .Returns(() => hazecastClient.Object);

        //Act
        util.Object.ConnectToHazelcast(
                    hazelcastSerialiser
                    , _eventBusSettings
                    , _mettricsLoggerServiceMock.Object);

        //Assert
        util.Verify(_ => _.ConnectToHazelcast(
                         hazelcastSerialiser
                         , _eventBusSettings
                         , _mettricsLoggerServiceMock.Object)
                         , Times.AtLeastOnce());*/
    }
}
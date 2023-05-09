namespace ETX.Workflow.Customer.Service.Tests;

[ExcludeFromCodeCoverage]
public class CustomerRegistrationBackgroudServiceTests
{
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;
    private readonly Mock<IMetricsLoggerService> _metricsLoggerServiceMock = default;
    private readonly Mock<IRequestHandler> _requestHandlerMock = default;
    private readonly Mock<IServiceProvider> _serviceProvider = default;

    public CustomerRegistrationBackgroudServiceTests()
    {
        _metricsLoggerServiceMock = new Mock<IMetricsLoggerService>();
        _requestHandlerMock = new Mock<IRequestHandler>();
        _serviceProvider = new Mock<IServiceProvider>();

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceItdbSettings);
    }

    [Fact]
    public void IsExtendsBackgroundService()
    {
        //Arrange
        var customerRegistrationBackgroudService = new CustomerRegistrationBackgroudService(
                                                        _dataSourceItdbSettings
                                                        , _metricsLoggerServiceMock.Object
                                                        , _serviceProvider.Object
                                                        );

        //Assert
        customerRegistrationBackgroudService
            .ShouldBeAssignableTo<BackgroundService>();
    }
}
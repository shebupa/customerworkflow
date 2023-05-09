namespace ETX.Workflow.Customer.Service.Tests;

[ExcludeFromCodeCoverage]
public class CustomerTradeBackgroudServiceTests
{
    private readonly DataSourceTitanSettings _dataSourceTitanSettings = default;
    private readonly Mock<IMetricsLoggerService> _metricsLoggerServiceMock = default;
    private readonly Mock<IRequestHandler> _requestHandlerMock = default;
    private readonly Mock<IServiceProvider> _serviceProvider = default;

    public CustomerTradeBackgroudServiceTests()
    {
        _metricsLoggerServiceMock = new Mock<IMetricsLoggerService>();
        _requestHandlerMock = new Mock<IRequestHandler>();
        _serviceProvider = new Mock<IServiceProvider>();

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceTitanSettings);
    }

    [Fact]
    public void IsExtendsBackgroundService()
    {
        //Arrange
        var customerRegistrationBackgroudService = new CustomerTradeBackgroudService(
                                                        _dataSourceTitanSettings
                                                        , _metricsLoggerServiceMock.Object
                                                        , _serviceProvider.Object
                                                        );

        //Assert
        customerRegistrationBackgroudService
            .ShouldBeAssignableTo<BackgroundService>();
    }
}
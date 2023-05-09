namespace ETX.Workflow.Customer.Service;

public class CustomerRegistrationBackgroudService : BackgroundService
{
    private readonly IMetricsLoggerService _logger = default;
    private readonly IServiceProvider _services = default;
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;
    private Timer _timer = default;

    public CustomerRegistrationBackgroudService(
         DataSourceItdbSettings dataSourceItdbSettings
         , IMetricsLoggerService logger
         , IServiceProvider services
         )
    {
        _dataSourceItdbSettings = dataSourceItdbSettings;
        _logger = logger;
        _services = services;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override Task StartAsync(
            CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(
            CancellationToken cancellationToken)
    {
        _logger.LogErrorMessage($"CustomerWorkflow service is Stopping");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (Double.TryParse(_dataSourceItdbSettings.RegistrationPollingInterval, out double registrationStatusPollingInterval))
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(registrationStatusPollingInterval));
        }
    }

    private void DoWork(object state)
    {
        Task.Run(async () =>
        {
            using var scope = _services.CreateScope();
            var requestHandler = scope.ServiceProvider.GetRequiredService<IRequestHandler>();
            await requestHandler.HandleCommandsAsync(WorkflowEventSource.DbItdb);
        });
    }
}
//using System.Globalization

namespace ETX.Workflow.Customer.Service;

public class CustomerTradeBackgroudService : BackgroundService
{
    private readonly IMetricsLoggerService _logger = default;
    private readonly IServiceProvider _services = default;
    private readonly DataSourceTitanSettings _dataSourceTitanSettings = default;
    private Timer _timer = default;

    public CustomerTradeBackgroudService(
         DataSourceTitanSettings dataSourceTitanSettings
         , IMetricsLoggerService logger
         , IServiceProvider services
         )
    {
        _dataSourceTitanSettings = dataSourceTitanSettings;
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
        if (Double.TryParse(_dataSourceTitanSettings.FirstTradePollingInterval, out double firstTradePollingInterval))
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(firstTradePollingInterval));
        }
    }

    private void DoWork(object state)
    {
        Task.Run(async () =>
        {
            using var scope = _services.CreateScope();
            var requestHandler = scope.ServiceProvider.GetRequiredService<IRequestHandler>();
            await requestHandler.HandleCommandsAsync(WorkflowEventSource.DbTitan);
        });
    }
}
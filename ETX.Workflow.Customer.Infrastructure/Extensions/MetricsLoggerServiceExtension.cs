namespace ETX.Workflow.Customer.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class MetricsLoggerServiceExtension
{
    public static void AddMetricsLoggerService(this
          IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(LoggerFactory.GetInstance());
        services.AddSingleton<IMetricsLoggerService, MetricsLoggerService>();
    }
}
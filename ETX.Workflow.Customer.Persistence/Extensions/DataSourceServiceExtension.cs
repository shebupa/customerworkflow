namespace ETX.Workflow.Customer.Persistence.Extensions;

[ExcludeFromCodeCoverage]
public static class DataSourceServiceExtension
{
    public static void AddSqlServerContext(
         this IServiceCollection services,
         IConfiguration configuration)
    {
        var dataSourceItdbSettings = new DataSourceItdbSettings();
        configuration.Bind("ConnectionStrings", dataSourceItdbSettings);
        configuration.Bind("DbCustomerWorkflowPolling", dataSourceItdbSettings);
        services.AddDbContext<WorkflowItdbContext>(options =>
            options.UseSqlServer(dataSourceItdbSettings.SqlConnectionItdb));

        var dataSourceTitanSettings = new DataSourceTitanSettings();
        configuration.Bind("ConnectionStrings", dataSourceTitanSettings);
        configuration.Bind("DbCustomerWorkflowPolling", dataSourceTitanSettings);
        services.AddDbContext<WorkflowTitanContext>(options =>
            options.UseSqlServer(dataSourceTitanSettings.SqlConnectionTitan));

        services.AddSingleton(dataSourceItdbSettings);
        services.AddSingleton(dataSourceTitanSettings);
    }
}
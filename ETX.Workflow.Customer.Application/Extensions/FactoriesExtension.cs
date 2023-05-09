namespace ETX.Workflow.Customer.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class FactoriesExtension
{
    public static void AddApplicationServiceFactories(this
         IServiceCollection services
         )
    {
        services.AddScoped<WorkflowItdbDbSource>();
        services.AddScoped<WorkflowTitanDbSource>();

        services.AddScoped<Func<WorkflowEventSource, IWorkflowSource>>(serviceProvider => key =>
        {
            switch (key)
            {
                case WorkflowEventSource.DbItdb:
                    return serviceProvider.GetService<WorkflowItdbDbSource>();

                case WorkflowEventSource.DbTitan:
                    return serviceProvider.GetService<WorkflowTitanDbSource>();

                default:
                    throw new KeyNotFoundException();
            }
        });
    }
}
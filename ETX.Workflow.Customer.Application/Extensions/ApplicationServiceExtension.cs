namespace ETX.Workflow.Customer.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationServiceExtension
{
    public static void AddApplicationService(
         this IServiceCollection services,
         IConfiguration configuration)
    {
        services.AddScoped<IWorkflowResponseValidator, WorkflowResponseValidator>();
        services.AddScoped<IRequestHandler, RequestHandler>();
        services.AddSingleton<IWorkflowBuilder, WorkflowBuilder>();
    }
}
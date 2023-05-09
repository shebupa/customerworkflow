namespace ETX.Workflow.Customer.Persistence.Extensions;

[ExcludeFromCodeCoverage]
public static class FactoriesExtension
{
    public static void AddPersistanceFactories(this
         IServiceCollection services
         )
    {
        AddRepositoryFactories(services);
        IRepositoryManagerFactory repositoryManagerFactory = new RepositoryManagerFactory();
        var itdbContext = services
                          .BuildServiceProvider()
                          .GetService<WorkflowItdbContext>();

        var titanContext = services
                           .BuildServiceProvider()
                           .GetService<WorkflowTitanContext>();

        var customerWorkflowEventsItdbRepository = services
                                                   .BuildServiceProvider()
                                                   .GetService<WorkflowEventsItdbRepository>();

        var customerWorkflowEventsTitanRepository = services
                                                   .BuildServiceProvider()
                                                   .GetService<WorkflowEventsTitanRepository>();

        IRepositoryManager repositoryManagerItdb = new RepositoryManagerItdb(
                                                       itdbContext
                                                       , customerWorkflowEventsItdbRepository);
        IRepositoryManager repositoryManagerTitan = new RepositoryManagerTitan(
                                                        titanContext
                                                        , customerWorkflowEventsTitanRepository);

        //Register
        repositoryManagerFactory.Register(DataSource.Itdb, repositoryManagerItdb);
        repositoryManagerFactory.Register(DataSource.Titan, repositoryManagerTitan);

        services.AddScoped<IRepositoryManagerFactory>(services => repositoryManagerFactory);
    }

    private static void AddRepositoryFactories(this
         IServiceCollection services
         )
    {
        var itdbContext = services
                          .BuildServiceProvider()
                          .GetService<WorkflowItdbContext>();

        var titanContext = services
                         .BuildServiceProvider()
                         .GetService<WorkflowTitanContext>();

        var dataSourceItdbSettings = services
                         .BuildServiceProvider()
                         .GetService<DataSourceItdbSettings>();

        var dataSourceTitanSettings = services
                         .BuildServiceProvider()
                         .GetService<DataSourceTitanSettings>();

        services.AddScoped((x) => new WorkflowEventsItdbRepository(itdbContext, dataSourceItdbSettings));
        services.AddScoped((x) => new WorkflowEventsTitanRepository(titanContext, dataSourceTitanSettings));
    }
}
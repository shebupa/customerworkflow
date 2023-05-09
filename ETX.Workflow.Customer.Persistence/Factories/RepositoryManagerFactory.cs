namespace ETX.Workflow.Customer.Persistence.Factories;

public class RepositoryManagerFactory : IRepositoryManagerFactory
{
    private readonly Dictionary<DataSource, IRepositoryManager> _repositoryManagers
       = new Dictionary<DataSource, IRepositoryManager>();

    public RepositoryManagerFactory()
    {
    }

    public IRepositoryManager Create(DataSource dataSource)
    {
        return _repositoryManagers[dataSource];
    }

    public void Register(
        DataSource dataSource
        , IRepositoryManager repositoryManager)
    {
        _repositoryManagers.Add(dataSource, repositoryManager);
    }
}
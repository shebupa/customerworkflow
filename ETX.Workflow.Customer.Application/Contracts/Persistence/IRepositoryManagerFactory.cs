namespace ETX.Workflow.Customer.Application.Contracts.Persistence;

public interface IRepositoryManagerFactory
{
    void Register(
         DataSource dataSource
         , IRepositoryManager repositoryManager);

    IRepositoryManager Create(DataSource dataSource);
}
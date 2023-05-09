namespace ETX.Workflow.Customer.Persistence.Tests.Factories;

[ExcludeFromCodeCoverage]
public class RepositoryManagerFactoryTests
{
    private readonly Mock<IWorkflowEventsRepository> _customerWorkflowEventsRepositoryMock = default;
    private readonly WorkflowItdbContextTest _itdbContext = default;
    private readonly WorkflowTitanContextTest _titanContext = default;

    public RepositoryManagerFactoryTests()
    {
        _itdbContext = new WorkflowContextFactory<WorkflowItdbContextTest>()
                           .WorkflowContextInstance;
        _titanContext = new WorkflowContextFactory<WorkflowTitanContextTest>()
                           .WorkflowContextInstance;

        _customerWorkflowEventsRepositoryMock = new Mock<IWorkflowEventsRepository>();
    }

    [Fact]
    public void Is_Type_IRepositoryManagerFactory()
    {
        //Arrange
        var _repositoryManagerFactory = new RepositoryManagerFactory();

        //Assert
        _repositoryManagerFactory.ShouldBeAssignableTo<IRepositoryManagerFactory>();
    }

    [Fact]
    public void Register_Should_Accept_DataSource_Type_And_Instance()
    {
        //Arrange
        var _repositoryManagerFactory = new RepositoryManagerFactory();

        //Act
        _repositoryManagerFactory.Register(DataSource.Itdb, new RepositoryManagerItdb(
                                                               _itdbContext
                                                                , _customerWorkflowEventsRepositoryMock.Object));
    }

    [Fact]
    public void Create_Should_Return_RepositoryManagerInstance_For_DataSource()
    {
        //Arrange
        var _repositoryManagerFactory = new RepositoryManagerFactory();
        _repositoryManagerFactory.Register(DataSource.Itdb, new RepositoryManagerItdb(
                                                               _itdbContext
                                                                , _customerWorkflowEventsRepositoryMock.Object));
        //Act
        var repositoryManager = _repositoryManagerFactory.Create(DataSource.Itdb);

        //Assert
        repositoryManager.ShouldBeAssignableTo<RepositoryManagerItdb>();
    }
}
namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class RepositoryManagerTitanTests :
    IClassFixture<WorkflowContextFactory<WorkflowTitanContextTest>>
{
    private readonly IWorkflowEventsRepository _workflowEventsRepository = default;
    private readonly WorkflowTitanContextTest _dbcontext = default;
    private readonly IRepositoryManager _repositoryManager = default;
    private readonly DataSourceTitanSettings _dataSourceTitanSettings = default;

    public RepositoryManagerTitanTests(
                WorkflowContextFactory<WorkflowTitanContextTest> dbcontext)
    {
        //composition root
        _dbcontext = dbcontext.WorkflowContextInstance;

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceTitanSettings);

        _workflowEventsRepository = new WorkflowEventsTitanRepositoryFake(
                                                _dbcontext
                                                , _dataSourceTitanSettings);

        _repositoryManager = new RepositoryManagerTitan(_dbcontext, _workflowEventsRepository);
    }

    [Fact]
    public void WorkflowEventsRepository_Should_Be_Of_Type_IWorkflowEventsRepository()
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;

        //Assert
        workflowEventsRepository.ShouldBeAssignableTo<IWorkflowEventsRepository>();
    }

    [Fact]
    public void WorkflowEventsRepository_Should_Return_NotNull()
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;

        //Assert
        Assert.NotNull(workflowEventsRepository);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTrade")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents(
              CreateWorkflowRequest request)
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }
}
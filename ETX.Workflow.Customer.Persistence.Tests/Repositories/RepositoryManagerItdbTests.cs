namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class RepositoryManagerItdbTests :
    IClassFixture<WorkflowContextFactory<WorkflowItdbContextTest>>
{
    private readonly IWorkflowEventsRepository _workflowEventsRepository = default;
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;

    private readonly WorkflowContextFactory<WorkflowItdbContextTest>
        _workflowContextFactory = default;

    private readonly WorkflowItdbContextTest _dbcontext = default;
    private readonly IRepositoryManager _repositoryManager = default;

    public RepositoryManagerItdbTests(
                WorkflowContextFactory<WorkflowItdbContextTest> customerWorkflowContextFactory)
    {
        //composition root
        _workflowContextFactory = customerWorkflowContextFactory;
        _dbcontext = customerWorkflowContextFactory.WorkflowContextInstance;
        _workflowContextFactory.CreateDatabase();
        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceItdbSettings);

        _workflowEventsRepository = new WorkflowEventsItdbRepositoryFake(
                                                _dbcontext
                                                , _dataSourceItdbSettings);
        _repositoryManager = new RepositoryManagerItdb(_dbcontext, _workflowEventsRepository);
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
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents_Demo(
              CreateWorkflowRequest request)
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.Where(x => x.EventTypeId == 1)
                      .First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 1)
                      .First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Join")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents_Join(
             CreateWorkflowRequest request)
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.Where(x => x.EventTypeId == 2)
                       .First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 2)
                       .First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Completed")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents_Completed(
             CreateWorkflowRequest request)
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.Where(x => x.EventTypeId == 3)
                       .First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 3)
                       .First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Authorised")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents_Authorised(
             CreateWorkflowRequest request)

    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.Where(x => x.EventTypeId == 4)
                      .First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 4)
                       .First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstDepositWorkflow")]
    public async Task WorkflowEventsAsync_Should_Return_CustomerWorkflowEvents_FirstDeposit(
            CreateWorkflowRequest request)
    {
        //Act
        var workflowEventsRepository = _repositoryManager.WorkflowEventsRepository;
        var workflowEvents = await workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        workflowEvents.ShouldNotBeNull();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.Where(x => x.EventTypeId == 5)
                       .First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 5)
                       .First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }
}
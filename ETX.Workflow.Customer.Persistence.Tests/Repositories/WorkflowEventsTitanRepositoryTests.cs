namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class WorkflowEventsTitanRepositoryTests
     : WorkflowEventsTitanRepository
     , IClassFixture<WorkflowContextFactory<WorkflowTitanContextTest>>
{
    private readonly WorkflowTitanContextTest _dbContext = default;
    private readonly DataSourceItdbSettings _dataSourceTitanSettings = default;
    private readonly WorkflowContextFactory<WorkflowTitanContextTest> _contextFactory = default;
    private WorkflowEventsTitanRepositoryTests _workflowEventsRepository = default;

    public WorkflowEventsTitanRepositoryTests(
           WorkflowContextFactory<WorkflowTitanContextTest> contextFactory) :
                base(contextFactory.WorkflowContextInstance, new DataSourceTitanSettings())
    {
        _contextFactory = contextFactory;
        _dbContext = contextFactory.WorkflowContextInstance;

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceTitanSettings);
    }

    [Fact]
    public void Is_Implement_ICustomerWorkflowEventsRepository()
    {
        //Arrange
        var customerWorkflowEventsRepository = new WorkflowEventsTitanRepositoryTests(
                                                   _contextFactory);

        //Assert
        customerWorkflowEventsRepository.ShouldBeAssignableTo<IWorkflowEventsRepository>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTradeWorkflow")]
    public async Task GetWorkflowEvents_Should_Return_CreateWorkflowEvents_List(
        CustomerWorkflowEvents customerWorkflowEvents)
    {
        //Arrange
        _workflowEventsRepository = new WorkflowEventsTitanRepositoryTests(
                                                   _contextFactory);

        //Act
        var workflowEvents = await _workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        _workflowEventsRepository.ShouldBeAssignableTo<IWorkflowEventsRepository>();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.ShouldNotBeNull();
        workflowEvents.First().Id.ShouldBeEquivalentTo(customerWorkflowEvents.Id.ToString().ToUpper());
        workflowEvents.First().EventTypeId.ShouldBeEquivalentTo(customerWorkflowEvents.EventTypeId);
    }

    public new async Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        var ret = await FindByConditionAsync(QueryConstants.QUERY_SELECT_CUSTOMERWORKFLOWEVENTS
                                             , default);
        return ret;
    }

    public new async Task<int> UpdateWorkflowEventsProcessingStatus(
        QueryCondition queryCondition)
    {
        var ret = await UpdateByConditionAsync(QueryConstants.QUERY_UPDATE_WITH_WHERE
                                             , queryCondition
                                             , null);
        return ret;
    }
}
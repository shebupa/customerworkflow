namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class WorkflowEventsItdbRepositoryTests
     : WorkflowEventsItdbRepository
     , IClassFixture<WorkflowContextFactory<WorkflowItdbContextTest>>
{
    private readonly WorkflowItdbContextTest _dbContext = default;
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;
    private readonly WorkflowContextFactory<WorkflowItdbContextTest> _contextFactory = default;
    private WorkflowEventsItdbRepositoryTests _workflowEventsRepository = default;

    public WorkflowEventsItdbRepositoryTests(
           WorkflowContextFactory<WorkflowItdbContextTest> contextFactory) :
                base(contextFactory.WorkflowContextInstance, new DataSourceItdbSettings())
    {
        _contextFactory = contextFactory;
        _contextFactory.CreateDatabase();
        _dbContext = contextFactory.WorkflowContextInstance;

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceItdbSettings);
    }

    [Fact]
    public void Is_Implement_IWorkflowEventsRepository()
    {
        //Arrange
        _workflowEventsRepository = new WorkflowEventsItdbRepositoryTests(
                                                _contextFactory);

        //Assert
        _workflowEventsRepository.ShouldBeAssignableTo<IWorkflowEventsRepository>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task GetWorkflowEvents_Should_Return_CreateWorkflowEvents_List(
        CustomerWorkflowEvents customerWorkflowEvents)
    {
        //Arrange
        _workflowEventsRepository = new WorkflowEventsItdbRepositoryTests(
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

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task GetWorkflowEvents_Should_Return_Events_Piror_To_Ten_Minutes(
       CustomerWorkflowEvents customerWorkflowEvents)
    {
        //Arrange
        _workflowEventsRepository = new WorkflowEventsItdbRepositoryTests(
                                                   _contextFactory);

        //Act
        var workflowEvents = await _workflowEventsRepository.GetWorkflowEventsAsync();

        //Assert
        _workflowEventsRepository.ShouldBeAssignableTo<IWorkflowEventsRepository>();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.ShouldNotBeNull();
        workflowEvents.Count().ShouldBe(5);
        var result = workflowEvents.ToList().SingleOrDefault(x => x.CustomerId.Equals("29880"));
        result.ShouldBeNull();
    }


    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo_Update")]
    public async Task UpdateByConditionAsync_Should_Return_Updated_Count_Of_One_For_Demo(
        CustomerWorkflowEvents customerWorkflowEvents)
    {
        //Arrange
        _workflowEventsRepository = new WorkflowEventsItdbRepositoryTests(
                                                _contextFactory);

        //Act
        var queryCondition = Util.BuildQueryCondition("InProgress", customerWorkflowEvents.Id.ToString());
        var result = await _workflowEventsRepository
                           .UpdateWorkflowEventsProcessingStatusAsync(queryCondition);

        //Assert
        result.ShouldBeAssignableTo<int>();
    }

    public new async Task<IEnumerable<CustomerWorkflowEvents>> GetWorkflowEventsAsync()
    {
        var ret = await FindByConditionAsync(QueryConstants.QUERY_SELECT_CUSTOMERWORKFLOWEVENTS
                                             , default);
        return ret;
    }

    public new async Task<int> UpdateWorkflowEventsProcessingStatusAsync(
        QueryCondition queryCondition)
    {
        var ret = await UpdateByConditionAsync(QueryConstants.QUERY_UPDATE_WITH_WHERE
                                             , queryCondition
                                             , null);
        return ret;
    }
}
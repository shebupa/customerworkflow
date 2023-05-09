namespace ETX.Workflow.Customer.Application.Tests;

public class WorkFlowItdbDbSourceTests
     : IClassFixture<WorkflowContextFactory<WorkflowItdbContextTest>>

{
    private readonly Mock<IRepositoryManagerFactory> _repositoryManagerFactoryMock = default;
    private readonly WorkflowItdbContextTest _dbContext = default;
    private readonly DataSourceItdbSettings _dataSourceItdbSettings = default;

    private WorkflowContextFactory<WorkflowItdbContextTest> _workflowContextFactory = default;

    public WorkFlowItdbDbSourceTests(
        WorkflowContextFactory<WorkflowItdbContextTest> workflowContextFactory)
    {
        _workflowContextFactory = workflowContextFactory;
        _dbContext = workflowContextFactory.WorkflowContextInstance;
        _repositoryManagerFactoryMock = new Mock<IRepositoryManagerFactory>();

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceItdbSettings);

        _repositoryManagerFactoryMock
            .Setup(x => x.Create(DataSource.Itdb))
            .Returns(new RepositoryManagerItdb(_dbContext
                         , new WorkflowEventsItdbRepositoryFake(
                             _dbContext
                             , _dataSourceItdbSettings)));
    }

    [Fact]
    public void Is_Implement_ICustomerWorkFlowSource()
    {
        //Arrange
        var customerWorkFlowSource = new WorkflowItdbDbSource(
                                         _repositoryManagerFactoryMock.Object);

        //Assert
        customerWorkFlowSource.ShouldBeAssignableTo<IWorkflowSource>();
        customerWorkFlowSource.ShouldBeAssignableTo<WorkflowWithProcessingStatus>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task GetWorkflowEvents_On_Itdb_Should_Return_CustomerWorkflowEvents_List_And_Update_Status_InProgress(
                   CreateWorkflowRequest request)
    {
        //Arrange
        var customerWorkFlowSource = new WorkflowItdbDbSource(_repositoryManagerFactoryMock.Object);

        //Act
        var workflowEvents = await customerWorkFlowSource.GetWorkflowEventsAsync();

        //Assert
        customerWorkFlowSource.ShouldBeAssignableTo<IWorkflowSource>();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.ShouldNotBeNull();
        workflowEvents.Where(x => x.EventTypeId == 1).First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 1).First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo")]
    public async Task GetWorkflowEvents_On_Itdb_Should_Return_CustomerWorkflowEvents_List_And_Update_Status_Completed(
                   CreateWorkflowRequest request)
    {
        //Arrange
        var responseList = new List<ResponseBase>();
        var customerWorkFlowSource = new WorkflowItdbDbSource(_repositoryManagerFactoryMock.Object);

        //Act
        var workflowEvents = await customerWorkFlowSource.GetWorkflowEventsAsync();
        workflowEvents.ForEach(x => x.Status = "Completed");
        workflowEvents.ForEach(x => responseList.Add(new ResponseBase { CustomerWorkflowId = x.Id, CustomerId = x.CustomerId }));
        var countOfUpdate = await customerWorkFlowSource.UpdateWorkflowEventsAsync(
                                                         QueryConditionConstants.COMPLETED
                                                         , responseList);

        //Assert
        customerWorkFlowSource.ShouldBeAssignableTo<IWorkflowSource>();
        workflowEvents.ShouldNotBeNull();
        workflowEvents.Where(x => x.EventTypeId == 1).First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 1).First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
        countOfUpdate.ShouldBeEquivalentTo(5);
    }
}
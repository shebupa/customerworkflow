namespace ETX.Workflow.Customer.Application.Tests;

public class WorkFlowTitanDbSourceTests
     : IClassFixture<WorkflowContextFactory<WorkflowTitanContextTest>>
{
    private readonly Mock<IRepositoryManagerFactory> _repositoryManagerFactoryMock = default;
    private readonly WorkflowTitanContextTest _dbContext = default;
    private readonly DataSourceTitanSettings _dataSourceTitanSettings = default;

    private WorkflowContextFactory<WorkflowTitanContextTest> _workflowContextFactory = default;

    public WorkFlowTitanDbSourceTests(
        WorkflowContextFactory<WorkflowTitanContextTest> workflowContextFactory)
    {
        _workflowContextFactory = workflowContextFactory;
        _dbContext = workflowContextFactory.WorkflowContextInstance;
        _repositoryManagerFactoryMock = new Mock<IRepositoryManagerFactory>();

        //Config Read
        var config = Util.InitConfiguration();
        config.Bind("ConnectionStrings", _dataSourceTitanSettings);

        _repositoryManagerFactoryMock
            .Setup(x => x.Create(DataSource.Titan))
            .Returns(new RepositoryManagerTitan(_dbContext
                         , new WorkflowEventsTitanRepositoryFake(
                             _dbContext
                             , _dataSourceTitanSettings)));
    }

    [Fact]
    public void Is_Implement_IWorkFlowSource()
    {
        //Arrange
        var customerWorkFlowSource = new WorkflowTitanDbSource(
                                         _repositoryManagerFactoryMock.Object);

        //Assert
        customerWorkFlowSource.ShouldBeAssignableTo<IWorkflowSource>();
        customerWorkFlowSource.ShouldBeAssignableTo<WorkflowWithProcessingStatus>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTradeWorkflow")]
    public async Task GetWorkflowEvents_On_Titan_Should_Return_CustomerWorkflowEvents_List_And_Update_Status_InProgress(
                   CreateWorkflowRequest request)
    {
        //Arrange
        var customerWorkFlowSource = new WorkflowTitanDbSource(_repositoryManagerFactoryMock.Object);

        //Act
        var workflowEvents = await customerWorkFlowSource.GetWorkflowEventsAsync();

        //Assert
        customerWorkFlowSource.ShouldBeAssignableTo<IWorkflowSource>();
        workflowEvents.ShouldBeAssignableTo<List<CustomerWorkflowEvents>>();
        workflowEvents.ShouldNotBeNull();
        workflowEvents.Where(x => x.EventTypeId == 6).First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 6).First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTradeWorkflow")]
    public async Task GetWorkflowEvents_On_Titan_Should_Return_CustomerWorkflowEvents_List_And_Update_Status_Completed(
                   CreateWorkflowRequest request)
    {
        //Arrange
        var responseList = new List<ResponseBase>();
        var customerWorkFlowSource = new WorkflowTitanDbSource(_repositoryManagerFactoryMock.Object);

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
        workflowEvents.Where(x => x.EventTypeId == 6).First().Id.ShouldBeEquivalentTo(request.Id.ToString().ToUpper());
        workflowEvents.Where(x => x.EventTypeId == 6).First().EventTypeId.ShouldBeEquivalentTo(request.EventTypeId);
        countOfUpdate.ShouldBeEquivalentTo(1);
    }
}
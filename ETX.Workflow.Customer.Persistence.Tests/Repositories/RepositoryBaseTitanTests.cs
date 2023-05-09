namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class RepositoryBaseTitanTests
    : IClassFixture<WorkflowContextFactory<WorkflowTitanContextTest>>
{
    private WorkflowTitanContextTest _dbContext = default;
    private readonly RepositoryBase<WorkflowTitanContextTest, CustomerWorkflowEvents> _repositoryBase = default;

    private readonly WorkflowContextFactory<WorkflowTitanContextTest>
        _workflowContextFactory = default;

    public RepositoryBaseTitanTests(
        WorkflowContextFactory<WorkflowTitanContextTest> workflowContextFactory)
    {
        _workflowContextFactory = workflowContextFactory;
        _workflowContextFactory.CreateDatabase();
        _dbContext = _workflowContextFactory.WorkflowContextInstance;
        _repositoryBase = new RepositoryBase<WorkflowTitanContextTest
                                             , CustomerWorkflowEvents>(_dbContext);
    }

    [Fact]
    public void Is_Implement_IRepositoryBase()
    {
        //Assert
        _repositoryBase
            .ShouldBeAssignableTo<IRepositoryBase<WorkflowTitanContextTest, CustomerWorkflowEvents>>();
    }

    [Fact]
    public async Task FindByConditionAsync_Should_Return_IEnumerable()
    {
        //Act
        var result = await _repositoryBase.FindByConditionAsync(
                                          QueryConstants.QUERY_SELECT_CUSTOMERWORKFLOWEVENTS
                                          , default);

        //Assert
        _repositoryBase.ShouldBeAssignableTo<IRepositoryBase<WorkflowTitanContextTest,
                                                            CustomerWorkflowEvents>>();
        result.ShouldBeAssignableTo<IList<CustomerWorkflowEvents>>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildFirstTradeWorkflow_Update")]
    public async Task UpdateByConditionAsync_Should_Return_Updated_Count_Of_One_For_Demo(
        CustomerWorkflowEvents customerWorkflowEvents)
    {
        var result = await _repositoryBase
                           .UpdateByConditionAsync(QueryConstants.QUERY_UPDATE_STATUS_INPROGRESS
                                                   , Util.BuildQueryCondition(customerWorkflowEvents.Id));

        // Assert
        result.ShouldBeAssignableTo<int>();
        result.ShouldBeEquivalentTo(1);
    }
}
namespace ETX.Workflow.Customer.Persistence.Tests.Repositories;

[ExcludeFromCodeCoverage]
public class RepositoryBaseItdbTests
    : IClassFixture<WorkflowContextFactory<WorkflowItdbContextTest>>
{
    private WorkflowItdbContextTest _dbContext = default;
    private readonly RepositoryBase<WorkflowItdbContextTest, CustomerWorkflowEvents> _repositoryBase = default;

    private readonly WorkflowContextFactory<WorkflowItdbContextTest>
        _workflowContextFactory = default;

    public RepositoryBaseItdbTests(
        WorkflowContextFactory<WorkflowItdbContextTest> workflowContextFactory)
    {
        _workflowContextFactory = workflowContextFactory;
        _workflowContextFactory.CreateDatabase();
        _dbContext = _workflowContextFactory.WorkflowContextInstance;
        _repositoryBase = new RepositoryBase<WorkflowItdbContextTest
                                             , CustomerWorkflowEvents>(_dbContext);
    }

    [Fact]
    public void Is_Implement_IRepositoryBase()
    {
        //Assert
        _repositoryBase.ShouldBeAssignableTo
                        <IRepositoryBase<WorkflowItdbContextTest, CustomerWorkflowEvents>>();
    }

    [Fact]
    public async Task FindByConditionAsync_Should_Return_IEnumerable()
    {
        //Act
        var result = await _repositoryBase
                            .FindByConditionAsync(QueryConstants.QUERY_SELECT_CUSTOMERWORKFLOWEVENTS
                                                  , default);

        // Assert
        result.ShouldBeAssignableTo<IList<CustomerWorkflowEvents>>();
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Demo_Update")]
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

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Completed_Update")]
    public async Task WorkflowRequest_BuildRegistrationStatusWorkflow_Completed_Update(
        CustomerWorkflowEvents customerWorkflowEvents)
    {
        var queryCondition = Util.BuildQueryCondition("InProgress", customerWorkflowEvents.Id.ToString());
        var result = await _repositoryBase
                           .UpdateByConditionAsync(QueryConstants.QUERY_UPDATE_STATUS_INPROGRESS
                                                   , queryCondition);

        // Assert
        result.ShouldBeAssignableTo<int>();
        result.ShouldBeEquivalentTo(1);
    }

    [Theory]
    [LoadData("CustomerWorkflowRequest_BuildRegistrationStatusWorkflow_Join_Update")]
    public async Task UpdateByConditionAsync_Should_Return_Updated_Count_Of_One_For_Join(
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
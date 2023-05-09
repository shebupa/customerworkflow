namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class WorkflowItdbContextTest : DbContext
{
    public WorkflowItdbContextTest(
        DbContextOptions<WorkflowItdbContextTest> options) : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Seed<CustomerWorkflowEvents>("./Data/WorkflowEventsItdb.json");
        base.OnModelCreating(modelBuilder);
    }
}
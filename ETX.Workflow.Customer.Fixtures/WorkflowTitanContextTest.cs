namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class WorkflowTitanContextTest : DbContext
{
    public WorkflowTitanContextTest(
        DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Seed<CustomerWorkflowEvents>("./Data/WorkflowEventsTitan.json");
        base.OnModelCreating(modelBuilder);
    }
}
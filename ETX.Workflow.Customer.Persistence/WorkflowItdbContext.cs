namespace ETX.Workflow.Customer.Persistence;

[ExcludeFromCodeCoverage]
public class WorkflowItdbContext : DbContext
{
    public WorkflowItdbContext(
            DbContextOptions<WorkflowItdbContext> options) : base(options)
    {
    }

    public DbSet<CustomerWorkflowEvents> CustomerWorkflowEvents { get; set; }

    protected override void OnModelCreating(
            ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerWorkflowEventsSchemaDefinition());
        base.OnModelCreating(modelBuilder);
    }
}
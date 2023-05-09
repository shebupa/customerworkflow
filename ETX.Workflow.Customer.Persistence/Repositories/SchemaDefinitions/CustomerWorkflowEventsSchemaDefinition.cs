namespace ETX.Workflow.Customer.Persistence.Repositories.SchemaDefinitions;

[ExcludeFromCodeCoverage]
public class CustomerWorkflowEventsSchemaDefinition : IEntityTypeConfiguration<CustomerWorkflowEvents>
{
    public void Configure(EntityTypeBuilder<CustomerWorkflowEvents> builder)
    {
        builder.ToTable("CustomerWorkflowEvents", "dbo");

        //Map Table Column.
        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
        builder.Property(p => p.EventTypeId).HasColumnName("EventTypeId");
        builder.Property(p => p.EventDateTime).HasColumnName("EventDateTime");
        builder.Property(p => p.EventDetails).HasColumnName("EventDetails");
        builder.Property(p => p.Status).HasColumnName("Status");

        builder.HasKey(k => k.Id);
    }
}
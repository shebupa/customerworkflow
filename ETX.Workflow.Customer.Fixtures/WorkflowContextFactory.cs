namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class WorkflowContextFactory<TDbContext>
    where TDbContext : DbContext
{
    public TDbContext WorkflowContextInstance;

    public WorkflowContextFactory()
    {
        CreateDatabase();
    }

    private static DbConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }

    public void CreateDatabase()
    {
        var contextOptions = new DbContextOptionsBuilder<TDbContext>()
           .UseSqlite(CreateInMemoryDatabase())
           .EnableSensitiveDataLogging()
           .Options;

        WorkflowContextInstance = (TDbContext)Activator.CreateInstance(typeof(TDbContext), contextOptions);
        WorkflowContextInstance.Database.EnsureCreated();
    }
}
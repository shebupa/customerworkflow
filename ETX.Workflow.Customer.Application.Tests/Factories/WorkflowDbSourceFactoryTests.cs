//namespace ETX.Workflow.Customer.Persistence.Tests.Factories;

//[ExcludeFromCodeCoverage]
//public class CustomerWorkflowDbSourceFactoryTests
//    : IClassFixture<CustomerWorkflowContextFactory<CustomerWorkflowItdbContextTest>>
//{
//    private readonly Mock<ICustomerWorkflowEventsRepository> customerWorkflowEventsRepository = default;
//    private readonly Mock<IRepositoryManagerFactory> _repositoryManagerFactoryMock = default;

//    public CustomerWorkflowDbSourceFactoryTests(
//        CustomerWorkflowContextFactory<CustomerWorkflowItdbContextTest> itdbContext)
//    {
//        _repositoryManagerFactoryMock = new Mock<IRepositoryManagerFactory>();
//    }

//    [Fact]
//    public void Is_Type_ICustomerWorkflowDbSourceFactory()
//    {
//        //Arrange
//        var _customerWorkflowDbSourceFactory = new CustomerWorkflowDbSourceFactory();

//        //Assert
//        _customerWorkflowDbSourceFactory.ShouldBeAssignableTo<ICustomerWorkflowDbSourceFactory>();
//    }

//    [Fact]
//    public void Register_Should_Accept_DataSoure_Type_And_Instance()
//    {
//        //Arrange
//        var _customerWorkflowDbSourceFactory = new CustomerWorkflowDbSourceFactory();

//        //Act
//        _customerWorkflowDbSourceFactory.Register(DataSource.Itdb, new CustomerWorkflowDbSource(
//                                                                       _repositoryManagerFactoryMock.Object));
//    }

//    public void Create_Should_Return_CustomerWorkflowDbSource_Instance_For_DataSource()
//    {
//        //Arrange
//        var _customerWorkflowDbSourceFactory = new CustomerWorkflowDbSourceFactory();
//        _customerWorkflowDbSourceFactory.Register(DataSource.Itdb, new CustomerWorkflowDbSource(
//                                                                       _repositoryManagerFactoryMock.Object));
//        //Act
//        var customerWorkflowDbSource = _customerWorkflowDbSourceFactory.Create(DataSource.Itdb);

//        //Assert
//        customerWorkflowDbSource.ShouldBeAssignableTo<CustomerWorkflowDbSource>();
//    }
//}
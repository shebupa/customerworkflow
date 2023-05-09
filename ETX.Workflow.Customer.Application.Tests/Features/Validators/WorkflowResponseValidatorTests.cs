namespace ETX.Workflow.Customer.Application.Tests.Features.Validators;

public class WorkflowResponseValidatorTests
{
    private readonly Mock<IMetricsLoggerService> _metricsloggerMock = default;

    public WorkflowResponseValidatorTests()
    {
        _metricsloggerMock = new Mock<IMetricsLoggerService>();
    }

    [Fact]
    public void WorkflowResponseValidator_Should_Be_Of_Type_IValidator()
    {
        //Arrange
        var firstDepositResponseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Assert
        firstDepositResponseValidator.ShouldBeAssignableTo<IWorkflowResponseValidator>();
    }

    [Theory]
    [LoadData("CreateFirstDepositCommandResponse_BuildFirstDepositWorkflowEvent_Invalid")]
    public void ValidateAsync_Should_Validate_FirstDeposit_Event_As_False(
        CreateFirstDepositCommandResponse createFirstDepositCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createFirstDepositCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeFalse();
    }

    [Theory]
    [LoadData("CreateRegistrationStatusCommandResponse_BuildRegistrationStatusWorkflowEvent_Invalid")]
    public void ValidateAsync_Should_Validate_RegistrationStatus_Event_As_False(
        CreateRegistrationStatusCommandResponse createRegistrationStatusCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createRegistrationStatusCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeFalse();
    }

    [Theory]
    [LoadData("CreateFirstTradeCommandResponse_BuildFirstTradeWorkflowEvent_Invalid")]
    public void ValidateAsync_Should_Validate_FirstTrade_Event_As_False(
        CreateFirstTradeCommandResponse createFirstTradeCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createFirstTradeCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeFalse();
    }

    [Theory]
    [LoadData("CreateFirstDepositCommandResponse_BuildFirstDepositWorkflowEvent")]
    public void ValidateAsync_Should_Validate_FirstDeposit_Event_As_True(
        CreateFirstDepositCommandResponse createFirstDepositCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createFirstDepositCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeTrue();
    }

    [Theory]
    [LoadData("CreateRegistrationStatusCommandResponse_BuildRegistrationStatusWorkflowEvent")]
    public void ValidateAsync_Should_Validate_RegistrationStatus_Event_As_True(
        CreateRegistrationStatusCommandResponse createRegistrationStatusCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createRegistrationStatusCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeTrue();
    }

    [Theory]
    [LoadData("CreateFirstTradeCommandResponse_BuildFirstTradeWorkflowEvent")]
    public void ValidateAsync_Should_Validate_FirstTrade_Event_As_True(
        CreateFirstTradeCommandResponse createFirstTradeCommandResponse)
    {
        //Arrange
        var responseValidator = new WorkflowResponseValidator(_metricsloggerMock.Object);

        //Act
        var response = responseValidator.IsValidResponseToSend(createFirstTradeCommandResponse);

        //Assert
        response.ShouldBeAssignableTo<bool>();
        response.ShouldBeTrue();
    }
}
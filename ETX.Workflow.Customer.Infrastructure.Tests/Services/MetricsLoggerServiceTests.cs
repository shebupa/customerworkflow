namespace ETX.Workflow.Customer.Infrastructure.Services.Tests;

[ExcludeFromCodeCoverage]
public class MetricsLoggerServiceTests
{
    [Fact]
    public void IsMetricsLogger_Of_Type_IMetricsLoggerService()
    {
        //Arrange
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);

        //Assert
        loggingService.ShouldBeAssignableTo<IMetricsLoggerService>();
    }

    [Fact]
    public void LogDebugMessage_Should_AtleastExecuteOnce()
    {
        //Arrange
        var logMessage = "TestLog";
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);
        metricsLoggerMock.Setup(x => x.LogDebugMessage(logMessage, null));

        //Act
        loggingService.LogDebugMessage(logMessage, null);

        //Assert
        metricsLoggerMock.Verify(x => x.LogDebugMessage(logMessage, null), Times.Once());
    }

    [Fact]
    public void LogInfoMessage_Should_AtleastExecuteOnce()
    {
        //Arrange
        var logMessage = "TestLog";
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);
        metricsLoggerMock.Setup(x => x.LogInfoMessage(logMessage, null));

        //Act
        loggingService.LogInfoMessage(logMessage, null);

        //Assert
        metricsLoggerMock.Verify(x => x.LogInfoMessage(logMessage, null), Times.Once());
    }

    [Fact]
    public void LogWarnMessage_Should_AtleastExecuteOnce()
    {
        //Arrange
        var logMessage = "TestLog";
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);
        metricsLoggerMock.Setup(x => x.LogWarnMessage(logMessage, null));

        //Act
        loggingService.LogWarnMessage(logMessage, null);

        //Assert
        metricsLoggerMock.Verify(x => x.LogWarnMessage(logMessage, null), Times.Once());
    }

    [Fact]
    public void LogErrorMessage_Should_AtleastExecuteOnce()
    {
        //Arrange
        var logMessage = "TestLog";
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);
        metricsLoggerMock.Setup(x => x.LogErrorMessage(logMessage, null));

        //Act
        loggingService.LogErrorMessage(logMessage, null);

        //Assert
        metricsLoggerMock.Verify(x => x.LogErrorMessage(logMessage, null), Times.Once());
    }

    [Fact]
    public void LogFatalMessage_Should_AtleastExecuteOnce()
    {
        //Arrange
        var logMessage = "TestLog";
        var metricsLoggerMock = new Mock<IMetricsLogger>();
        var loggingService = new MetricsLoggerService(metricsLoggerMock.Object);
        metricsLoggerMock.Setup(x => x.LogFatalMessage(logMessage, null));

        //Act
        loggingService.LogFatalMessage(logMessage, null);

        //Assert
        metricsLoggerMock.Verify(x => x.LogFatalMessage(logMessage, null), Times.Once());
    }
}
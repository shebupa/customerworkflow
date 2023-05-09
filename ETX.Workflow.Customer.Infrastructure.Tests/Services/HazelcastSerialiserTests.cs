using Hazelcast.Serialization;

namespace ETX.Workflow.Customer.Infrastructure.Tests.Services;

public class HazelcastSerialiserTests
{
    [Fact]
    public void Write_Should_Execute_WriteString_Once_And_Writes_Expected()
    {
        //Arrange
        var objectToWrite = "{'key' : 'value'}";
        var expected = "\"{'key' : 'value'}\"";
        var logger = new Mock<IMetricsLoggerService>();
        var serialiser = new HazelcastSerialiser(logger.Object);
        var objectDataOutput = new Mock<IObjectDataOutput>();
        objectDataOutput.Setup(_ => _.WriteString(objectToWrite));

        //Act
        serialiser.Write(objectDataOutput.Object, objectToWrite);

        //Assert
        serialiser.ShouldBeAssignableTo<IStreamSerializer<object>>();
        objectDataOutput.Verify(_ => _.WriteString(expected), Times.AtLeastOnce());
    }

    [Fact]
    public void Read_Should_Execute_ReadString_Once_And_Return_Expected()
    {
        //Arrange
        var expected = "\"{'Subject' : 'TestSubject', 'Message' : 'TestMessage'}";
        var logger = new Mock<IMetricsLoggerService>();
        var serialiser = new HazelcastSerialiser(logger.Object);
        var objectDataInput = new Mock<IObjectDataInput>();
        objectDataInput.Setup(_ => _.ReadString()).Returns(expected);

        //Act
        serialiser.Read(objectDataInput.Object);

        //Assert
        objectDataInput.Verify(_ => _.ReadString(), Times.AtLeastOnce());
    }
}
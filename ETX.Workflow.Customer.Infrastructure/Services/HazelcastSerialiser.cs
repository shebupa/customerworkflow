using Newtonsoft.Json;

namespace ETX.Workflow.Customer.Infrastructure.Services;

public class HazelcastSerialiser
      : IStreamSerializer<object>
{
    public int TypeId => 100;

    private readonly IMetricsLoggerService _logger = default;

    public HazelcastSerialiser(
       IMetricsLoggerService logger)
    {
        _logger = logger;
    }

    public void Dispose()
    { }

    public void Write(
        IObjectDataOutput output,
        object obj)
    {
        try
        {
            var jsonObject = JsonConvert.SerializeObject(obj, obj.GetType(), Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
            output.WriteString(jsonObject);

            _logger.LogInfoMessage($"End Customer Workflow Request\n" +
                                   $"jsonObject={jsonObject}");
        }
        catch (Exception exception)
        {
            _logger.LogErrorMessage($"Failed in Serialiser Write " +
                                    $"exception=\"{exception.Message}\"");
        }
    }

    public object Read(
            IObjectDataInput input)
    {
        var jsonObject = input.ReadString();
        return jsonObject;
    }
}
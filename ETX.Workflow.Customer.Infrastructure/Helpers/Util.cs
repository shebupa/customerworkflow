namespace ETX.Workflow.Customer.Infrastructure.Helpers;

public class Util
{
    public string[] ConfigureHazelcastNetwork(
       string networkDetails,
       string port
       )
    {
        var clusterDetails = new string[networkDetails.Split(";").Length];
        int index = 0;
        networkDetails.Split(";").ToList().ForEach(_ =>
        {
            clusterDetails[index] = $"{EventBusConstants.HAZELCAST_NETWORK_ADDRESS}.{index}={_}:{port}";
            index++;
        });

        return clusterDetails;
    }

    public virtual IHazelcastClient ConnectToHazelcast(
       IStreamSerializer<object> serialiser,
       BrokerSettings brokerSettings,
       IMetricsLoggerService logger
       )
    {
        HazelcastOptionsBuilder hazelcastOptionsBuilder = new HazelcastOptionsBuilder();
        hazelcastOptionsBuilder.With(ConfigureHazelcastNetwork(
                                      brokerSettings.HostName,
                                      brokerSettings.Port));
        var hazelcastOptions = hazelcastOptionsBuilder.Build();
        hazelcastOptions.ClusterName = brokerSettings.ClusterName;
        hazelcastOptions.Serialization.GlobalSerializer.Creator = () => serialiser;

        //Add Log

        //Blocking thread, executes as part hosting
        //Hazelcast messaging service connection.
        var client = HazelcastClientFactory.StartNewClientAsync(hazelcastOptions).Result;
        return client;
    }
}
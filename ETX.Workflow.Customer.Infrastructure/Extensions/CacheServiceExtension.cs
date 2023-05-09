namespace ETX.Workflow.Customer.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class CacheServiceExtension
{
    public static void AddCacheContext(
           this IServiceCollection services,
           IConfiguration configuration)
    {
        var cacheSettings = new CacheSettings();
        configuration.Bind(BrokerCacheConstants.CACHE_SERVICE, cacheSettings);
        services.AddSingleton(cacheSettings);

        var logger = services.BuildServiceProvider().GetService<IMetricsLoggerService>();
        try
        {
            var hazelcastClient = services.BuildServiceProvider().GetService<IHazelcastClient>();
            if (hazelcastClient == null)
            {
                var util = new Util();
                var client = util.ConnectToHazelcast(
                                  default //new GlobalJsonSerializer()
                                  , cacheSettings
                                  , logger);
                services.AddSingleton(client);
            }

            //Add Log
            logger.LogInfoMessage($"message=Connected to Cache Broker Service" +
                                  $"address={EventBusConstants.HAZELCAST_NETWORK_ADDRESS} " +
                                  $"hostName={cacheSettings.HostName} " +
                                  $"port={cacheSettings.Port} " +
                                  $"clusterName={cacheSettings.ClusterName}");
        }
        catch (Exception exception)
        {
            logger.LogErrorMessage($"Error connecting to Cache Broker Service " +
                                   $"address={EventBusConstants.HAZELCAST_NETWORK_ADDRESS} " +
                                   $"hostName={cacheSettings.HostName} " +
                                   $"port={cacheSettings.Port} exception={exception.Message} " +
                                   $"innerException={exception?.InnerException}" +
                                   $"clusterName={cacheSettings.ClusterName} " +
                                   $"message={exception.Message} " +
                                   $"innerException={exception?.InnerException}");
        }
    }
}
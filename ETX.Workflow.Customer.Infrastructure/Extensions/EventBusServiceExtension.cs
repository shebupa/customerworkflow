namespace ETX.Workflow.Customer.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class EventBusServiceExtension
{
    public static void AddEventBusService(this
           IServiceCollection services
           , IConfiguration configuration
           , IHostEnvironment environment
            )
    {
        //Event Bus Service
        var hazelcastClient = CreateEventBusService(services
                                                    , configuration
                                                    , environment
                                                    , EventBusConstants.CUSTOMER_EVENT_SERVICE_BUS);

        services.AddSingleton(hazelcastClient);
        services.AddSingleton<IEventBusBrokerService, HazelcastEventBusBrokerService>();
    }

    public static void AddPublisherService(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddScoped<IPublisherService, EventPublisherService>();
    }

    public static void AddEventServerServerPolicy(
            this IServiceCollection services
            , IConfiguration configuration)
    {
        services.AddScoped<IEventBusServicePolicyFacade, HazelcastEventBusServicePolicyFacade>();
        services.AddScoped<IEventBusBrokerService, HazelcastEventBusServicePolicy>();
    }

    private static IHazelcastClient CreateEventBusService(
            IServiceCollection services
            , IConfiguration configuration
            , IHostEnvironment environment
            , string eventBusConfig)
    {
        var eventBusSettings = new EventBusSettings();
        configuration.Bind(eventBusConfig, eventBusSettings);
        eventBusSettings.RegistrationStatusTopicName = (environment.IsDevelopment())
                                             ? System.Net.Dns.GetHostName() + "-" + eventBusSettings.RegistrationStatusTopicName
                                             : eventBusSettings.RegistrationStatusTopicName;

        eventBusSettings.FirstTradeTopicName = (environment.IsDevelopment())
                                     ? System.Net.Dns.GetHostName() + "-" + eventBusSettings.FirstTradeTopicName
                                     : eventBusSettings.FirstTradeTopicName;

        eventBusSettings.FirstDepositTopicName = (environment.IsDevelopment())
                                       ? System.Net.Dns.GetHostName() + "-" + eventBusSettings.FirstDepositTopicName
                                       : eventBusSettings.FirstDepositTopicName;

        services.AddSingleton(eventBusSettings);
        var logger = services.BuildServiceProvider().GetService<IMetricsLoggerService>();

        try
        {
            var util = new Util();
            var hazelcastClient = util.ConnectToHazelcast(
                               new HazelcastSerialiser(logger)
                              , eventBusSettings
                              , logger);

            logger.LogInfoMessage($"message=Connected to EventBus Broker Service" +
                                  $"address={EventBusConstants.HAZELCAST_NETWORK_ADDRESS} " +
                                  $"hostName={eventBusSettings.HostName} " +
                                  $"port={eventBusSettings.Port} " +
                                  $"RegistrationStatusTopicName={eventBusSettings.RegistrationStatusTopicName} " +
                                  $"FirstDepositTopicName={eventBusSettings.FirstDepositTopicName} " +
                                  $"FirstTradeTopicName={eventBusSettings.FirstTradeTopicName} " +
                                  $"clusterName={eventBusSettings.ClusterName}");

            //return new HazelcastEventBusBrokerService(client, eventBusSettings, logger);
            return hazelcastClient;
        }
        catch (Exception exception)
        {
            logger.LogErrorMessage($"Error connecting to EventBus Broker Service, " +
                                   $"address={eventBusSettings} " +
                                   $"hostName={eventBusSettings.HostName} " +
                                   $"port={eventBusSettings.Port} " +
                                   $"RegistrationStatusTopicName={eventBusSettings.RegistrationStatusTopicName} " +
                                   $"FirstDepositTopicName={eventBusSettings.FirstDepositTopicName} " +
                                   $"FirstTradeTopicName={eventBusSettings.FirstTradeTopicName} " +
                                   $"exception=\"{exception.Message}\" " +
                                   $"innerException=\"{exception?.InnerException}\"");
        }

        return null;
    }
}
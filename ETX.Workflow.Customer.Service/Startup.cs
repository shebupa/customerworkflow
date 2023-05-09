namespace ETX.Workflow.Customer.Service;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(
            IConfiguration configuration,
            IHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(
        IServiceCollection services
        )
    {
        services.AddMediatR(typeof(CreateRegistrationStatusCommandHandler).GetTypeInfo().Assembly);
        services.AddAutoMapper(typeof(CreateRegistrationStatusCommandRequest));
        services.AddMetricsLoggerService(Configuration);
        services.AddSqlServerContext(Configuration);
        services.AddPersistanceFactories();
        services.AddApplicationServiceFactories();
        services.AddEventBusService(Configuration, Environment);
        services.AddApplicationService(Configuration);
        services.AddCacheContext(Configuration);
        services.AddPublisherService(Configuration);
        services.AddEventServerServerPolicy(Configuration);
        services.AddHostedService<CustomerRegistrationBackgroudService>();
        services.AddHostedService<CustomerTradeBackgroudService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
    }
}